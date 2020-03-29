var CheckOutApp = angular.module('CheckOutApp', []).controller('CheckOutCtrl', function ($scope) {
    $scope.checkout = {};
    $scope.chose = {};
    $scope.update = function (selectedValue) {
        $scope.level2 = selectedValue.areas;
    };

    $scope.recUpdate = function (selectedValue) {
        $scope.reclevel2 = selectedValue.areas;
    };

    $scope.submit = function () {
        //$scope.checkout.serialNo = Utils.FormatDate(new Date(), null);

        if ($scope.checkout.isNeedInvoice === "empty") {
            $scope.checkout.isNeedInvoice = true;
        } else {
            $scope.checkout.isNeedInvoice = false;
        }

        if ($scope.SelArea.includes(':')) {
            $scope.checkout.recipientTownId = $scope.SelArea.split(':')[0];
            $scope.checkout.recipientZipCode = $scope.SelArea.split(':')[1];
        }

        if (typeof $scope.recSelArea === 'string' || $scope.recSelArea instanceof String) {
            if ($scope.recSelArea.includes(':')) {
                $scope.checkout.invoiceTownId = $scope.recSelArea.split(':')[0];
                $scope.checkout.invoiceZipCode = $scope.recSelArea.split(':')[1];
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
        $scope.$apply();
    };
});

var Checkout = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "Checkout", Checkout.InitModule);
    },

    InitModule() {
        NavBar.Init();
        Checkout.getMemberData();
        Checkout.getPlaces();
    },

    InitView(data) {
        let appElement = document.querySelector('[ng-controller=CheckOutCtrl]');
        let $scope = angular.element(appElement).scope();
        $scope.member = data;
        $scope.$apply();
    },

    getMemberData() {
        Utils.ProcessAjax("/api/member", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    Utils.SetCookie("memberData", JSON.stringify(ret.data), 1);
                    Checkout.InitView(ret.data);
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    },

    getPlaces() {
        Utils.ProcessAjax("/api/Common/getPlaces", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=CheckOutCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.Citys = ret.data;
                    $scope.$apply();
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    },

    sendOrder(order) {
        Utils.SetCookie("order", JSON.stringify(order), 1);
        window.location.href = 'checkout-success.html?&tenantCode=' + Utils.TenantCode;
    }
};

window.onload = Checkout.doLoad;