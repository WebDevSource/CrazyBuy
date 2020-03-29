var prdCountNotEnough = false;
var cartApp = angular.module('CartApp', []).controller('CartCtrl', function ($scope) {    

    $scope.delCartItem = function (id) {
        Cart.delCartItem(id);
    };
});

var Cart = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", ["cart","products"], Cart.InitModule);

    },

    InitModule() {
        NavBar.Init();
        Cart.checkPrdCount();
    },

    checkPrdCount() {
        Cart.InitView();
       /* 
        * Utils.ProcessAjax("/api/prd/isProductEnough", "GET", true, "",
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
        */
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
                let onlyOrder = new Map();
                let shipCoole = new Map();
                let shipNomal = new Map();
                let product = new Map();
                let stockZero = new Map();
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CartCtrl]');
                    let $scope = angular.element(appElement).scope();

                    $scope.TotalAmt = 0;
                    $scope.CalculateSum = function (cart) {
                        $scope.TotalAmt += cart.amount;
                    };

                    $scope.carts = ret.data;
                    $.each($scope.carts, function (index, value) {
                        let imageUrl = "./images/noitem.jpg";
                        if (value.prdImages) {
                            let images = value.prdImages ? JSON.parse(value.prdImages) : "";
                            imageUrl = Utils.BackendImageUrl + "id=" + value.productId + "&filename=" + images[0].filename;
                        }
                        let specialRule = JSON.parse(value.specialRule);
                        value.type = i18next.t("price_" + value.type);
                        $scope.carts[index].prdImages = imageUrl;
                        $scope.carts[index].type = value.type;
                        $scope.carts[index].specialRule = specialRule
                        $scope.carts[index].sepc = value.prdSepc ? JSON.parse(value.prdSepc) : null;
                        let prdId = value.productId;
                        if (value.stockNum < 1) {
                            stockZero.set(prdId, value);
                        } 
                        if (Array.isArray(specialRule) && (specialRule.includes(i18next.t("tag_only")) || specialRule.includes(i18next.t("tag_factory")))) {
                            onlyOrder.set(prdId, value);
  
                        }
                        let shipType = JSON.parse(value.shipType);
                        if (shipType.includes(i18next.t("ship_type_nomal")) && shipType.includes(i18next.t("ship_type_cool"))) {
                            //
                        } else if (shipType.includes(i18next.t("ship_type_nomal")) || shipType.includes(i18next.t("ship_type_nomal1")) ) {
                            shipNomal.set(prdId, value);

                        } else if (shipType.includes(i18next.t("ship_type_cool")) || shipType.includes(i18next.t("ship_type_cool1"))) {
                            shipCoole.set(prdId, value);
                        }
                        product.set(prdId, value);
                    });

                    if (onlyOrder.size > 1) {
                        prdCountNotEnough = true;
                    } else if (onlyOrder.size == 1 && product.size > 1) {
                        prdCountNotEnough = true;
                    } else if (shipNomal.size > 0 && shipCoole.size > 0) {
                        prdCountNotEnough = true;
                    } else if (stockZero.size > 0) {
                        prdCountNotEnough = true;
                    }

                    $scope.errorMessages = [];


                    if (stockZero.size > 0) {
                        $scope.errorMessages.push({ "name": i18next.t("cart_stock_zero"), "values": Array.from(stockZero.values()) });
                    }

                    if (shipNomal.size > 0 && shipCoole.size > 0) {
                        $scope.errorMessages.push({ "name": i18next.t("ship_type_nomal"), "values": Array.from(shipNomal.values()) });
                        $scope.errorMessages.push({ "name": i18next.t("ship_type_cool"), "values": Array.from(shipCoole.values()) });
                    }
                   
                    if (onlyOrder.size == 1 && product.size > 1) {
                        $scope.errorMessages.push({ "name": i18next.t("tag_only"), "values": Array.from(onlyOrder.values()) });
                    }

                   
                    $scope.limitMessage = i18next.t("cart_order_limit");
                    
                    $scope.canOrder = !prdCountNotEnough;
                    $scope.prdCountCheck = prdCountNotEnough;
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
