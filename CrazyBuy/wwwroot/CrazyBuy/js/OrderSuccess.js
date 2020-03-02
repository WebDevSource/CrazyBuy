var CheckOutApp = angular.module('OrderApp', []).controller('OrderCtrl', function ($scope) {
    $scope.id = Utils.GetUrlParameter('id');
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
        window.location.href = 'order-detail.html?&tenantId=' + Utils.GetUrlParameter("tenantId") + '&id=' + Utils.GetUrlParameter('id');
    }
};

window.onload = OrderSuccess.doLoad;