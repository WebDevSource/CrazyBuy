var CheckOutApp = angular.module('CheckOutApp', []).controller('CheckOutCtrl', function ($scope) {

});

var Checkout = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "Checkout", Checkout.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Questions.InitView();
    },

	InitView() {
	
    }
};

window.onload = Checkout.doLoad;