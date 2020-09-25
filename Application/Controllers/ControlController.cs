using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using Coop.Configuration;
using Coop.Entities;
using Coop.Infrastructure.Extensions;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using Coop.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Coop.Controllers
{
    //[OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ControlController : BaseController
    {
        private readonly CultureInfo _thTh = new CultureInfo("en-US");
        private readonly IUnitOfWork _unitOfWork;
        #region "unitOfWork"
        public ControlController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion
    }
}