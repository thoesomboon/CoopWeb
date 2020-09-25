using System;
using System.ComponentModel;

namespace SaLPro.Library
{
    /// <summary>
    /// DB: T_TXN_CODE :: CD_CODE    
    /// </summary>
    public class CDCODE
    {
        /// <summary>
        /// ATM,AUTO
        /// </summary>
        [Description("ATM,AUTO")]
        public const string A = "A";
        /// <summary>
        /// CREDIT: ฝากเข้า
        /// </summary>
        [Description("CREDIT: ฝากเข้า")]
        public const string C = "C";
        /// <summary>
        /// DEBIT: จ่ายออก
        /// </summary>
        [Description("DEBIT: จ่ายออก")]
        public const string D = "D";
    }
}

