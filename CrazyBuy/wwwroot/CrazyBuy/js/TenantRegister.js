var TenantRegister = {
    places: new Array,
    serviceItemNumber: 0,

    DoLoad: function () {
        //Utils.Initial();
        Utils.InitI18next("zh-TW", "tenantRegister", TenantRegister.InitModule);
        //TenantRegister.AddServiceItem(TenantRegister.serviceItemNumber);
    },

    InitModule: function () {
        TenantRegister.GetPlaces();
        TenantRegister.InitMsgTxt();
        TenantRegister.InitialValidator();
        TenantRegister.AddServiceItem(TenantRegister.serviceItemNumber);

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
            //console.log(e.isDefaultPrevented());
            if (e.isDefaultPrevented()) { // 未驗證通過 則不處理
                return;
            } else { // 通过后，送出表单
                //alert(TenantRegister.CheckValue());
                //alert(e.isDefaultPrevented());
                //console.log(TenantRegister.GetInputValue());
                if (TenantRegister.CheckValue()) {
                    TenantRegister.Register(TenantRegister.GetInputValue());
                }

            }
            e.preventDefault(); // 防止原始 form 提交表单
        });
    },

    //CheckTenantCode: function () {
    //    let tenantCode = $("#tenantCode").val().trim();
    //    if (tenantCode != "") {
    //        let ret = Utils.AsyncProcessAjax("/api/tenant/isExist/" + tenantCode, "GET", "", "");
    //        if (ret.data) {
    //            //alert(i18next.t("msg_tenantCode_existed"));
    //            return i18next.t("err_tenantCode_existed");
    //        }
    //        else return "";
    //    }
    //},

    CheckValue: function () {
        let errString = "";
        let tenantCode = [];
        let cellphone = [];
        let email = [];

        //errString += TenantRegister.CheckTenantCode();

        for (i = 1; i <= TenantRegister.serviceItemNumber; i++) {
            if ($("#Item" + i).length > 0) {
                let findcellphone = cellphone.find(function (item) { return item == $("#cellphone" + i).val().trim() });
                let findemail = email.find(function (item) { return item == $("#email" + i).val().trim() });

                //商店網址重複判斷
                if ($("#tenantType" + i).val() !== "LINE公告系統") {
                    let findTenantCode = tenantCode.find(function (item) { return item == $("#tenantCode" + i).val().trim() });

                    if (findTenantCode == undefined) {
                        tenantCode.push($("#tenantCode" + i).val().trim());
                    }
                    else {
                        errString += findTenantCode + " " + i18next.t("label_tenantCode") + i18next.t("err_checkRepeat") + "\n";
                    }
                }

                //手機號碼重複判斷
                if (findcellphone == undefined) {
                    cellphone.push($("#cellphone" + i).val().trim());
                }
                else {
                    errString += findcellphone + " " + i18next.t("label_cellphone") + i18next.t("err_checkRepeat") + "\n";
                }

                //Email重複判斷
                if (findemail == undefined) {
                    email.push($("#email" + i).val().trim());
                }
                else {
                    errString += findemail + " Email" + i18next.t("err_checkRepeat") + "\n";
                }
            }
        }

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
            memberName: $("#memberName").val().trim(),
            lineId: $("#lineId").val().trim(),
            enterpriseName: $("#enterpriseName").val().trim(),
            enterpriseId: $("#enterpriseId").val().trim(),
            owner: $("#owner").val().trim(),
            cityId: $("#city").val(),
            townId: $("#town").val().split("_")[0],
            zipCode: $("#town").val().split("_")[1],
            address: $("#address").val().trim(),
            ServiceItem:[]
        };

        for (i = 1; i <= TenantRegister.serviceItemNumber; i++) {
            console.log($("#Item" + i).length);
            if ($("#Item" + i).length > 0) {
                let item = {
                    tenantType: $("#tenantType" + i).val(),
                    cellphone: $("#cellphone" + i).val().trim(),
                    email: $("#email" + i).val().trim(),
                    memberPwd: $("#memberPwd" + i).val().trim(),
                    tenantCode: "",
                    tenantName: "",
                    FBCommunity: "",
                    FBFan: "",
                    LineOfficialAccount: ""
                };

                if ($("#tenantType" + i).val() === "LINE公告系統") {
                    item.LineOfficialAccount = $("#LineOfficialAccount" + i).val().trim();
                }
                else {
                    item.tenantCode = $("#tenantCode" + i).val().trim();
                    item.tenantName = $("#tenantName" + i).val().trim();
                    item.FBCommunity = $("#FBCommunity" + i).val().trim();
                    item.FBFan = $("#FBFan" + i).val().trim();
                }

                postData.ServiceItem.push(item);
            }
        }

        console.log(JSON.stringify(postData));
        return postData;
    },

    Register: function (postData) {
        //console.log("Register=" + postData);
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
                    case -999:
                        console.log(ret.data);
                        alert(ret.data);
                        break;
                }
                if (ret.code == 1) window.location = "index.html";
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
                    TenantRegister.UpdateTown($("#city").val());
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    UpdateTown: function (cityId) {
        $("#town option").remove();
        //$("#town").append($("<option></option>").attr("value", "").text("鄉鎮市區"));
        if (cityId != "縣市") {
            let towns = TenantRegister.places.find(item => { return item.id == cityId });
            for (let i of towns.areas) {
                $("#town").append($("<option></option>").attr("value", i.townId + "_" + i.zipCode).text(i.townName));
            }
        }
    },

    AddServiceItem: function (currentNumber) {
        TenantRegister.serviceItemNumber += 1;

        let itemNumber = TenantRegister.serviceItemNumber;
        let strHtml = `<section id="Item${itemNumber}" class="my-md-30 my-30 border">
                <hr class="m-0 border-danger" style="border-width: 2px;" />
                <div class="my-20 px-md-15 d-flex flex-nowrap justify-content-between">
                    <h5 class=" order-title font-weight-bold">${i18next.t("label_service_title")}</h5>
                    <div class="justify-content-end">
                        <button type="button" class="btn btn-outline-danger btn-sm" id="addServiceItem${itemNumber}" onclick="TenantRegister.AddServiceItem(${itemNumber});">${i18next.t("btn_addService")}</button>
                        <button type="button" class="btn btn-outline-secondary rounded-circle btn-sm" style="display:none;" id="removeServiceItem${itemNumber}" onclick="TenantRegister.RemoveServiceItem(${itemNumber});">X</button>
                    </div>
                </div>
                <div class="row d-flex flex-wrap px-md-30">
                    <div class="form-group col-md-6 col-12 mb-3 order-0">
                        <p class="mb-2"><label class="required" for="">${i18next.t("label_tenantType")}</label></p>
                        <select class="checkout-select w-100" id="tenantType${itemNumber}" name="tenantType" onchange="TenantRegister.SetServiceContent(${itemNumber})">
                            <option value="團媽" selected>團媽系統</option>
                            <option value="轉批媽">轉批媽系統</option>
                            <option value="批發商">批發商系統</option>
                            <option value="LINE公告系統">LINE 公告系統</option>
                        </select>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div  id="ItemContent${itemNumber}" class="d-flex flex-wrap"></div>
                </div>
            </section>`;

        $("#serviceItem").append(strHtml);
        TenantRegister.SetServiceContent(itemNumber);

        //按鈕設定
        $("#addServiceItem" + currentNumber).hide();
        $("#removeServiceItem" + currentNumber).show();
    },

    RemoveServiceItem: function (currentNumber) {
        $("#Item" + currentNumber).remove();

        //更新驗證項目
        $('#checkout-userinfo').validator("update");
    },

    SetServiceContent: function (currentNumber) {
        //console.log("SetServiceContent : " + currentNumber);
        try {
            let tenantType = $("#tenantType" + currentNumber).val();

            switch (tenantType) {
                case "團媽":
                case "轉批媽":
                case "批發商":
                    TenantRegister.ShowCrazyBuy(currentNumber);
                    break;
                case "LINE公告系統":
                    TenantRegister.ShowLineNotify(currentNumber);
                    break;
            }
        }
        catch(e){
            alert("Error: {" + e + "}");
        }
    },

    ShowCrazyBuy: function (currentNumber) {
        console.log("ShowCrazyBuy : " + currentNumber);
        let itemNumber = currentNumber;
        let strHtml = `<div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2">
                                <label class="required" for="">${i18next.t("label_tenantCode")}</label>&nbsp;&nbsp;
                                <span style="font-size:14px; color:red;">${i18next.t("label_tenantCode_note")}</span>
                            </p>
                            <div class="d-flex">
                                <label class="border pt-1" style="background-color:#E0E0E0;">https://crazybuy.winpower365.com/</label>
                                <input class="w-100" type="text" id="tenantCode${itemNumber}" name="tenantCode" pattern="^[A-Za-z0-9]+$" required="required" maxlength="30">
                            </div>
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label class="required" for="">${i18next.t("label_tenantName")}</label></p>
                            <input class="w-100" type="text" id="tenantName${itemNumber}" name="tenantName" required="required">
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label class="required" for=""">${i18next.t("label_cellphone")}</label></p>
                            <input class="w-100" type="text" id="cellphone${itemNumber}" name="cellphone" pattern="^[09]{2}[0-9]{8}$" required="required">
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2">
                                <label class="required" for="">Email</label>&nbsp;&nbsp;
                                <span style="font-size:14px; color:red;">${i18next.t("label_email_note")}</span>
                            </p>
                            <input class="w-100" type="email" id="email${itemNumber}" name="email" required="required">
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label class="required" for="">${i18next.t("label_memberPwd")}</label></p>
                            <input class="w-100" type="password" id="memberPwd${itemNumber}" name="memberPwd" required="required">
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label class="required" for="">${i18next.t("label_checkPwd")}</label></p>
                            <input class="w-100" type="password" id="checkPwd${itemNumber}" name="checkPwd" data-match="#memberPwd${itemNumber}" data-match-error="${i18next.t("err_checkPassword")}" required="required">
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label for="">${i18next.t("label_FBCommunity")}</label></p>
                            <input class="w-100" type="text" id="FBCommunity${itemNumber}" name="FBCommunity">
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label for="">${i18next.t("label_FBFan")}</label></p>
                            <input class="w-100" type="text" id="FBFan${itemNumber}" name="FBFan">
                        </div>`;

        $("#ItemContent" + itemNumber).empty();
        $("#ItemContent" + itemNumber).append(strHtml);

        //更新驗證項目
        $('#checkout-userinfo').validator("update");
    },

    ShowLineNotify: function (currentNumber) {
        //console.log("ShowLineNotify : " + currentNumber);
        let itemNumber = currentNumber;
        let strHtml = `<div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label class="required" for="">${i18next.t("label_cellphone")}</label></p>
                            <input class="w-100" type="text" id="cellphone${itemNumber}" name="cellphone" pattern="^[09]{2}[0-9]{8}$" required="required">
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2">
                                <label class="required" for="">Email</label>&nbsp;&nbsp;
                                <span style="font-size:14px; color:red;">${i18next.t("label_email_note")}</span>
                            </p>
                            <input class="w-100" type="email" id="email${itemNumber}" name="email" required="required">
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label class="required" for="">${i18next.t("label_memberPwd")}</label></p>
                            <input class="w-100" type="password" id="memberPwd${itemNumber}" name="memberPwd" required="required">
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label class="required" for="">${i18next.t("label_checkPwd")}</label></p>
                            <input class="w-100" type="password" id="checkPwd${itemNumber}" name="checkPwd" data-match="#memberPwd${itemNumber}" data-match-error="${i18next.t("err_checkPassword")}" required="required">
                            <div class="help-block with-errors"></div>
                        </div>
                        <div class="form-group col-md-6 col-12 mb-3 order-0">
                            <p class="mb-2"><label for="">${i18next.t("label_Line")}</label></p>
                            <input class="w-100" type="text" id="LineOfficialAccount${itemNumber}" name="LineOfficialAccount" placeholder="@XXXXX">
                        </div>`;

        $("#ItemContent" + itemNumber).empty();
        $("#ItemContent" + itemNumber).append(strHtml);

        //更新驗證項目
        $('#checkout-userinfo').validator("update");
    },

};