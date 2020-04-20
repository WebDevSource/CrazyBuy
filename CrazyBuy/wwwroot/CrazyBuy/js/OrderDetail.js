var CheckOutApp = angular.module('OrderApp', []).controller('OrderCtrl', function ($scope) {
    $scope.id = Utils.GetUrlParameter('id');
    $scope.submit = function () {
        $scope.contact.orderId = parseInt($scope.id, 0);
        OrderDetail.addContactItem($scope.contact);
    };
    $scope.useSample = function () {
        $scope.contact.ContactContent = "匯款日：\n末五碼：\n備註：";
    };
});


var OrderDetail = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "orderDetail", OrderDetail.InitModule);
    },


    InitModule() {
        Utils.checkRole();
        NavBar.Init();
        OrderDetail.InitView();

    },

    InitView() {
        OrderDetail.getDetailData(Utils.GetUrlParameter('id'));
        OrderDetail.getBankInfo();
    },

    addContactItem(args) {
        Utils.ProcessAjax("/api/OrderContactItem", "PUT", true, args,
            function (ret) {
                if (ret.code === 1) {
                    OrderDetail.getDetailData(Utils.GetUrlParameter('id'));
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    checkLevelMember(id) {
        Utils.ProcessAjax("/api/Common/getMembmerLevelById/" + id, "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=OrderCtrl]');
                    let $scope = angular.element(appElement).scope();
                    let disName = ret.data.levelName;
                    let dis = ret.data.discount;
                    let value = i18next.t("memberLevelValue");
                    value = value.replace('{0}', disName);
                    value = value.replace('{1}', dis);
                    $scope.memberLevelValue = value;
                    $scope.$apply();
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
                    $scope.invoicePrice = 0;
                    $scope.master = ret.data.master;
                    $scope.detail = ret.data.detail;
                    $.each($scope.detail, function (index, value) {
                        console.log(value);
                        if (value.prdImages) {
                            let images = value.prdImages ? JSON.parse(value.prdImages) : "";
                            imageUrl = Utils.BackendImageUrl + "id=" + value.prdId + "&filename=" + images[0].filename;
                            $scope.detail[index].prdImages = imageUrl;
                        }
                    });
                    $scope.now = new Date();
                    $scope.contact = {};
                    $scope.contactList = ret.data.contactList;
                    $scope.resultAmt = 0;
                    $scope.ShipAmt = $scope.master.shippingAmount;
                    if ($scope.master.isNeedInvoice) {
                        $scope.invoicePrice = parseInt((($scope.master.orderAmount + $scope.ShipAmt) * 0.05).toFixed(0), 0);
                    }
                    if (ret.data.master.levelId != null) {
                        OrderDetail.checkLevelMember(ret.data.master.levelId);
                    }
                    $scope.resultAmt = $scope.master.totalAmount;
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
                    let appElement = document.querySelector('[ng-controller=OrderCtrl]');
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
};

window.onload = OrderDetail.doLoad;