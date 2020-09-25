using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Coop.Infrastructure.Helpers
{
    public static class AttributeHelper
    {
        /// <summary>
        /// usage : var author = AttributeHelper.GetPropertyAttributeValue<Book, string, AuthorAttribute, string>(prop => prop.Name, attr => attr.Author);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="valueSelector"></param>
        /// <returns></returns>
        public static TValue GetPropertyAttributeValue<T, TOut, TAttribute, TValue>(
            Expression<Func<T, TOut>> propertyExpression,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)expression.Member;
            var attr = propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return attr != null ? valueSelector(attr) : default(TValue);
        }
        public static string GetDescription<T>(string fieldName)
        {
            string result;
            FieldInfo fi = typeof(T).GetField(fieldName.ToString());
            if (fi != null)
            {
                try
                {
                    object[] descriptionAttrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    DescriptionAttribute description = (DescriptionAttribute)descriptionAttrs[0];
                    result = (description.Description);
                }
                catch
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }

            return result;
        }


        public static string GetFieldAttributeValue<T,U>(string fieldName) where U:Attribute
        {
            string result;
            FieldInfo fi = typeof(T).GetField(fieldName.ToString());
            if (fi != null)
            {
                try
                {
                    object[] attrs = fi.GetCustomAttributes(typeof(U), false);
                    U nameAttr = (U)attrs[0];
                    result = (nameAttr.ToString());
                }
                catch
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }

            return result;
        }
    }


    /// <summary>
    /// USAGE:
    /// //get class properties with attribute [AuthorAttribute]
    /// var props = typeof(Book).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(AuthorAttribute)));
    ///             foreach (var prop in props)
    ///             {
    ///                string value = prop.GetAttributValue((AuthorAttribute a) => a.Name);
    /// }
    /// 
    /// //get class properties with attribute [AuthorAttribute]
    /// var props = typeof(Book).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(AuthorAttribute)));
    /// IList<string> values = props.Select(prop => prop.GetAttributValue((AuthorAttribute a) => a.Name)).Where(attr => attr != null).ToList();
    /// </summary>
    public static class PropertyInfoExtensions
    {
        public static TValue GetAttributValue<TAttribute, TValue>(this PropertyInfo prop, Func<TAttribute, TValue> value) where TAttribute : Attribute
        {
            var att = prop.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return value(att);
            }
            return default(TValue);
        }
    }
}