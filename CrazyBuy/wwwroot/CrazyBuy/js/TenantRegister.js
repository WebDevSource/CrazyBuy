var TenantRegister = {
    places: new Array,

    DoLoad: function () {
        //Utils.Initial();
        Utils.InitI18next("zh-TW", "tenantRegister", TenantRegister.InitModule);
    },

    InitModule: function () {
        TenantRegister.GetPlaces();
        TenantRegister.InitMsgTxt();
        TenantRegister.InitialValidator();

    },

    InitMsgTxt: function () {
        //將訊息Code轉換成文字
        $('#checkout-userinfo :input').each(function () {
            if ($(this).attr("data-error") != "undefined") {
                $(this).attr("data-error", i18next.t($(this).attr("data-error")));
            }
            if ($(this).attr("data-pattern-error") != "undefined") {
                $(this).attr("data-pattern-error", i18next.t($(this).attr("data-pattern-error")));
            }
            if ($(this).attr("data-match-error") != "undefined") {
                $(this).attr("data-match-error", i18next.t($(this).attr("data-match-error")));
            }
            if ($(this).attr("placeholder") != "undefined") {
                $(this).attr("placeholder", i18next.t($(this).attr("placeholder")));
            }
        });
    },

    InitialValidator: function () {
        //驗證
        $('#checkout-userinfo').validator().on('submit', function (e) {
            console.log("InitialValidator");
            if (e.isDefaultPrevented()) { // 未驗證通過 則不處理
                return;
            } else { // 通过后，送出表单
                //alert(TenantRegister.CheckValue());
                if (TenantRegister.CheckValue()) {
                    TenantRegister.Register(TenantRegister.GetInputValue());
                }
            }
            e.preventDefault(); // 防止原始 form 提交表单
        });
    },

    CheckTenantCode: function () {
        let tenantCode = $("#tenantCode").val().trim();
        if (tenantCode != "") {
            let ret = Utils.AsyncProcessAjax("/api/tenant/isExist/" + tenantCode, "GET", "", "");
            if (ret.data) {
                //alert(i18next.t("msg_tenantCode_existed"));
                return i18next.t("err_tenantCode_existed");
            }
            else return "";
        }
    },

    CheckValue: function () {
        //console.log($("#agree-register").prop("checked"));
        let errString = "";

        errString += TenantRegister.CheckTenantCode();

        if (errString == "") {
            //TenantRegister.GetInputValue();
            return true;
        }
        else {
            alert(errString);
            return false;
        }
    },

    GetInputValue: function () {
        let postData = {
            tenantCode: $("#tenantCode").val().trim(),
            tenantName: $("#tenantName").val().trim(),
            enterpriseName: $("#enterpriseName").val().trim(),
            enterpriseId: $("#enterpriseId").val().trim(),
            owner: $("#owner").val().trim(),
            cityId: $("#city").val(),
            townId: $("#town").val().split("_")[0],
            zipCode: $("#town").val().split("_")[1],
            address: $("#address").val().trim(),
            memberName: $("#memberName").val().trim(),
            cellphone: $("#cellphone").val().trim(),
            memberPwd: $("#memberPwd").val().trim(),
            email: $("#email").val().trim(),
            lineId: $("#lineId").val().trim(),
            FBCommunity: $("#FBCommunity").val().trim(),
            FBFan: $("#FBFan").val().trim(),
            tenantType: $("input[name=tenantType]:checked").val()
        };
        console.log(JSON.stringify(postData));
        return postData;
    },

    Register: function (postData) {
        console.log("Register=" + postData);
        Utils.ProcessAjax("/api/tenant/Register", "POST", "", postData,
            function (ret) {
                switch (ret.code) {
                    case 1:
                        alert(i18next.t("register_success"));
                        break;
                    case -1:
                        console.log(ret.data);
                        alert(i18next.t("msg_service_error"));
                        break;
                }
                //window.location = "index.html";
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    GetPlaces: function () {
        Utils.ProcessAjax("/api/Common/getPlaces", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    TenantRegister.places = ret.data;
                    for (let i of TenantRegister.places) {
                        $("#city").append($("<option></option>").attr("value", i.id).text(i.name));
                    }
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    UpdateTown: function (cityId) {
        $("#town option").remove();
        $("#town").append($("<option></option>").attr("value", "").text("鄉鎮市區"));
        if (cityId != "縣市") {
            let towns = TenantRegister.places.find(item => { return item.id == cityId });
            for (let i of towns.areas) {
                $("#town").append($("<option></option>").attr("value", i.townId + "_" + i.zipCode).text(i.townName));
            }
        }
    },



};