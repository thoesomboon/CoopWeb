using System;
using System.ComponentModel;

namespace Coop.Library
{
    /// <summary>
    /// DB: T_FILESTATUS :: T_FILESTATUS
    /// T_CODE.T_TYPE: FSTS
    /// 
    /// </summary>
    public class FileStatus
    {
        /// <summary>
        /// ปกติ
        /// </summary>
        [Description("ปกติ")]
        public const string A = "A";
        /// <summary>
        /// ปิดแล้ว
        /// </summary>
        [Description("ปิดแล้ว")]
        public const string C = "C";
        /// <summary>
        /// ลบแล้ว
        /// </summary>
        [Description("ลบแล้ว")]
        public const string D = "D";
        /// <summary>
        /// จ่ายแล้ว/Posted
        /// </summary>
        [Description("จ่ายแล้ว/Posted")]
        public const string P = "P";
        /// <summary>
        /// ลาออกแล้ว
        /// </summary>
        [Description("ลาออกแล้ว")]
        public const string R = "R";
        /// <summary>
        /// พ้นสภาพแล้ว
        /// </summary>
        [Description("พ้นสภาพแล้ว")]
        public const string T = "T";
        /// <summary>
        /// ไม่อนุมัติรับสมาชิก
        /// </summary>
        [Description("ไม่อนุมัติรับสมาชิก")]
        public const string U = "U";
        /// <summary>
        /// เข้ามาใหม่
        /// </summary>
        [Description("เข้ามาใหม่")]
        public const string N = "N";
        /// <summary>
        /// ยกเลิก
        /// </summary>
        [Description("ยกเลิก")]
        public const string S = "S";
    }

    /// <summary>
    /// DB: T_LOAN_STATUS :: T_LOAN_STATUS
    /// T_CODE.T_TYPE: LONSTS
    /// </summary>
    public class LoanStatus
    {
        /// <summary>
        /// I - ส่งดอกเบี้ย
        /// </summary>
        [Description("I - ส่งดอกเบี้ย")]
        public const string I = "I";
        /// <summary>
        /// N - ปกติ
        /// </summary>
        [Description("N - ปกติ")]
        public const string N ="N";
        /// <summary>
        /// S - หยุดหนี้
        /// </summary>
        [Description("S - หยุดหนี้")]
        public const string S = "S";
        /// <summary>
        /// H - หยุดหนี้ชั่วคราว
        /// </summary>
        [Description("H - หยุดหนี้ชั่วคราว")]
        public const string T = "T";
    }

    /// <summary>
    /// DB: T_SHARE_STATUS :: T_SHARE_STATUS
    /// T_CODE.T_TYPE: SHRSTS
    /// </summary>
    public class ShareStatus
    {
        /// <summary>
        /// ปกติ
        /// </summary>
        [Description("ปกติ")]
        public const string N = "N";
        /// <summary>
        /// หยุดหยุดหุ้น
        /// </summary>
        [Description("หยุดหนี้หยุดหุ้น")]
        public const string S ="S";
        /// <summary>
        /// หยุดหนี้หยุดหุ้นชั่วคราววคราว
        /// </summary>
        [Description("หยุดหนี้หยุดหุ้นชั่วคราวว")]
        public const string T ="T";
    }

    /// <summary>
    /// DB: T_INSTALL :: T_INSTALL
    /// T_CODE.T_TYPE: INSTAL
    /// </summary>
    public class INSTAL
    {
        /// <summary>
        /// ชำระที่สหกรณ์
        /// </summary>
        [Description("ชำระที่สหกรณ์")]
        public const string C = "C";
        /// <summary>
        /// ชำระที่ต้นสังกัด
        /// </summary>
        [Description("ชำระที่ต้นสังกัด")]
        public const string O = "O";
    }

    /// <summary>
    /// DB: T_PAYMENT :: T_PAYMENT
    /// T_CODE.T_TYPE: PAYM
    /// </summary>
    public class PAYM
    {
        /// <summary>
        /// 0 => NULL
        /// </summary>
        [Description("0 => NULL")]
        public const string N = "0";
        /// <summary>
        /// A => All
        /// </summary>
        [Description("A => All")]
        public const string A = "A";
        /// <summary>
        /// P => Partial
        /// </summary>
        [Description("P => Partial")]
        public const string P = "P";
    }

    /// <summary>
    /// DB: T_INT_CALC :: T_INT_CALC
    /// T_CODE.T_TYPE: INTC
    /// </summary>
    public class INTC
    {
        /// <summary>
        /// 0 => NULL
        /// </summary>
        [Description("0 => NULL")]
        public const string N = "0";
        /// <summary>
        /// รายวัน
        /// </summary>
        [Description("รายวัน")]
        public const string D = "D";
        /// <summary>
        /// รายเดือน
        /// </summary>
        [Description("รายเดือน")]
        public const string M = "M";
    }
    
        /// <summary>
    /// DB: T_PRINCIPLE :: T_PRINCIPLE
    /// T_CODE.T_TYPE: PRNCIP
    /// </summary>
    public class PRNCIP
    {
        /// <summary>
        /// 0 => NULL
        /// </summary>
        [Description("0 => NULL")]
        public const string N = "0";
        /// <summary>
        /// เงินงวดเท่ากัน (ธนาคาร)
        /// </summary>
        [Description("เงินงวดเท่ากัน (ธนาคาร)")]
        public const string B = "B";
        /// <summary>
        /// เงินต้นเท่ากัน (สหกรณ์)
        /// </summary>
        [Description("เงินต้นเท่ากัน (สหกรณ์)")]
        public const string C = "C";
    }

    public class RCPT
    {
        /// <summary>
        /// หุ้น
        /// </summary>
        [Description("หุ้น")]
        public const string SHR = "SHR";
        /// <summary>
        /// หนี้
        /// </summary>
        [Description("หนี้")]
        public const string LON = "LON";
        /// <summary>
        /// เงินฝาก
        /// </summary>
        [Description("เงินฝาก")]
        public const string DEP = "DEP";
        /// <summary>
        /// ค่าธรรมเนียม
        /// </summary>
        [Description("ค่าธรรมเนียม")]
        public const string APP = "APP";
        /// <summary>
        /// เงินหักอื่น ๆ
        /// </summary>
        [Description("เงินหักอื่น ๆ")]
        public const string DED = "DED";
        /// <summary>
        /// สวัสดิการ
        /// </summary>
        [Description("สวัสดิการ")]
        public const string SUB = "SUB";
        /// <summary>
        /// เงินอื่นๆ
        /// </summary>
        [Description("เงินอื่นๆ")]
        public const string IND = "IND";
    }
    public class AuthorizeLevel
    {
        [Description("ผู้ดูแลระบบ")]
        public const int System = 9;
        [Description("หัวหน้าฝ่าย / หัวหน้าแผนก")]
        public const int Manager = 7;
        [Description("พนักงานทั่วไป")]
        public const int Staff = 5;
    }


    public class ContractStatus
    {
        /// <summary>
        /// ปกติ
        /// </summary>
        [Description("ปกติ")]
        public const string A = "A";

        /// <summary>
        /// รับโอน
        /// </summary>
        [Description("รับโอน")]
        public const string R = "R";

        /// <summary>
        /// จบ
        /// </summary>
        [Description("จบ")]
        public const string S = "S";

        /// <summary>
        /// โอนให้ผู้ค้ำ
        /// </summary>
        [Description("โอนให้ผู้ค้ำ")]
        public const string T = "T";

        /// <summary>
        /// ยกเลิก
        /// </summary>
        [Description("ยกเลิก")]
        public const string C = "C";
    }
        public class TxnType
    {
        [Description("เงินฝาก")]
        public const string DEP = "DEP";
        [Description("สัญญาเงินกู้")]
        public const string LON = "LON";
    }
    public class Otx
    {
        [Description("ฝาก/ถอนบัญชีเงินฝาก")]
        public const string DEP = "OtxDeposit";
        [Description("รับชำระเงินกู้")]
        public const string LON = "OtxLoan";
    }
}