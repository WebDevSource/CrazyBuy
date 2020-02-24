var cartApp = angular.module('CartApp', []).controller('CartCtrl', function ($scope) {
    Cart.InitView();
});

var Cart = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "cart", Cart.InitModule);
    },

    InitModule() {
        NavBar.Init();
    },

    InitView() {
        Utils.ProcessAjax("/api/ShopCart", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CartCtrl]');
                    let $scope = angular.element(appElement).scope();

                    $scope.carts = ret.data;
                    $scope.$apply();
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    }
};

window.onload = Cart.doLoad;
