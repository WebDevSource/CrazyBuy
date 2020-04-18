var CheckOutApp = angular.module('OrderApp', []).controller('OrderCtrl', function ($scope) {
    $scope.id = Utils.GetUrlParameter('id');
    $scope.serialNo = Utils.GetUrlParameter('serialNo');
});


var OrderSuccess = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "orderSuccess", OrderSuccess.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Utils.checkRole();
        OrderSuccess.InitView();
    },

    InitView() {
        Utils.ProcessAjax("/api/order/" + Utils.GetUrlParameter('id'), "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=OrderCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.master = ret.data.master;
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    checkDetail() {
        window.location.href = 'order-detail.html?&tenantCode=' + Utils.TenantCode + '&id=' + Utils.GetUrlParameter('id');
    }
};

window.onload = OrderSuccess.doLoad;