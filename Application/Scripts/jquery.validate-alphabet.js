(function ($) {
    //    var pasteEventName = ($.browser.msie ? 'paste' : 'input') + ".validatekey";
    var iPhone = (window.orientation != undefined);
    $.fn.extend({
        validatekey: function (validatetype) {
            return this.each(function () {
                var input = $(this);
                var ignore = false;  //Variable for ignoring control keys

                function keydownEvent(e) {
                    var k = e.keyCode;
                    ignore = (k < 16 || (k > 16 && k < 32) || (k > 32 && k < 41));

                    //backspace, delete, and escape get special treatment
                    if (k == 8 || k == 46 || (iPhone && k == 127)) {//backspace/delete
                        return true;
                    } else if (k == 27) {//escape
                        return true;
                    }
                    //return false;
                };

                function keypressEvent(e) {
                    if (ignore) {
                        ignore = false;
                        //Fixes Mac FF bug on backspace
                        return (e.keyCode == 8) ? true : null;
                    }
                    e = e || window.event;
                    var k = e.charCode || e.keyCode || e.which;
                    if (e.ctrlKey || e.altKey || e.metaKey) {//Ignore
                        return false;
                    } else if (validate(e, validatetype, k)) {//typeable characters
                        return false;
                    }
                    return true;
                };

                if (!input.attr("readonly"))
                    input
					.bind("keydown.validatekey", keydownEvent)
					.bind("keypress.validatekey", keypressEvent)
                //					.bind(pasteEventName, function () {
                //					});

                function validate(event, type, key) {
                    var value;
                    var countDot;
                    var countDecimal;
                    var countSpace;
                    var countMiddleLine;
                    var counttickmarks;
                    var newcountDot;
                    var newcountSpace;
                    var newcountMiddleLine;
                    var newcounttickmarks;
                    var r;
                    
                    switch (type) {
                        
                        case "forNumberBenefitShare": 
                            //ป้อนได้เฉพาะตัวเลข 0 - 9   ::::::: 0=48 ,1=49 ,2=50 ,3=51 ,4=52 ,5=53 ,6=54 ,7=55 ,8=56 ,9=57 ใช้เฉพาะ Benefit Share
                            value = event.target.value;
                            if (key == 48) {
                                if (value.length <= 0) {
                                    return true;
                                }
                                if (value.length == 3) {
                                    return true;
                                }
                            }
                            return (key < 48 || key > 57);
                        case "forNumber":
                            //ป้อนได้เฉพาะตัวเลข 0 - 9   ::::::: 0=48 ,1=49 ,2=50 ,3=51 ,4=52 ,5=53 ,6=54 ,7=55 ,8=56 ,9=57
                            return (key < 48 || key > 57);
                        case "forNumberAndOnePoint":
                            //ป้อนได้เฉพาะตัวอักษร 0-9,จุด 1 จุด, Format(999.##)
                            value = event.target.value;
                            countDot = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == ".")
                                    countDot++;
                            }
                            newcountDot = countDot;
                            if (value.length >= 3 && countDot == 0 && key != 46) {
                                return true;
                            }
                            if (newcountDot > 0 && key == 46)
                                return true;
                            if ((key < 48 || key > 57) && key != 46) {
                                return true;
                            }
                            if (value.length == 0 && key == 46) {
                                return true;
                            }
                            break;
                        case "forNumberAndOnePointAndTwoDecimal":
                            //ป้อนได้เฉพาะตัวอักษร 0-9,จุด 1 จุด, Format(999.##)
                            value = event.target.value;
                            countDot = 0;
                            countDecimal = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == ".")
                                {
                                    countDot++;
                                }
                                if (countDot > 0)
                                {
                                    countDecimal++;
                                }
                            }
                            newcountDot = countDot;
                            if (value.length >= 3 && countDot == 0 && key != 46) {
                                return true;
                            }
                            if (newcountDot > 0 && key == 46)
                                return true;
                            if ((key < 48 || key > 57) && key != 46) {
                                return true;
                            }
                            if (value.length == 0 && key == 46) {
                                return true;
                            }
                            if (countDecimal > 2)
                            {
                                return true;
                            }
                            break;
                        case "forAlphabetEnglish":
                            //ป้อนได้เฉพาะตัวอักษร A-Z , a-z
                            return (key < 65 || key > 122);
                        case "forAlphabetEnglishAllspecialCharAndNumberAndSpace":
                            //ป้อนได้เฉพาะตัวอักษร A-Z , a-z , ตัวเลข 0-9 , อักขระพิเศษทั้งหมด และspace
                            return ((key < 65 || key > 122) && /*(key < 48 || key > 57) &&*/ (key < 32 || key > 127) /*&& key != 32 && key != 46*/);
                        case "forAlphabetThaiAllspecialCharAndNumberAndSpace":
                            //ป้อนได้เฉพาะตัวอักษร  ก-ฮ + สระ, ตัวเลข 0 - 9 , อักขระพิเศษทั้งหมด และspace
                            return ((key < 3585 || key > 3642) && (key < 3648 || key > 3662) && (key > 32 && key > 66));
                            //break;
                        case "forAlphabetThai":
                            //ป้อนได้เฉพาะตัวอักษรไทยพ่อขุนรามคำแหงมหาราช และสระ
                            return ((key < 3585 || key > 3642) && (key < 3648 || key > 3662));
                        case "forAlphabetEnglishAndNumber":
                            //ป้อนได้เฉพาะตัวอักษร A-Z , a-z , ตัวเลข 0 - 9
                            if (key == 91 || key == 92 || key == 93 || key == 94 || key == 95) {
                                return true;
                            }
                            return ((key < 65 || key > 122) && (key < 48 || key > 57));
                        case "forAlphabetThaiAndNumber":
                            //ป้อนได้เฉพาะตัว ก-ฉ , ตัวเลข 0 - 9
                            return ((key < 3585 || key > 3642) && (key < 3648 || key > 3662) && (key < 48 || key > 57));
                        case "forAlphabetThaiAndEnglish":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ                      
                            return ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662));
                        case "forAlphabetThaiAndEnglishAndNumber":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ, ตัวเลข 0 - 9
                            return ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662) && (key < 48 || key > 57));
                        case "forAlphabetEnglishAndPoint":
                            //ป้อนได้เฉพาะตัวอักษร A-Z , a-z,จุด
                            return (key != 46 && (key < 65 || key > 122));
                        case "forAlphabetThaiAndEnglishTiltle":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ + จุด
                            return (key != 46 && ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662)));
                        case "forBackspace":
                            //กดได้เฉพาะ backspace กับ delete
                            return (key != 116 || key != 8);
                        case "forAlphabetThaiAndEnglishLastName":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ + จุด1จุด+เคาะ2ครั้ง
                            value = event.target.value;
                            countDot = 0;
                            countSpace = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                                if (value.substring(r, r + 1) == ".")
                                    countDot++;
                            }
                            newcountDot = countDot;
                            newcountSpace = countSpace;
                            if (newcountDot > 0 && key == 46)
                                return true;
                            if (newcountSpace > 1 && key == 32)
                                return true;
                            if ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662) && key != 32 && key != 46) {
                                return true;
                            }
                            break;
                        case "forAlphabetEnglishLastName":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z + จุด1จุด+เคาะ2ครั้ง
                            value = event.target.value;
                            countDot = 0;
                            countSpace = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                                if (value.substring(r, r + 1) == ".")
                                    countDot++;
                            }
                            newcountDot = countDot;
                            newcountSpace = countSpace;
                            if (newcountDot > 0 && key == 46)
                                return true;
                            if (newcountSpace > 1 && key == 32)
                                return true;
                            if ((key < 65 || key > 90) && (key < 97 || key > 122) && key != 32 && key != 46) {
                                return true;
                            }
                            break;
                        case "forAlphabetThaiAndEnglishForEmployeeLastName":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ, - (ได้ 1 ครั้ง)
                            value = event.target.value;
                            counttickmarks = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == "-")
                                    counttickmarks++;
                            }
                            newcounttickmarks = counttickmarks;
                            if (newcounttickmarks > 0 && key == 45)
                                return true;
                            if ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662) && key != 45) {
                                return true;
                            }
                            break;
                        case "forAlphabetEnglishAndPointAndSpace":
                            //ป้อนได้เฉพาะตัวอักษร A-Z , a-z,จุด,ช่องว่าง 1 ช่องว่าง
                            value = event.target.value;
                            countDot = 0;
                            countSpace = 0;
                            countMiddleLine = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                                if (value.substring(r, r + 1) == ".")
                                    countDot++;
                            }
                            newcountDot = countDot;
                            newcountSpace = countSpace;
                            newcountMiddleLine = countMiddleLine;
                            if (newcountDot > 0 && key == 46)
                                return true;
                            if (newcountSpace > 0 && key == 32)
                                return true;
                            if ((key < 65 || key > 122) && key != 32 && key != 46) {
                                return true;
                            }
                            break;
                        case "forAlphabetEnglishAndPointAndSpaceAndScroll":
                            //ป้อนได้เฉพาะตัวอักษร A-Z , a-z,จุด,"-",ช่องว่าง
                            value = event.target.value;
                            countDot = 0;
                            countSpace = 0;
                            countMiddleLine = 0;

                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                                if (value.substring(r, r + 1) == ".")
                                    countDot++;
                                if (value.substring(r, r + 1) == "-")
                                    countMiddleLine++;
                            }
                            newcountDot = countDot;
                            newcountSpace = countSpace;
                            newcountMiddleLine = countMiddleLine;
                            if (newcountDot > 0 && key == 46)
                                return true;
                            if (newcountSpace > 0 && key == 32)
                                return true;
                            if (newcountMiddleLine > 0 && key == 45)
                                return true;
                            if ((key < 65 || key > 122) && key != 32 && key != 45 && key != 46) {
                                return true;
                            }
                            break;
                        case "forAlphabetThaiAndSpaceAndSlash":
                            //ป้อนได้เฉพาะตัว ก-ฉ , ตัวเลข 0 - 9 , ช่องว่าง 1 ช่องว่าง ,และ / ไม่จำกัดจำนวน
                            value = event.target.value;
                            countSpace = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                            }
                            newcountSpace = countSpace;
                            if (newcountSpace > 0 && key == 32)
                                return true;
                            if ((key < 3585 || key > 3642) && (key < 3648 || key > 3662) && key != 32 && key != 47)
                                return true;
                            break;
                        case "forAlphabetThaiAndNumberAndSpaceAndSlash":
                            //ป้อนได้เฉพาะตัว ก-ฉ , ตัวเลข 0 - 9 , ช่องว่าง 1 ช่องว่าง ,และ / ไม่จำกัดจำนวน
                            value = event.target.value;
                            countSpace = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                            }
                            newcountSpace = countSpace;
                            if (newcountSpace > 0 && key == 32)
                                return true;
                            if ((key < 3585 || key > 3642) && (key < 3648 || key > 3662) && (key < 48 || key > 57) && key != 32 && key != 47)
                                return true;
                            break;
                        case "forAlphabetEnglishAndPointAndSpaceAndSlash":
                            //ป้อนได้เฉพาะตัวอักษร A-Z , a-z,จุด,ช่องว่าง 1 ช่องว่าง ,และ / ไม่จำกัดจำนวน
                            value = event.target.value;
                            countDot = 0;
                            countSpace = 0;
                            countMiddleLine = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                                if (value.substring(r, r + 1) == ".")
                                    countDot++;
                                if (value.substring(r, r + 1) == "-")
                                    countMiddleLine++;
                            }
                            newcountDot = countDot;
                            newcountSpace = countSpace;
                            newcountMiddleLine = countMiddleLine;
                            if (newcountDot > 0 && key == 46)
                                return true;
                            if (newcountSpace > 0 && key == 32)
                                return true;
                            if (newcountMiddleLine > 0 && key == 45)
                                return true;
                            if ((key < 65 || key > 122) && key != 32 && key != 46 && key != 47) {
                                return true;
                            }
                            break;
                        case "forAlphabetThaiAndEnglishAndNumberAndSpace":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ, ตัวเลข 0 - 9 และ space 
                            return ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662) && (key < 48 || key > 57) && (key != 32) && key == 46);
                            break;
                        case "forAlphabetThaiAndEnglishAndNumberAndOneSpaceAndDot":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ, ตัวเลข 0 - 9 และ space 1ครั้ง  และ dot
                            value = event.target.value;
                            countSpace = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                            }

                            if (countSpace > 0 && key == 32)
                            { return true; } else {
                                return ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662) && (key < 48 || key > 57) && (key != 32) && key != 46);
                            }
                            break;
                        case "forAlphabetThaiAndEnglishAndOneSpaceAndDot":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ,  และ space 1ครั้ง  และ dot
                            value = event.target.value;
                            countSpace = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                            }

                            if (countSpace > 0 && key == 32)
                            { return true; } else {
                                return ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662) && (key != 32) && key != 46);
                            }
                            break;
                        case "forAlphabetThaiAndEnglishAndOneSpaceAndOneDot":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ,  และ space 1ครั้ง  และ dot

                            value = event.target.value;
                            countSpace = 0;
                            countDot = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == " ")
                                    countSpace++;
                                if (value.substring(r, r + 1) == ".")
                                    countDot++;
                            }
                            newcountDot = countDot;
                            if (newcountDot > 0 && event.keyCode == 46) {
                                event.preventDefault();
                            }
                            if (countSpace > 0 && key == 32)
                            { return true; } else {
                                return ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662) && (key != 32) && key != 46);
                            }
                            break;
                        case "forAlphabetEnglishAndOnlyOnePointAndNumber":
                            //ป้อนได้เฉพาะตัวอักษร A-Z , a-z,0-9,จุด1 จุด
                            value = event.target.value;
                            countDot = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == ".")
                                    countDot++;
                            }
                            newcountDot = countDot;
                            if (newcountDot > 0 && key == 46)
                                return true;
                            if ((key < 65 || key > 122) && (key < 48 || key > 57) && key != 46) {
                                return true;
                            }
                            break;

                        case "forAlphabetThaiAndEnglishExceptSomespecialChar":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ + อักขระพิเศษทั้งหมดยกเว้น " ' | &              
                            if (key == 34)
                                return true;
                            if (key == 38)
                                return true;
                            if (key == 39)
                                return true;
                            if (key == 124)
                                return true;
                            if ((key < 65 || key > 90) && (key < 97 || key > 122) && (key < 3585 || key > 3642) && (key < 3648 || key > 3662) && (!(key > 32 || key < 47)) && (!(key > 91 || key < 96)) && (!(key > 58 || key < 64)) && (!(key > 123 || key < 127))) {
                                return true;
                            }
                            break;
                        case "forAlphabetEnglishExceptSomespecialChar":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,อักขระพิเศษทั้งหมดยกเว้น " ' | &             
                            if (key == 32 || key == 34 || key == 38 || key == 39 || key == 124) {
                                return true;
                            }
                            if ((key < 65 || key > 122) && (key < 48 || key > 57) && (key != 42) && (key != 44) && (key != 45) && (key != 46) && (key != 47) && (key != 64) && (key != 33) && (key != 35) && (key != 36) && (key != 37) && (key != 38) && (key != 40) && (key != 41) && (key != 43) && (key != 58) && (key != 59) && (key != 60) && (key != 61) && (key != 62) && (key != 63) && (key != 123) && (key != 125)) {
                                return true;
                            }
                            if ((key < 3585 || key > 3642) && (key < 3648 || key > 3662)) {
                                return false;
                            }
                            break;
                        case "forAlphabetEnglishExceptSomespecialCharForPassword":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,อักขระพิเศษทั้งหมดยกเว้น " ' |              
                            if (key == 34)
                                return true;
                            if (key == 39)
                                return true;
                            if (key == 124)
                                return true;
                            if ((key > 32 && key < 126)) {
                                return false;
                            } else {
                                return true;
                            }
                            break;
                        case "forAlphabetThaiAndEnglishBankAdnPoint":
                            var _value = event.srcElement.value;
                            countDot = 0;
                            countSpace = 0;
                            var l = _value.substring(3);


                            for (r = 0; r < _value.length; r++) {
                                if (_value.substring(r, r + 1) == " ")
                                    countSpace++;
                                if (_value.substring(r, r + 1) == ".")
                                    countDot++;
                            }
                            newcountDot = countDot;
                            newcountSpace = countSpace;
                            if (newcountDot > 0 && event.keyCode == 46) {
                                event.preventDefault();
                            }
                            if (newcountSpace > 1 && event.keyCode == 32) {
                                event.preventDefault();
                            }
                            if ((event.keyCode < 65 || event.keyCode > 90) && (event.keyCode < 97 || event.keyCode > 122) && (event.keyCode < 3585 || event.keyCode > 3642) && (event.keyCode < 3648 || event.keyCode > 3662)) {
                                event.preventDefault();
                            }

                            break;
                        case "forAlphabetEnglishAllspecialChar":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z + อักขระพิเศษทั้งหมด
                            if (key == 34)
                                return true;
                            if (key == 39)
                                return true;
                            if (key == 124)
                                return true;
                            if (key < 32 || key > 127) {
                                return true;
                            }
                            break;
                        case "forAlphabetEnglishSpecialCharOnlyOneAtsign":
                            //ป้อนได้เฉพาะตัวอักษร  A-Z ,a-z ,ก-ฮ + สระ +    
                            value = event.target.value;
                            countAtsign = 0;
                            for (r = 0; r < value.length; r++) {
                                if (value.substring(r, r + 1) == "@")
                                    countAtsign++;
                            }
                            newcountAtsign = countAtsign;
                            if (newcountAtsign > 0 && key == 64)
                                return true;

                            if ((key < 65 || key > 90) && (key < 97 || key > 122) && (!(key > 32 || key < 47)) && (!(key > 91 || key < 96)) && (!(key > 58 || key < 64)) && (!(key > 123 || key < 127))) {
                                return true;
                            }
                            break;

                    }
                    return false;
                }

            });
        }
    });
})(jQuery);
