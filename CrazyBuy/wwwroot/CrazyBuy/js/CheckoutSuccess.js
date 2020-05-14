var CheckOutApp = angular.module('CheckOutApp', []).controller('CheckOutCtrl', function ($scope) {

    $scope.invoicePrice = 0;

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
        Utils.checkRole();
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
                        if ($scope.checkout.isNeedInvoice || $scope.checkout.invoiceType == i18next.t("invoice_type_donate")) {
                            $scope.invoicePrice = parseInt(($scope.TotalAmt * 0.05).toFixed(0), 0);
                            $scope.ResultAmt = $scope.TotalAmt + $scope.invoicePrice;
                        } else {
                            $scope.ResultAmt = $scope.TotalAmt;
                        }
                        $scope.ResultAmt += parseInt($scope.checkout.shippingAmount);
                        console.log($scope.ResultAmt);
                    };
                    $scope.carts = ret.data;
                    $.each($scope.carts, function (index, value) {
                        if (value.prdImages) {
                            let images = value.prdImages ? JSON.parse(value.prdImages) : "";
                            imageUrl = Utils.BackendImageUrl + "&id=" + value.productId + "&filename=" + images[0].filename;
                            $scope.carts[index].prdImages = imageUrl;
                        }
                        let specialRule = JSON.parse(value.specialRule);
                        $scope.carts[index].specialRule = specialRule
                        $scope.carts[index].sepc = value.prdSepc ? JSON.parse(value.prdSepc) : null;
                        $scope.carts[index].type = i18next.t($scope.carts[index].type);

                        let hasOnly = false;
                        let tags = [];
                        for (let tag in specialRule) {
                            let name = specialRule[tag];
                            if (name.indexOf(i18next.t("tag_factory")) > -1) {
                                if (hasOnly) {
                                    continue;
                                } else {
                                    hasOnly = true;
                                    name = i18next.t("tag_only");
                                }
                            } else if (name.indexOf(i18next.t("tag_only")) > -1) {
                                if (hasOnly) {
                                    continue;
                                } else {

                                    hasOnly = true;
                                }
                            }
                            tags.push(name);
                        }
                        $scope.carts[index].specialRule = tags;

                    });
                    let order = Utils.GetCookie("order");
                    if (!order) {
                        alert("請由購物車結帳");
                        location.href = "./cart.html?tenantCode=" + Utils.TenantCode;
                    }
                    $scope.checkout = JSON.parse(order);
                    $scope.memberData = JSON.parse(Utils.GetCookie("memberData"));
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error"));}
        );
    },

    sendOrder() {
        var args = JSON.parse(Utils.GetCookie("order"));
        Utils.ProcessAjax("/api/order", "PUT", true, args,
            function (ret) {                
                if (ret.code === 1) {
                    switch (ret.data.code) {
                        case -1:
                            alert(i18next.t("msg_cart_is_null"));
                            break;
                        case -2:
                            alert(i18next.t("msg_product_not_enough"));
                            break;
                        default:
                            Utils.SetCookie("order", "");
                            window.location.href = 'order-success.html?tenantCode=' + Utils.TenantCode + '&id=' + ret.data.code + '&serialNo=' + ret.data.data.serialNo;
                            break;
                    }
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error"));}
        );
    }
};

window.onload = CheckoutSuccess.doLoad;