//****function Convert Type JavaScript(String)To(Bool) By Jak 2013-05-15*****************************************//
String.prototype.bool = function () {
    return (/^true/i).test(this);
};
//***************************************************************************************************************//

var validation = {
    isValidMemberId: function (str) {
        if (str * 1 == 0 || str == undefined) {
            return false;
        }
        //var pattern = new RegExp('^[0-9]{6}$');
        var strMemberIdReplace = str.replace("-", "");
        strMemberIdReplace = strMemberIdReplace.replace("_", "");
        var MemberIdLength = strMemberIdReplace.length;
        if (MemberIdLength != 5) {
            return alert("เลขที่สมาชิกไม่ถูกต้อง");
        }
        if (MemberIdLength == 5) {
            var MemberIdValue = 0;
            MemberIdValue += strMemberIdReplace[0] * 1;
            MemberIdValue += strMemberIdReplace[1] * 2;
            MemberIdValue += strMemberIdReplace[2] * 1;
            MemberIdValue += strMemberIdReplace[3] * 2;
            MemberIdValue += strMemberIdReplace[4] * 1;
            //MemberIdValue += strMemberIdReplace[5] * 2;
            MemberIdValue = MemberIdValue % 10;

            //strMemberIdReplace = 10 - temp;
            // ถ้าไม่เท่ากัน Return false ถ้าเท่ากัน Return true

            //Suwan comment out ก่อน 2020-6-6
            //if (MemberIdValue != (strMemberIdReplace[strMemberIdReplace.length - 1] * 1)) {
            //    return alert("เลขที่สมาชิกไม่ถูกต้อง");
            //}
        }
        return str;
    },
    isValidLoanId: function (str) {
        var pattern = new RegExp('^[ก-ฮA-Za-z0-9]{9}$');
        return pattern.test(str);
    },
    isValidAccountNo: function (str) {
        var stracccountNoReplace = str.replace("-", "");
        stracccountNoReplace = stracccountNoReplace.replace("_", "");
        var accountNoLength = stracccountNoReplace.length;
        if (accountNoLength != 8) {
            return alert("เลขที่บัญชีไม่ถูกต้อง");
        }
        else {
            var accountNoValue = 0;
            accountNoValue += stracccountNoReplace[0] * 7;
            accountNoValue += stracccountNoReplace[1] * 6;
            accountNoValue += stracccountNoReplace[2] * 5;
            accountNoValue += stracccountNoReplace[3] * 4;
            accountNoValue += stracccountNoReplace[4] * 3;
            accountNoValue += stracccountNoReplace[5] * 2;
            accountNoValue += stracccountNoReplace[6] * 1;
            accountNoValue += stracccountNoReplace[7] * 0;
            var temp = accountNoValue % 10;
            accountNoValue = 10 - temp;
            accountNoValue = (accountNoValue == 10) ? 0 : accountNoValue;
            // ถ้าไม่เท่ากัน Return false ถ้าเท่ากัน Return true
            //Suwan comment out ก่อน 2020-6-6
            //if (accountNoValue != (stracccountNoReplace[stracccountNoReplace.length - 1] * 1)) {
            //    return alert("เลขที่บัญชีไม่ถูกต้อง");
            //}
        }
        return str;
    },
    isValidDateTH: function (strDate) {
        /// 28/2/2015
        //debugger
        var isValidDate = true;
        if (strDate == null || strDate.length < 6) { return false; }
        var splitDate = strDate.split("/");

        var dd = splitDate[0] * 1;
        var month = (splitDate[1] * 1);
        var year = (splitDate[2] * 1) - 543;
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (month == 1 || month > 2) {
            if (dd > ListofDays[month - 1]) {
                //alert('Invalid date format!');
                isValidDate = false;
            }
        }
        if (month == 2) {
            var lyear = false;
            if ((!(year % 4) && year % 100) || !(year % 400)) {
                lyear = true;
            }
            if ((lyear == false) && (dd >= 29)) {
                //alert('Invalid date format!');
                isValidDate = false;
            }
            if ((lyear == true) && (dd > 29)) {
                //alert('Invalid date format!');
                isValidDate = false;
            }
        }
        return isValidDate;
    }
};

var conversion = {
    DateInBE: function (strDate) {
        //debugger
        if (strDate == null || strDate == "" || strDate.length < 6) { return ""; }
        // DateInBE
        var parsedDate = "";
        if (isNaN(Date.parse(strDate))) { ///if Case: strDate: "/Date(1049821200000)/"
            parsedDate = new Date(parseInt(strDate.substr(6)));
        } else {
            parsedDate = new Date(strDate) /// Case : 2/28/2015 12:00:00 AM ==> ///Date.parse(strDate) = 1388077200000	Number
        }
        var chkNaN = Date.parse(parsedDate);
        //if (chkNaN == 0) {            
        //    chkNaN = Date.parse(parsedDate);
        //}
        if (isNaN(chkNaN)) { return ""; }
        //debugger
        var returnDate = [(parsedDate.getDate().padZero()), (parsedDate.getMonth() + 1).padZero(), ((parsedDate.getFullYear() * 1) + 543)].join('/');
        //var returnDate = [Number.padZero(parsedDate.getDate()), Number.padZero(parsedDate.getMonth() + 1), ((parsedDate.getFullYear() * 1) + 543)].join('/');
        //var returnDate = ("0" + parsedDate.getDate()).slice(-2) + '/' + ("0" + (parsedDate.getMonth() + 1)).slice(-2) + '/' + ((parsedDate.getFullYear() * 1) + 543);
        /// return value exp : 28/2/2015
        return returnDate;
    },
    DateInCE: function (strMaskedDate) {
        /// Get 28/2/2015
        // DateInCE
        //debugger
        if (strMaskedDate == null || strMaskedDate.length < 6) { return ""; }
        //debugger
        var splitDate = strMaskedDate.split("/");
        //debugger
        var parsedDate = new Date(splitDate[1] + '/' + splitDate[0] + '/' + splitDate[2]);
        //debugger
        var chkNaN = Date.parse(parsedDate);
        if (isNaN(chkNaN)) { return ""; }
        //debugger
        //DD MM YY
        //var returnDate =  parsedDate.getDate() + '/' + Number(parsedDate.getMonth() + 1) + '/' +((parsedDate.getFullYear() * 1) - 543);
        //MM DD YY
        var returnDate = Number(parsedDate.getMonth() + 1) + '/' + parsedDate.getDate() + '/' + ((parsedDate.getFullYear() * 1) - 543);
        //YY MM DD
        //var returnDate = ((parsedDate.getFullYear() * 1) - 543) + '/' + Number(parsedDate.getMonth() + 1) + '/' + parsedDate.getDate();
        //var DateMDY = Number(parsedDate.getMonth() + 1) + '/' + parsedDate.getDate() + '/' + ((parsedDate.getFullYear() * 1) - 543);
        //DateTime returnDate = DateTime.ParseExact(DateMDY, "MM/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
        /// return value exp : 2/28/2015
      //debugger
      return returnDate;
    },

    //DateInBEColumn: function (strDate) {
    //    /Exp: strDate => Mon Feb 24 2014 00:00:00 GMT+0700 (SE Asia Standard Time)
    //    if (strDate == null || strDate.length < 6) { return ""; }
    //    var parsedDate = new Date(strDate);
    //    var chkNaN = Date.parse(strDate);
    //    if (chkNaN == 0) {
    //        parsedDate = new Date(strDate)
    //        chkNaN = Date.parse(parsedDate);
    //    }
    //    if (isNaN(chkNaN)) { return ""; }

    //    var returnDate = [(parsedDate.getDate().padZero()), (parsedDate.getMonth() + 1).padZero(), ((parsedDate.getFullYear() * 1) + 543)].join('/');
    //    return returnDate;
    //},
    toMaskMemberId: function (strMemberId) {  
        ///#####-#
        if (strMemberId == null || strMemberId.length != 7) { return ""; }
        var tempStr = strMemberId.toString();
        var returnMaskedMemberId = tempStr.substr(0, tempStr.length - 1) + '-' + tempStr.substr(tempStr.length - 1);
        return returnMaskedMemberId;
    },
    //toMaskLoanId: function (strLoanId) {
    //    ///&&-#####/##
    //    if (strLoanId == null || strLoanId.length != 9) { return ""; }
    //    var tempStr = strLoanId.toString();
    //    var returnMaskedLoanId = (tempStr.substr(0, 2) + '-' + tempStr.substr(2, tempStr.length - 4) + '/' + tempStr.substr(tempStr.length - 2));
    //    return returnMaskedLoanId;
    //},
    toMaskLoanId: function (strLoanId) {
        ///&&-##-#####  Edit 07/02/2562 By Nee
        if (strLoanId == null || strLoanId.length != 9) { return ""; }
        var tempStr = strLoanId.toString();
        var returnMaskedLoanId = (tempStr.substr(0, 2) + '-' + tempStr.substr(2, 2) + '-' + tempStr.substr(4,5));
        return returnMaskedLoanId;      
    },

    toMaskReferenceNo: function (strReferenceNo) {
        ///##-######-##
        if (strReferenceNo == null || strReferenceNo.length != 10) { return ""; }
        var tempStr = strReferenceNo.toString();
        var returnMaskedReferenceNo = (tempStr.substr(0, 2) + '-' + tempStr.substr(2, tempStr.length - 4) + '-' + tempStr.substr(tempStr.length - 2));
        return returnMaskedReferenceNo;
    },

    toMaskReceiptNo: function (strReceiptNo) {
        ///######/##
        if (strReceiptNo == null || strReceiptNo.length != 8) { return ""; }
        var tempStr = strReceiptNo.toString();
        var returnMaskedReceiptNo = (tempStr.substr(0, 6) + '/' + tempStr.substr(6, tempStr.length - 1));
        return returnMaskedReceiptNo;
    },

    toMaskAccountNo: function (strAccountNo) {
        ///##-######-##(account_no.toString().substr(0, 2)#-#=account_no.toString().substr(2, account_no.toString().length - 2)#-#=account_no.toString().substr(account_no.toString().length - 1))
        if (strAccountNo == null || strAccountNo.length != 10) { return ""; }
        var tempStr = strAccountNo.toString();
        var returnMaskedAccountNo = (tempStr.substr(0, 2) + '-' + tempStr.substr(2, tempStr.length - 4) + '-' + tempStr.substr(tempStr.length - 2));
        return returnMaskedAccountNo;
    },

    toMaskIdCard: function (strIdCard) {
        ///#-####-#####-##-#
        if (strIdCard == null || strIdCard.length != 13) { return ""; }
        var tempStr = strIdCard.toString();
        var returnMaskedIdCard = (tempStr.substr(0, 1) + '-' + tempStr.substr(1, 4) + '-' + tempStr.substr(5, 5) + '-' + tempStr.substr(10, 2) + '-' + tempStr.substr(12));
        return returnMaskedIdCard;
    },

    //JsontoDate: function (strDate) {
    //if (strDate == null || strDate == "" || strDate.length < 6) { return ""; }
    //    //debugger
    //var parsedDate = "";
    //if (isNaN(Date.parse(strDate))) { ///if Case: strDate: "/Date(1049821200000)/"
    //    parsedDate = new Date(parseInt(strDate.substr(6)));
    //    //debugger
    //} else {
    //    parsedDate = new Date(strDate);/// Case : 2/28/2015 12:00:00 AM ==> ///Date.parse(strDate) = 1388077200000	Number
    //    //debugger
    //}
    //var chkNaN = Date.parse(parsedDate);

    //if (isNaN(chkNaN)) { return ""; }

    //var returnDate = [(parsedDate.getMonth() + 1).padZero(), (parsedDate.getDate().padZero()), ((parsedDate.getFullYear() * 1))].join('/');
    //return returnDate;
    //},
    toMaskRequestNo: function (strNo) {
        ///&&-00000/00  Add new 03/07/2562 By Keen
        if (strNo == null || strNo.length != 9) { return ""; }
        var tempStr = strNo.toString();
        var returnMaskedRequestNo = (tempStr.substr(0, 2) + '-' + tempStr.substr(2, 5) + '/' + tempStr.substr(7, 2));
        return returnMaskedRequestNo;
    },
};

var conversionDate = {
    DateInBE: function (strDate) {
        //debugger
        if (strDate == null) { return ""; }
        var returnDate = [(strDate.getDate().padZero()), (strDate.getMonth() + 1).padZero(), ((strDate.getFullYear() * 1) + 543)].join('/');
        //debugger
        return returnDate;
    },
    DateInCE: function (strMaskedDate) {
        /// Get 28/2/2015
        //debugger
        if (strMaskedDate == null) { return ""; }
        var splitDate = strMaskedDate.split("/");
        var parsedDate = new Date(splitDate[1] + '/' + splitDate[0] + '/' + splitDate[2]);
        var chkNaN = Date.parse(parsedDate);
        if (isNaN(chkNaN)) { return ""; }
        //debugger
        var returnDate = Number(parsedDate.getMonth() + 1) + '/' + parsedDate.getDate() + '/' + ((parsedDate.getFullYear() * 1) - 543);
        return returnDate;
    }
};

Number.prototype.padZero = function (len) {
    var n = String(this);
    var zero = "0";
    if (len === undefined) {
        return (n >= 0 && n < 10) ? zero + n : n;
    }
    len = len || 2; //len = (len === undefined) ? 2 : len;
    for (var i = 0; i < len; ++i) {
        zero += "0";
    }
    return (zero + n).slice(-len);
}

String.prototype.padZero = function (len) {
    var n = String(this);
    var num = n * 1;

    return num.padZero(len);
}

Date._validate = function (value, min, max, name) {
    value = value * 1;
    if (typeof value != "number") {
        return false;
        //throw new TypeError(value + " is not a Number.");
    } else if (value < min || value > max) {
        return false;
        //throw new RangeError(value + " is not a valid value for " + name + ".");
    }
    return true;
};
Date.isLeapYear = function (year) { return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0)); };
Date.getDaysInMonth = function (year, month) { return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month]; };
Date.validateDay = function (n, year, month) { return Date._validate(n, 1, Date.getDaysInMonth(year, month), "days"); };
Date.validateMonth = function (n) { return Date._validate(n, 0, 11, "months"); };
Date.validateYear = function (n) { return Date._validate(n, 1, 9999, "years"); };

//var HoldAmt = parseFloat($("#HoldAmt").val().replace(",", ""));
var numeric = {
    txt2DBL: function (txtIN) {
        //debugger
        if (txtIN == null) { return 0; }
        //debugger
        var DBL = parseFloat(txtIN.replace(",", ""));
        //debugger
        return DBL;
    },
    addCommas: function (numIN) {
        //debugger
        if (numIN != null) {
            //debugger
            var parts = numIN.toString().split(".");
            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return parts.join(".");
        }
    }
};

//function ValidateDateThai(DateThai) {
//    var dateTh = DateThai + '';
//    if (dateTh != null || dateTh != '' || dateTh.length == 10) {
//        var day = dateTh.split('/')[0] * 1;
//        var month = dateTh.split('/')[1] * 1;
//        var year = dateTh.split('/')[2] * 1 - 543;
//        var leap = 0;
//        if (day * 0 == 0 && month * 0 == 0 && year * 0 == 0) {
//            if (year < 0) {
//                return false;
//            }
//            if (month < 1 || month > 12) {
//                return false;
//            }
//            if (day < 1 || day > 31) {
//                return false;
//            }
//            if (year % 4 == 0 || year % 100 == 0 || year % 400 == 0) {
//                leap = 1;
//            }
//            if ((month == 2) && (leap == 1) && (day > 29)) {
//                return false;
//            }
//            if ((month == 2) && (leap != 1) && (day > 28)) {
//                return false;
//            }
//            if ((day > 31) && ((month == 1) || (month == 3) || (month == 5) || (month == 7) || (month == 8) || (month == 10) || (month == 12))) {
//                return false;
//            }
//            if ((day > 30) && ((month == 4) || (month == 6) || (month == 9) || (month == 11))) {
//                return false;
//            }
//            return true;
//        }
//    }
//    return false;