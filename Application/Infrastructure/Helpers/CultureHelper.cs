#region Using

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Coop.Infrastructure.Helpers
{
    public static class CultureHelper
    {
        // Include ONLY cultures you are implementing as views
        private static readonly Dictionary<String, bool> Cultures = new Dictionary<string, bool>
                                                                        {
                                                                            {"en-US", true},
                                                                            // first culture is the DEFAULT
                                                                            {"th-TH", false}
                                                                        };


        /// <summary>
        ///   Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture "en-US"
        /// </summary>
        /// <param name="name"> Culture's name (e.g. en-US) </param>
        public static string GetValidCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture(); // return Default culture

            if (Cultures.ContainsKey(name))
                return name;

            // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB", 
            // the function will return closes match that is "en-US" because at least the language is the same (ie English)            
            foreach (var c in Cultures.Keys.Where(c => c.StartsWith(name.Substring(0, 2))))
                return c;


            // else             
            return GetDefaultCulture(); // return Default culture as no match found
        }


        /// <summary>
        ///   Returns default culture name which is the first name decalared (e.g. en-US)
        /// </summary>
        /// <returns> </returns>
        public static string GetDefaultCulture()
        {
            return Cultures.Keys.ElementAt(0); // return Default culture
        }


        /// <summary>
        ///   Returns "true" if view is implemented separately, and "false" if not.
        ///   For example, if "es-CL" is true, then separate views must exist e.g. Index.es-cl.cshtml, About.es-cl.cshtml
        /// </summary>
        /// <param name="name"> Culture's name </param>
        /// <returns> </returns>
        public static bool IsViewSeparate(string name)
        {
            return Cultures.ContainsKey(name) && Cultures[name];
        }
    }
}