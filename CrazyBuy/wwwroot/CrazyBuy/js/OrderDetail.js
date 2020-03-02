﻿var CheckOutApp = angular.module('OrderApp', []).controller('OrderCtrl', function ($scope) {
    $scope.id = Utils.GetUrlParameter('id');
    $scope.submit = function () {
        $scope.contact.orderId = parseInt($scope.id, 0);
        OrderDetail.addContactItem($scope.contact);
    };
    OrderDetail.getDetailData($scope.id);
});


var OrderDetail = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "orderDetail", OrderDetail.InitModule);
    },


    InitModule() {
        NavBar.Init();
        OrderDetail.InitView();
    },

    InitView() {

    },

    addContactItem(args) {
        Utils.ProcessAjax("/api/OrderContactItem", "PUT", true, args,
            function (ret) {
                if (ret.code === 1) {
                    OrderDetail.getDetailData(Utils.GetUrlParameter('id'));
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    },

    getDetailData(id) {
        Utils.ProcessAjax("/api/order/" + id, "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=OrderCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.TotalAmt = 0;
                    $scope.ShipAmt = 0;
                    $scope.master = ret.data.master;
                    $scope.detail = ret.data.detail;
                    $scope.now = new Date();
                    $scope.contact = {};
                    $scope.contact.ContactContent = "匯款日：\n末五碼：\n備註：";
                    $scope.contactList = ret.data.contactList;
                    $scope.resultAmt = 0;
                    if ('面交取貨' !== $scope.master.shippingMethod) {
                        $scope.ShipAmt = 180;
                    }

                    $scope.CalculateSum = function (item) {
                        $scope.TotalAmt += item.amount;
                        $scope.resultAmt = $scope.TotalAmt + $scope.ShipAmt;
                    }
                    $scope.$apply();
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    },
};

window.onload = OrderDetail.doLoad;