Utils = {
    DataTableLanguage: null,
    SystemLoginUser: null,
    LocalServiceUrl: "",
    PublicServiceUrl: "",
    // navbarBotton: { "btn_home": "./index.html", "btn_product": "./products.html", "btn_statement": "./announcement.html","btn_FQA": "questions.html"},
    ROLE_ADMIN: 'admin',
    ROLE_MEMBER: 'member',
    ROLE_GUEST: 'guest',
    TenantCode: '',

    // BackendUrl: "http://crazybuyadmin-dev.orangeinfo.tw/api/S_TenantPrd/DownloadImgFile?id=7&filename=",
    BackendUrl: "http://crazybuyadmin-dev.orangeinfo.tw/",
    //BackendImageUrl: "http://crazybuyadmin-dev.orangeinfo.tw/" + "api/S_TenantPrd/DownloadImgFile?type=prd",
    //BackendBulletinImageUrl: "http://crazybuyadmin-dev.orangeinfo.tw/" + "api/S_TenantBulletin/DownloadImgFile?type=bulletin",


    BackendImageUrl: "/api/Common/DownloadImgFile?type=prd",
    BackendBulletinImageUrl: "/api/Common/DownloadImgFile?type=bulletin",

    //BackendImageUrl: "/api/Common/DownloadImgFile?",
    Initial: function async(callback) {
        Utils.InitView();
        Utils.TenantCode = Utils.GetUrlParameter("tenantCode");
        Utils.CheckToken();
        Utils.InitEvent();
        //        Backend.SetupMessageConnect(callback);
    },

    InitView() {
        $('[data-userauthority]').hide();
        $('[data-authority]').hide();

    },

    InitEvent() {
        $("body").on("click", ".btn-admin-edit", function () {
            Utils.openBankend();
        });
        $("body").on("click", 'a', function () {

            let me = $(this);
            let url = me.attr("href");
            if (me.attr("data-toggle")) {

            }
            else if (url && url.indexOf('javascript:') < 0) {
                url += (url.indexOf("?") > 0 ? "&" : "?") + 'tenantCode=' + Utils.TenantCode;
                $(this).attr("href", url);
            }
        });
    },


    InitNavBar() {
        let html = "";
        for (let key in Utils.navbarBotton) {
            html += '<li class="nav-item ">'
                + '<a class="nav-link " href = "' + Utils.navbarBotton[key] + '" > ' + i18next.t(key) + '</a ></li > ';
        }
        $(".navbar-nav").html(html);
        let role = Utils.getRole();
        $('[data-userauthority="' + role + '"]').show()
        $('[data-authority="' + role + '"]').show();
    },

    checkRole() {
        let role = Utils.getRole();
        if (role == Utils.ROLE_GUEST) {
            alert(i18next.t("msg_error_loginFrist"));
            location.href = "./index.html?tenantCode=" + Utils.TenantCode;
        }
    },

    openBankend() {
        //window.open(Utils.BackendUrl + Utils.TenantCode);

        window.open(Utils.BackendUrl + "manager/login");

    },

    getRole() {
        let role = Utils.GetCookie("role");
        return role ? role : Utils.ROLE_GUEST;
    },

    InitialForBackendEdit: function async(callback) {
        Utils.CheckToken(callback);
        Backend.SetupMessageConnect(callback);
    },

    HeaderInitial: function () {
        $("#header-navbar").load("/Backend/header.html", function () {
            //alert(Utils.SystemLoginUser.userName);
            $("#imgPhoto").attr("src", Utils.SystemLoginUser.userPortraitUrl);
            $("#spanUserName").html(Utils.SystemLoginUser.userName);
        });
    },

    SystemFunctionInitial: function () {
        setTimeout(function () {
            var temp = "";
            var menuBuffer = "<div class=\"pcoded-inner-navbar main-menu\">";
            var moduleTemplate = "<div class=\"pcoded-navigatio-lavel\">[moduleName]</div>";
            var menuTemplate = "<li class=\"\">" + //"<li class=\"pcoded-hasmenu\">" +
                "<a href=\"[functionUrl]\">" +
                "<span class=\"pcoded-micon\"><i class=\"[functionIcon]\"></i></span>" +
                "<span class=\"pcoded-mtext\">[functionName]</span>" +
                "</a>" +
                "</li>";

            for (var i = 0; i <= Utils.SystemLoginUser.module.length - 1; i++) {
                temp = moduleTemplate;
                menuBuffer += temp.replace("[moduleName]", Utils.SystemLoginUser.module[i].moduleName);
                menuBuffer += "<ul class=\"pcoded-item pcoded-left-item\">";

                for (var j = 0; j <= Utils.SystemLoginUser.module[i].function.length - 1; j++) {
                    temp = menuTemplate;
                    //alert(temp);
                    menuBuffer += temp.replace("[functionUrl]", Utils.SystemLoginUser.module[i].function[j].functionUrl)
                        .replace("[functionIcon]", Utils.SystemLoginUser.module[i].function[j].functionIcon)
                        .replace("[functionName]", Utils.SystemLoginUser.module[i].function[j].functionName);
                }

                menuBuffer += "</ul>";
            }

            menuBuffer += "</div>";

            $(".pcoded-navbar").html(menuBuffer);

            //Utils.LoadExtScript("/assets/js/pcoded.min.js");
            Utils.LoadExtScript("/Backend/assets/js/vartical-layout.min.js");
            //Utils.LoadExtScript("/assets/js/jquery.mCustomScrollbar.concat.min.js");
            Utils.LoadExtScript("/Backend/assets/js/script.js");

            $(".navbar-wrapper").show();
            $(".pcoded-navbar").show();

        }, 100);
    },

    Logout: function () {
        if (confirm("系統即將登出，請問您是否確認離開?") == true) {
            location.href = "/Backend/index.html";
        }
    },

    UserProfile: function () {
        alert("UserProfile Click.");
    },

    ClearToken: function () {
        Utils.SetCookie("token", "", 0);
        Utils.SetCookie("role", Utils.ROLE_GUEST);
    },

    CheckToken: function async(callback) {
        var token = "";
        token = Utils.GetCookie("token");
        if (!Utils.TenantCode) {
            //alert(i18next.t("msg_tenant_not_find"));
            location.href = "./error.html";
            return;
        } else {
            let ret = Utils.AsyncProcessAjax("/api/tenant/isExist/" + Utils.TenantCode, "GET", "", "");
            if (ret.code != 1) {
                alert(i18next.t("msg_tenant_not_find"));
                location.href = "./error.html";
                return;
            }
        }

        if (token) {
            Utils.ProcessAjax("/api/values/authorize", "GET", token, "",
                function (result) {
                    //alert(JSON.stringify(result));

                    //alert(`SystemLoginUser.id = ${result.data.userId}`);
                    //Utils.SystemLoginUser = result.data;

                    //Utils.HeaderInitial();
                    //Utils.SystemFunctionInitial();
                    //Utils.GetModuleNavigation();
                    //                    if (callback) callback();
                    for (let i = 0; i < result.length; i++) {
                        let item = result[i];
                        if (item.type == "userType") {
                            Utils.SetCookie("role", item.value);
                        }
                    }

                },
                function (result) {
                    alert(i18next.t("msg_service_error"));

                    Utils.ClearToken();

                    window.location.reload();
                });
        } else {
            Utils.ClearToken();
            NavBar.login("test", "1234");
        }

    },


    VerifyUser: function (userId, userPassword, rememberCheck) {
        var result = "";
        var buffer = {
            "userId": userId,
            "userPassword": userPassword
        };

        //if rememberCheck write cookie for keeping userId

        Utils.ProcessAjax("/Backend/SystemUtils/VerifyUser", "POST", "", JSON.stringify(buffer), Utils.VerifyUserProcessDone, Utils.VerifyUserProcessDone);
    },

    VerifyUserProcessDone: function (result) {
        //alert(JSON.stringify(result));

        if (result.code == 0) {
            //綁定成功
            alert(result.data.token);

            //Saving Token to Cookie
            Utils.SetCookie("token", result.data.token, 1);
            Utils.SetCookie("tenantId", result.data.tenantId, 1);
            Utils.SetCookie("uId", result.data.uId, 1);

            location.href = "main.html";
        }
        else {
            //綁定失敗
            alert(result.data);
        }
    },

    FormatDate: function (value, format) {
        if (value.constructor.toString() != Date.toString()) {
            return null;
        }
        if (format == null) {
            format = "yyyy-MM-dd HH:mm:ss";
        }
        var y = value.getFullYear();
        var m = value.getMonth() + 1;
        var d = value.getDate();
        var h = value.getHours();
        var n = value.getMinutes();
        var s = value.getSeconds();
        var w = value.getDay();
        return format.replace("yyyy", y).replace("yy", y % 100 < 10 ? "0" + y % 100 : y % 100)
            .replace("MM", m < 10 ? "0" + m : m).replace("M", m)
            .replace("dd", d < 10 ? "0" + d : d).replace("d", d)
            .replace("HH", h < 10 ? "0" + h : h).replace("H", h)
            .replace("mm", n < 10 ? "0" + n : n).replace("m", n)
            .replace("ss", s < 10 ? "0" + s : s).replace("s", s);
    },

    GetUrlParameter: function (name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    },

    Date2timestamp: function (date) {
        /* date to timestamp */
        var myTimeStamp = new Date(date).getTime();

        return myTimeStamp;
    },

    Timestamp2date: function (timestamp) {
        /* timestamp to date */
        var myDate = new Date(timestamp);

        return myDate;
    },

    ProcessAjax: function (url, method, authToken, data, processDone, processFailed) {
        try {
            if (typeof data == "object" && method != "GET") {
                data = JSON.stringify(data);
            }
            $.ajax({
                url: url,
                type: method,
                data: data,
                headers: {
                    "Content-Type": "application/json",
                },
                beforeSend: function (xhr) {
                    if (authToken) {
                        let token = Utils.GetCookie("token");
                        xhr.setRequestHeader("Authorization", 'Bearer ' + token);
                    }
                },
                success: function (data) {
                    //alert(JSON.stringify(result));
                    processDone(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(JSON.stringify(errorThrown));
                    processFailed(errorThrown);
                }
            });
        }
        catch (e) { alert(e); }
    },

    AsyncProcessAjax: function (url, method, authToken, data) {
        let result;
        try {
            if (typeof data == "object") {
                data = JSON.stringify(data);
            }
            $.ajax({
                url: url,
                type: method,
                data: data,
                async: false,
                headers: {
                    "Content-Type": "application/json"
                },
                beforeSend: function (xhr) {
                    if (authToken != "") {
                        let token = Utils.GetCookie("token");
                        xhr.setRequestHeader("Authorization", 'Bearer ' + token);
                    }
                },
                success(ret) {
                    result = ret;
                },
                error(ret) {
                    console.log(ret);
                }
            });
            return result;
        }
        catch (e) {
            console.log(e);
        }

    },

    SetCookie: function (cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = Utils.TenantCode + cname + "=" + encodeURI(cvalue) + ";" + expires + ";path=/";
    },

    GetCookie: function (cname) {
        var name = Utils.TenantCode + cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return decodeURI(c.substring(name.length, c.length));
            }
        }
        return "";
    },

    LoadExtStyls: function (filename) {
        var fileref = document.createElement("link");
        fileref.setAttribute("rel", "stylesheet");
        fileref.setAttribute("type", "text/css");
        fileref.setAttribute("href", filename);

        document.getElementsByTagName("head")[0].appendChild(fileref);
    },

    LoadExtScript: function (filename) {
        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", filename);

        document.getElementsByTagName("head")[0].appendChild(fileref);
    },

    InitI18next: function (sLanguage, sNamespaces, callback) {
        let files = Array.isArray(sNamespaces) ? sNamespaces : [sNamespaces];
        files.push("translation");
        i18next
            .use(i18nextXHRBackend)
            .init({
                debug: true,
                lng: sLanguage,
                ns: files,
                defaultNS: files,
                backend: {
                    // for all available options read the backend's repository readme file
                    loadPath: './locales/{{lng}}/{{ns}}.json'
                }
            },
                function (t) {
                    jqueryI18next.init(i18next, $, {
                        tName: 't', // --> appends $.t = i18next.t
                        i18nName: 'i18n', // --> appends $.i18n = i18next
                        handleName: 'localize', // --> appends $(selector).localize(opts);
                        selectorAttr: 'data-i18n', // selector for translating elements
                        targetAttr: 'i18n-target', // data-() attribute to grab target element to translate (if different than itself)
                        optionsAttr: 'i18n-options', // data-() attribute that contains options, will load/set if useOptionsAttr = true
                        useOptionsAttr: false, // see optionsAttr
                        parseDefaultValueFromContent: true // parses default values from content ele.val or ele.text
                    });

                    $("body").localize();

                    if (callback != null) {
                        callback();
                    }

                });

    },

    InitServiceUrl: function (callback) {
        Utils.ProcessAjax("/Backend/SystemUtils/GetServiceUrl", "POST", "", null,
            function (result) {
                if (result.code == 0) {
                    Utils.LocalServiceUrl = result.data.localServiceUrl;
                    Utils.PublicServiceUrl = result.data.publicServiceUrl;
                } else {
                    Utils.LocalServiceUrl = location.protocol + "//" + location.host;
                    Utils.PublicServiceUrl = location.protocol + "//" + location.host;
                }

                if (callback != null) { callback(); }
            }, function (result) {
                Utils.LocalServiceUrl = location.protocol + "//" + location.host;
                Utils.PublicServiceUrl = location.protocol + "//" + location.host;
                if (callback != null) { callback(); }
            });
    },

    InitDataTable: function () {
        Utils.DataTableLanguage = {  //自訂語言提示 
            "lengthMenu": i18next.t("show") + " _MENU_ " + i18next.t("entries"),
            "info": i18next.t("show") + " _START_ " + i18next.t("to") + " _END_ " + i18next.t("of") + " _TOTAL_ " + i18next.t("entries"),
            "infoEmpty": i18next.t("showing") + " 0 " + i18next.t("to") + " 0 " + i18next.t("of") + " 0 " + i18next.t("entries"),
            "sInfoFiltered": "(" + i18next.t("filtered from") + " _MAX_ " + i18next.t("total entries") + ")",
            "search": i18next.t("search") + ":",
            "emptyTable": i18next.t("emptyTable"),
            "zeroRecords": i18next.t("zeroRecords"),
            "paginate": {
                "previous": i18next.t("previous"),
                "next": i18next.t("next")
            }
        };
    },

    GetModuleNavigation: function (functionId) {
        var buffer = {
            "moduleIcon": "",
            "moduleId": "",
            "moduleName": "",
            "functionIcon": "",
            "functionId": "",
            "functionName": ""
        };

        //filter Utils.SystemLoginUser
        let pathname = location.pathname;
        let arrModule = Utils.SystemLoginUser.module;
        let arrFunction = null;
        for (let i = 0; i <= arrModule.length - 1; i++) {
            arrFunction = arrModule[i].function;
            for (let j = 0; j <= arrFunction.length - 1; j++) {
                if (functionId && arrFunction[j].functionId === functionId) {
                    buffer.moduleIcon = arrModule[i].moduleIcon;
                    buffer.moduleId = arrModule[i].moduleId;
                    buffer.moduleName = arrModule[i].moduleName;
                    buffer.functionIcon = arrFunction[j].functionIcon;
                    buffer.functionId = arrFunction[j].functionId;
                    buffer.functionName = arrFunction[j].functionName;

                } else if (arrFunction[j].functionUrl.trim() == pathname.trim()) {
                    buffer.moduleIcon = arrModule[i].moduleIcon;
                    buffer.moduleId = arrModule[i].moduleId;
                    buffer.moduleName = arrModule[i].moduleName;
                    buffer.functionIcon = arrFunction[j].functionIcon;
                    buffer.functionId = arrFunction[j].functionId;
                    buffer.functionName = arrFunction[j].functionName;
                }
            }
        }
        //console.log(JSON.stringify(buffer));
        $('#NavigationMain').html(buffer.moduleName);
        $('#NavigationSub').html(buffer.functionName);
        return buffer;
    },

    GetNewID: function () {
        var d = Date.now();
        if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
            d += performance.now(); //use high-precision timer if available
        }
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
    },

    GetDropdownList: function (mainId, callback) {
        var token = Utils.GetCookie("token");
        var buffer = {
            "mainId": mainId
        };

        //if rememberCheck write cookie for keeping userId

        Utils.ProcessAjax("/Backend/SystemUtils/GetDropdownList", "POST", token, JSON.stringify(buffer), callback, callback);
    },

    ParseDateTimeToString(datetime) {

        String.prototype.padLeft = function (value, size) {
            var x = this;
            while (x.length < size) { x = value + x; }
            return x;
        };

        var hh = String(datetime.getHours()).padLeft('0', 2);
        var mm = String(datetime.getMinutes()).padLeft('0', 2);
        var ss = String(datetime.getSeconds()).padLeft('0', 2);
        var str = $.datepicker.formatDate('yy/mm/dd ', datetime) + " " + hh + ":" + mm + ":" + ss

        return str;

    },

    ParseDateTimeWithoutSecToString(datetime) {

        String.prototype.padLeft = function (value, size) {
            var x = this;
            while (x.length < size) { x = value + x; }
            return x;
        };

        var hh = String(datetime.getHours()).padLeft('0', 2);
        var mm = String(datetime.getMinutes()).padLeft('0', 2);
        var ss = String(datetime.getSeconds()).padLeft('0', 2);
        var str = $.datepicker.formatDate('yy/mm/dd ', datetime) + " " + hh + ":" + mm

        return str;

    },



    DownloadExcel(base64, fileName) {
        // base64ToArrayBuffer
        var binaryString = window.atob(base64);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }

        // SaveByteArray
        Utils.SaveByteArray(fileName, bytes);
    },

    SaveByteArray(excelName, byte) {
        var blob = new Blob([byte], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        var fileName = excelName + ".xlsx";
        link.download = fileName;
        link.click();
    },

    /// ********  Pagination **********///

    SetPagination: function (totalCount, currentPage, pageSize, preFunName, nextFuncName, pageFuncName) {

        var paginationContent = '<ul class="pagination"></ul>';

        var paginationObj = $(paginationContent);

        var lastPage = Math.ceil(parseFloat(totalCount) / parseFloat(pageSize));

        var showPages = Utils.GetNumberPages(lastPage, currentPage, 10);

        if (currentPage > 1) {
            var preContent = '<li class="page-item"><a class="page-link" href="javascript:' + preFunName + '">Pre</a></li>';
            paginationObj.append(preContent);
        }
        showPages.forEach(function (e) {
            var activeClass = currentPage === e ? "active" : "";
            var page =
                '<li class="page-item ' + activeClass + '" > <a class="page-link" onclick="' + pageFuncName + '" href="#">' + e + '</a></li>';
            paginationObj.append(page);
        });

        if (currentPage < lastPage) {
            var nextContent = '<li class="page-item"><a class="page-link" href="javascript:' + nextFuncName + '">Next</a></li>';
            paginationObj.append(nextContent);
        }

        var result = paginationObj.prop('outerHTML');
        return result;

    },

    GetImageUrl: function (data) {
        let imageUrl = "./images/noitem.jpg";
        if (Utils.getRole() == Utils.ROLE_GUEST) {

        } else if (data.prdImages) {
            let images = data.prdImages ? JSON.parse(data.prdImages) : "";
            imageUrl = Utils.BackendImageUrl + "&id=" + data.id + "&filename=" + images[0].filename;
        }
        return imageUrl;
    },

    GetBulletinImageUrl: function (data) {
        let images = data.uplaodImg ? JSON.parse(data.uplaodImg) : "";
        let imageUrl = "";
        if (images) {
            imageUrl = Utils.BackendBulletinImageUrl + "&id=" + data.id + "&filename=" + images[0].filename;

        }
        return imageUrl;
    },

    GetNumberPages: function (lastPage, CurrentPage, showPageSize) {
        var showPages = [];
        var startIndex = 0;
        var lastIndex = 0;

        var middnumber = Math.ceil(parseFloat(showPageSize) / 2);

        startIndex = CurrentPage - middnumber;
        if (startIndex < 1) {
            startIndex = 1;
        }

        lastIndex = (startIndex + showPageSize) - 1;
        if (lastIndex > lastPage) {
            lastIndex = lastPage;
        }
        for (var i = startIndex; i <= lastIndex; i++) {
            showPages.push(i);
        }
        return showPages;
    },

    GetTenantId: function () {
        var userData = JSON.parse(Utils.GetCookie('user'))
        return userData.tenantId;
    },

    parseTextToHtml: function (text) {
        text = text.replace('\r\n', '</br>')
            .replace(/\r\n|\n/g, '</br>');
        return text;
    }
};