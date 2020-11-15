﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class BatDayCloseModel
    {
        //public DepositTypeModel DepositType { get; set; }
        public int CoopID { get; set; }
        //public string DepositTypeID { get; set; }
        //public string DepositTypeName { get; set; }
        public string BudgetYear { get; set; }
        public Nullable<System.DateTime> SystemDate { get; set; }
        public String SystemDateTH { get; set; }
        public System.DateTime NextSystemDate { get; set; }
        public String NextSystemDateTH { get; set; }
        public System.DateTime NextWorkingDay { get; set; }
        public String NextWorkingDayTH { get; set; }
        public int UserId { get; set; }
    }
}
