var CheckOutApp = angular.module('CheckOutApp', []).controller('CheckOutCtrl', function ($scope) {
    $scope.checkout = JSON.parse(Utils.GetCookie("order"));
    $scope.memberData = JSON.parse(Utils.GetCookie("memberData"));

    $scope.sendOrder = function () {
        CheckoutSuccess.sendOrder();
    };
});

var CheckoutSuccess = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "checkoutSuccess", CheckoutSuccess.InitModule);
    },


    InitModule() {
        NavBar.Init();
        CheckoutSuccess.InitView();
    },

    InitView() {
        Utils.ProcessAjax("/api/ShopCart", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CheckOutCtrl]');
                    let $scope = angular.element(appElement).scope();

                    $scope.TotalAmt = 0;
                    $scope.ResultAmt = 0;
                    $scope.CalculateSum = function (cart) {
                        $scope.TotalAmt += cart.amount;
                        $scope.ResultAmt = $scope.TotalAmt + parseInt(($scope.TotalAmt * 0.05).toFixed(0), 0);
                        $scope.ResultAmt += checkout.shippingMethod === '快遞送貨' ? 180 : 0;
                        console.log($scope.ResultAmt);
                    };
                    $scope.carts = ret.data;
                    $.each($scope.carts, function (index, value) {
                        if (value.prdImages) {
                            let images = value.prdImages ? JSON.parse(value.prdImages) : "";
                            imageUrl = Utils.BackendImageUrl + "id=" + value.productId + "&filename=" + images[0].filename;
                            $scope.carts[index].prdImages = imageUrl;
                        }

                    });
                    $scope.$apply();
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    },

    sendOrder() {
        var args = JSON.parse(Utils.GetCookie("order"));
        Utils.ProcessAjax("/api/order", "PUT", true, args,
            function (ret) {                
                if (ret.code === 1) {
                    switch (ret.data.code) {
                        case -1:
                            alert("there not item in cart.");
                            break;
                        case -2:
                            alert("product not enough.");
                            break;
                        default:
                            window.location.href = 'order-success.html?tenantCode=' + Utils.TenantCode + '&id=' + ret.data.code + '&serialNo=' + ret.data.data.serialNo;
                            break;
                    }
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    }
};

window.onload = CheckoutSuccess.doLoad;