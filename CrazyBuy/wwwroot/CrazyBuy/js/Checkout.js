var CheckOutApp = angular.module('CheckOutApp', []).controller('CheckOutCtrl', function ($scope) {
    $scope.checkout = {};
    $scope.chose = {};
    $scope.update = function (selectedValue) {
        $scope.level2 = $scope.towns[selectedValue];
    };

    $scope.memupdate = function (selectedValue) {
        $scope.memlevel2 = $scope.towns[selectedValue];
    };

    $scope.recUpdate = function (selectedValue) {
        $scope.reclevel2 = $scope.towns[selectedValue];
    };

    $scope.submit = function () {
        //$scope.checkout.serialNo = Utils.FormatDate(new Date(), null);

        if ($scope.checkout.invoiceType == i18next.t("invoice_type_three") || $scope.checkout.invoiceType == i18next.t("invoice_type_two")) {
            $scope.checkout.isNeedInvoice = true;
        } else {
            $scope.checkout.isNeedInvoice = false;
        }

        if ($scope.SelArea.includes(':')) {
            $scope.checkout.recipientTownId = $scope.SelArea.split(':')[0];
            $scope.checkout.recipientZipCode = $scope.SelArea.split(':')[1];
            $scope.checkout.recipientTownName = $scope.townMap[$scope.checkout.recipientTownId].townName;
            $scope.checkout.recipientCityName = $scope.cityMap[$scope.checkout.recipientCityId].name;

        }

        if ($scope.member.cityId) {
            $scope.checkout.memberTownName = $scope.townMap[$scope.member.townId].townName;
            $scope.checkout.memberCityName = $scope.cityMap[$scope.member.cityId].name;
        }

        if (typeof $scope.recSelArea === 'string' || $scope.recSelArea instanceof String) {
            if ($scope.recSelArea.includes(':')) {
                $scope.checkout.invoiceTownId = $scope.recSelArea.split(':')[0];
                $scope.checkout.invoiceZipCode = $scope.recSelArea.split(':')[1];
                $scope.checkout.invoiceTownName = $scope.townMap[$scope.checkout.invoiceTownId].townName;
                $scope.checkout.invoiceCityName = $scope.cityMap[$scope.checkout.invoiceCityId].name;
            }
        }
        Checkout.sendOrder($scope.checkout);
    };

    $scope.replaceData = function () {
        $scope.checkout.recipientName = $scope.member.name;
        $scope.checkout.recipientGender = $scope.member.gender;
        $scope.checkout.recipientTel = $scope.member.tel;
        $scope.checkout.recipientMobile = $scope.member.cellphone;
        $scope.checkout.recipientEmail = $scope.member.email;
        $scope.checkout.recipientAddr = $scope.member.address;
        $scope.checkout.recipientCityId = $scope.member.cityId
        $scope.level2 = $scope.memlevel2;
        $scope.SelArea = $scope.memSelArea;
    };
});

var Checkout = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "checkout", Checkout.InitModule);
    },

    InitModule() {
        Utils.checkRole();
        NavBar.Init();
        Checkout.getPlaces();
        Checkout.getFreight();
        Checkout.getPaymentType();
        Checkout.getBankInfo();
    },

    InitView(data) {
        let appElement = document.querySelector('[ng-controller=CheckOutCtrl]');
        let $scope = angular.element(appElement).scope();

        $scope.member = data;
        $scope.memlevel2 = $scope.towns[$scope.member.cityId];

        let zipAddress = $scope.member.townId + ':' + $scope.member.zipCode;
        $scope.memSelArea = zipAddress;
        $scope.$apply();
    },

    getMemberData() {
        Utils.ProcessAjax("/api/member", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    Utils.SetCookie("memberData", JSON.stringify(ret.data), 1);
                    Checkout.InitView(ret.data);
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    getPlaces() {
        Utils.ProcessAjax("/api/Common/getPlaces", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CheckOutCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.Citys = ret.data;
                    $scope.towns = [];
                    $scope.cityMap = [];
                    $scope.townMap = [];

                    for (let i in ret.data) {
                        let item = ret.data[i];
                        $scope.towns[item.id] = item.areas;
                        $scope.cityMap[item.id] = item;
                        for (let j in item.areas) {
                            let town = item.areas[j];
                            $scope.townMap[town.townId] = town;
                        }
                    }

                    $scope.$apply();
                    Checkout.getMemberData();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    getFreight() {
        Utils.ProcessAjax("/api/Common/getFreight", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CheckOutCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.Freight = ret.data;
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    getPaymentType() {
        Utils.ProcessAjax("/api/Common/getPaymentType", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CheckOutCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.PaymentType = ret.data;
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    getBankInfo() {
        Utils.ProcessAjax("/api/Common/getBankInfo", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CheckOutCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.bank = ret.data.bank;
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    sendOrder(order) {
        let ship = order.shippingMethod.split(":");
        order.shippingMethod = ship[0];
        order.shippingAmount = parseInt(ship[1]);
        Utils.SetCookie("order", JSON.stringify(order), 1);
        window.location.href = 'checkout-success.html?&tenantCode=' + Utils.TenantCode;
    },


};

window.onload = Checkout.doLoad;