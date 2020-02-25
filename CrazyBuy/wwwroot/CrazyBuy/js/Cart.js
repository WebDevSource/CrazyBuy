var prdCountNotEnough = true;
var cartApp = angular.module('CartApp', []).controller('CartCtrl', function ($scope) {
    Cart.checkPrdCount();

    $scope.delCartItem = function (id) {
        Cart.delCartItem(id);
    };
});

var Cart = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "cart", Cart.InitModule);
    },

    InitModule() {
        NavBar.Init();
    },

    checkPrdCount() {
        Utils.ProcessAjax("/api/prd/isProductEnough", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    prdCountNotEnough = !ret.data;
                    Cart.InitView();
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    },

    delCartItem(id) {
        Utils.ProcessAjax("/api/ShopCart/" + id, "DELETE", true, "",
            function (ret) {
                if (ret.code === 1) {
                    Cart.checkPrdCount();
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    },

    InitView() {
        Utils.ProcessAjax("/api/ShopCart", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CartCtrl]');
                    let $scope = angular.element(appElement).scope();

                    $scope.TotalAmt = 0;
                    $scope.CalculateSum = function (cart) {
                        $scope.TotalAmt += cart.amount;
                    }
                    $scope.canOrder = !prdCountNotEnough;
                    $scope.prdCountCheck = prdCountNotEnough;
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
