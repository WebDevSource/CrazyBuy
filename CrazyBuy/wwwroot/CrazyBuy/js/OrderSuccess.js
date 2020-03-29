var CheckOutApp = angular.module('OrderApp', []).controller('OrderCtrl', function ($scope) {
    $scope.id = Utils.GetUrlParameter('id');
    $scope.serialNo = Utils.GetUrlParameter('serialNo');
});


var OrderSuccess = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "orderSuccess", Questions.InitModule);
    },


    InitModule() {
        NavBar.Init();
        OrderSuccess.InitView();
    },

    InitView() {

    },

    checkDetail() {
        window.location.href = 'order-detail.html?&tenantCode=' + Utils.TenantCode + '&id=' + Utils.GetUrlParameter('id');
    }
};

window.onload = OrderSuccess.doLoad;