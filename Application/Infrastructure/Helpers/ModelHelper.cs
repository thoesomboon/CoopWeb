using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Coop.Infrastructure.Helpers
{
    public static class ModelHelper<TTarget> where TTarget : class, new()
    {
        /// <summary>
        /// Copies all readable properties from the source to a new instance
        /// of TTarget.
        /// </summary>
        public static TTarget Apply<TSource>(TSource source) where TSource : class
        {
            return PropertyCopier<TSource>.Copy(source);
        }

        /// <summary>
        /// Static class to efficiently store the compiled delegate which can
        /// do the copying. We need a bit of work to ensure that exceptions are
        /// appropriately propagated, as the exception is generated at type initialization
        /// time, but we wish it to be thrown as an ArgumentException.
        /// </summary>
        private static class PropertyCopier<TSource> where TSource : class
        {
            private static readonly Func<TSource, TTarget> Copier;
            private static readonly Exception InitializationException;

            internal static TTarget Copy(TSource source)
            {
                if (InitializationException != null)
                {
                    throw InitializationException;
                }
                return source == null ? null : Copier(source);
            }
            static PropertyCopier()
            {
                try
                {
                    Copier = BuildCopier();
                    InitializationException = null;
                }
                catch (Exception e)
                {
                    Copier = null;
                    InitializationException = e;
                }
            }

            private static Func<TSource, TTarget> BuildCopier()
            {
                var sourceParameter = Expression.Parameter(typeof(TSource), "source");
                var bindings = new List<MemberBinding>();
                foreach (var sourceProperty in typeof(TSource).GetProperties())
                {
                    if (!sourceProperty.CanRead)
                    {
                        continue;
                    }
                    var targetProperty = typeof(TTarget).GetProperty(sourceProperty.Name);
                    if (targetProperty == null)
                    {
                        //except target null property
                        continue;
                        //throw new ArgumentException("Property " + sourceProperty.Name +
                        //                            " is not present and accessible in " + typeof(TTarget).FullName);
                    }
                    if (!targetProperty.CanWrite)
                    {
                        throw new ArgumentException("Property " + sourceProperty.Name + " is not writable in " +
                                                    typeof(TTarget).FullName);
                    }
                    if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    {
                        throw new ArgumentException("Property " + sourceProperty.Name + " ("+sourceProperty.PropertyType +") has an incompatible type in " +
                                                    typeof(TTarget).FullName + " ("+sourceProperty.PropertyType +")");
                    }
                    bindings.Add(Expression.Bind(targetProperty, Expression.Property(sourceParameter, sourceProperty)));
                }
                Expression initializer = Expression.MemberInit(Expression.New(typeof(TTarget)), bindings);
                return Expression.Lambda<Func<TSource, TTarget>>(initializer, sourceParameter).Compile();
            }
        }
    }
}