using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Infrastructure.Helpers
{
    public interface IStatefulStorage
    {
        TValue Get<TValue>(string name);
        TValue GetOrAdd<TValue>(string name, Func<TValue> valueFactory);
    }

    public class StatefulStorageHelper
    {
        public static IStatefulStorage PerApplication
        {
            get { return new StatefulStoragePerApplication(); }
        }

        public static IStatefulStorage PerRequest
        {
            get { return new StatefulStoragePerRequest(); }
        }

        public static IStatefulStorage PerSession
        {
            get { return new StatefulStoragePerSession(); }
        }
    }

    public class StatefulStoragePerApplication : DictionaryStatefulStorage
    {
        // Ambient environment constructor
        public StatefulStoragePerApplication()
            : base(key => HttpContext.Current.Application[key],
                   (key, value) => HttpContext.Current.Application[key] = value)
        {
        }

        // IoC-friendly constructor
        public StatefulStoragePerApplication(HttpApplicationStateBase app)
            : base(key => app[key],
                   (key, value) => app[key] = value)
        {
        }
    }

    public class StatefulStoragePerRequest : DictionaryStatefulStorage
    {
        // Ambient environment constructor
        public StatefulStoragePerRequest()
            : base(() => HttpContext.Current.Items)
        {
        }

        // IoC-friendly constructor
        public StatefulStoragePerRequest(HttpContextBase context)
            : base(() => context.Items)
        {
        }
    }

    public class StatefulStoragePerSession : DictionaryStatefulStorage
    {
        // Ambient environment constructor
        public StatefulStoragePerSession()
            : base(key => HttpContext.Current.Session[key],
                   (key, value) => HttpContext.Current.Session[key] = value)
        {
        }

        // IoC-friendly constructor
        public StatefulStoragePerSession(HttpSessionStateBase session)
            : base(key => session[key],
                   (key, value) => session[key] = value)
        {
        }
    }

    public abstract class DictionaryStatefulStorage : IStatefulStorage
    {
        private readonly Func<string, object> _getter;
        private readonly Action<string, object> _setter;

        protected DictionaryStatefulStorage(Func<IDictionary> dictionaryAccessor)
        {
            _getter = key => dictionaryAccessor()[key];
            _setter = (key, value) => dictionaryAccessor()[key] = value;
        }

        protected DictionaryStatefulStorage(Func<string, object> getter, Action<string, object> setter)
        {
            _getter = getter;
            _setter = setter;
        }

        #region IStatefulStorage Members

        public TValue Get<TValue>(string name)
        {
            return (TValue)_getter(FullNameOf(typeof(TValue), name));
        }

        public TValue GetOrAdd<TValue>(string name, Func<TValue> valueFactory)
        {
            var fullName = FullNameOf(typeof(TValue), name);
            var result = (TValue)_getter(fullName);

            if (Equals(result, default(TValue)))
            {
                result = valueFactory();
                _setter(fullName, result);
            }
            return result;
        }

        #endregion

        protected static string FullNameOf(Type type, string name)
        {
            var fullName = type.FullName;
            if (!String.IsNullOrWhiteSpace(name))
                fullName += "::" + name;

            return fullName;
        }
    }

    public static class StatefulStorageExtensions
    {
        public static TValue Get<TValue>(this IStatefulStorage storage)
        {
            return storage.Get<TValue>(null);
        }

        public static TValue GetOrAdd<TValue>(this IStatefulStorage storage, Func<TValue> valueFactory)
        {
            return storage.GetOrAdd(null, valueFactory);
        }
    }
}