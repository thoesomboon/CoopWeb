using System;
using System.ComponentModel;

namespace SaLPro.Library
{
    /// <summary>
    /// Type of Share & Loan
    /// </summary>
    public class SLType
    {
        /// <summary>
        /// เงินกู้
        /// </summary>
        [Description("เงินกู้/หนี้")]
        public const string LON = "LON";
        /// <summary>
        /// หุ้น
        /// </summary>
        [Description("หุ้น")]
        public const string SHR = "SHR";
        /// <summary>
        /// MEMBER : เกี่ยวกับสมาชิก
        /// </summary>
        [Description("MEMBER : เกี่ยวกับสมาชิก")]
        public const string MEM = "MEM";
        /// <summary>
        /// ATM
        /// </summary>
        [Description("ATM")]
        public const string ATM = "ATM";
        /// <summary>
        /// ROUTE
        /// </summary>
        [Description("ROUTE")]
        public const string RUT = "RUT";
        /// <summary>
        /// Deposit
        /// </summary>
        [Description("ออมทรัพย์")]
        public const string DEP = "DEP";
        /// <summary>
        /// Policy
        /// </summary>
        [Description("ประกัน")]
        public const string POL = "POL";
    }

    /// <summary>
    /// Table Type
    /// </summary>
    public class TType
    {
        /// <summary>
        /// Guarantor
        /// </summary>
        [Description("ผู้ค้ำประกัน")]
        public const string GUAR = "GUAR";
        /// <summary>
        /// Security
        /// </summary>
        [Description("หลักทรัพย์ค้ำประกัน")]
        public const string SEC = "SEC";
        /// <summary>
        /// ShareStatus
        /// </summary>
        [Description("สถานะภาพหุ้น")]
        public const string SHRSTS = "SHRSTS";
        /// <summary>
        /// TitleName
        /// </summary>
        [Description("คำนำหน้าชื่อ")]
        public const string FST = "FST";
        /// <summary>
        /// LoanStatus
        /// </summary>
        [Description("สถานะหนี้")]
        public const string LONSTS = "LONSTS";
        /// <summary>
        /// ResignReason
        /// </summary>
        [Description("เหตุผลลาออก")]
        public const string RESIGN = "RESIGN";

        [Description("ประกัน")]
        public const string INS = "INS";

        [Description("ประเภทประกัน")]
        public const string INS_TP = "INS_TP";
    }

    public class TxnType
    {
        /// <summary>
        /// จ่ายปันผล
        /// </summary>
        [Description("ผู้จ่ายปันผล")]
        public const string DIV = "DIV";
//ATM
//DEP
//DIV
//ECO
//F03
//F06
//F12
//FIX
//LON
//MEM
//RUT
//SAF
//SAV
//SHR
//SPC
//SUB
//USR
    }
}

