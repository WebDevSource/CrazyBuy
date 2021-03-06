﻿var prdCountNotEnough = false;
var cartApp = angular.module('CartApp', []).controller('CartCtrl', function ($scope) {

    $scope.delCartItem = function (id) {
        Cart.delCartItem(id);
    };
});

var Cart = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", ["cart", "products"], Cart.InitModule);

    },

    InitModule() {
        Utils.checkRole();
        NavBar.Init();
        // Cart.checkTimeOut();
        Cart.checkPrdCount();
        Cart.checkLevelMember();
    },

    checkPrdCount() {
        Utils.ProcessAjax("/api/prd/isProductEnough", "GET", true, "",
            function (ret) {
                if (ret.code === -1) {
                    if (Array.isArray(ret.data)) {
                        let appElement = document.querySelector('[ng-controller=CartCtrl]');
                        let $scope = angular.element(appElement).scope();
                        $scope.enoughList = ret.data;
                        Cart.InitView();
                    } else {

                        alert(i18next.t("msg_service_error"));
                    }
                } else {
                    Cart.InitView();
                }
            },
            function (error) { alert(i18next.t("ajax error")) }
        );
    },

    checkTimeOut() {
        Utils.ProcessAjax("/api/Common/getTimeOutItems", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let data = ret.data;
                    if (data.length > 0) {
                        let msg = '以下商品已經超過上架時間，\n'
                        data.forEach(element => msg += element.name + '\n');
                        alert(msg);
                    }
                }
            },
            function (error) { alert(i18next.t("ajax error")) }
        );
    },

    checkLevelMember() {
        Utils.ProcessAjax("/api/Common/getMembmerLevel", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CartCtrl]');
                    let $scope = angular.element(appElement).scope();
                    let disName = ret.data.levelName;
                    let dis = ret.data.discount;
                    $scope.disWording = '(' + disName + ' ' + dis + i18next.t("dis") + ')';
                    Cart.InitView();
                }
            },
            function (error) { alert(i18next.t("ajax error")) }
        );

    },

    delCartItem(id) {
        Utils.ProcessAjax("/api/ShopCart/" + id, "DELETE", true, "",
            function (ret) {
                if (ret.code === 1) {
                    Cart.checkPrdCount();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")) }
        );
    },

    goBackIndex() {
        let index = 'index.html?tenantCode=' + Utils.GetUrlParameter('tenantCode');
        window.location.href = index;
    },

    InitView() {
        Utils.ProcessAjax("/api/ShopCart", "GET", true, "",
            function (ret) {
                prdCountNotEnough = false;
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
                            imageUrl = Utils.BackendImageUrl + "&id=" + value.productId + "&filename=" + images[0].filename;
                        }
                        let specialRule = JSON.parse(value.specialRule);
                        value.type = i18next.t(value.type);
                        $scope.carts[index].prdImages = imageUrl;
                        $scope.carts[index].type = value.type;
                        $scope.carts[index].specialRule = specialRule
                        $scope.carts[index].sepc = value.prdSepc ? JSON.parse(value.prdSepc) : null;
                        let hasOnly = false;
                        let tags = [];
                        let prdId = value.productId;
                        for (let tag in specialRule) {
                            let name = specialRule[tag];
                            if (name.indexOf(i18next.t("tag_factory")) > -1) {
                                if (hasOnly) {
                                    continue;
                                } else {
                                    hasOnly = true;
                                    name = i18next.t("tag_only");
                                    onlyOrder.set(prdId, value);
                                }
                            } else if (name.indexOf(i18next.t("tag_only")) > -1) {
                                if (hasOnly) {
                                    continue;
                                } else {

                                    hasOnly = true;
                                    onlyOrder.set(prdId, value);
                                }
                            }
                            tags.push(name);
                        }


                        $scope.carts[index].specialRule = tags;


                        if (Array.isArray($scope.enoughList) && $scope.enoughList.includes(prdId)) {
                            stockZero.set(prdId, value);
                        }
              /*          if (Array.isArray(specialRule) && (specialRule.includes(i18next.t("tag_only")) || specialRule.includes(i18next.t("tag_factory")))) {
                            onlyOrder.set(prdId, value);

                        }
                */        if (value.shipType) {
                            let shipType = JSON.parse(value.shipType);
                            if (shipType.includes(i18next.t("ship_type_nomal")) && shipType.includes(i18next.t("ship_type_cool"))) {
                                //
                            } else if (shipType.includes(i18next.t("ship_type_nomal")) || shipType.includes(i18next.t("ship_type_nomal1"))) {
                                shipNomal.set(prdId, value);

                            } else if (shipType.includes(i18next.t("ship_type_cool")) || shipType.includes(i18next.t("ship_type_cool1"))) {
                                shipCoole.set(prdId, value);
                            }
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
                    } else if ($scope.carts.length < 1) {
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
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    }
};

window.onload = Cart.doLoad;
