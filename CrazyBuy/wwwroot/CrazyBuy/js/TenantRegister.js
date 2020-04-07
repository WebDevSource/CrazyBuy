var TenantRegister = {
    DoLoad: function () {
        $("#btnSubmit").click(TenantRegister.Register);
    },

    Register: function () {
        var tenantCode = $("#tenantCode").val();
        var tenantName = $("#tenantName").val();
        var enterpriseName = $("#enterpriseName").val();
        var enterpriseId = $("#enterpriseId").val();
        var owner = $("#owner").val();

        var postData = {
            tenantCode: tenantCode,
            tenantName: tenantName, 
            enterpriseName: enterpriseName,
            enterpriseId: enterpriseId,
            owner: owner
        };

        Utils.ProcessAjax("/api/tenant/Register", "POST", "", postData,
            function (ret) {
                switch (ret.code) {
                    case 0:
                        alert(i18next.t("register_success"));
                        break;
                    case -1:
                        console.log(ret.data);
                        alert(i18next.t("msg_service_error"));
                        break;
                }
                window.location = "index.html";
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );




    }

};