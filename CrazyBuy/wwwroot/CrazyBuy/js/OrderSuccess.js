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
	
    }
};

window.onload = OrderSuccess.doLoad;