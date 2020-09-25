#region Using

using System;
using System.Web.Mvc;

#endregion

namespace Coop.Infrastructure.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SeparatelyCultureViewAttribute : FilterAttribute
    {

    }
}