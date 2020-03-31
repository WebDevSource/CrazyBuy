var CheckOutApp = angular.module('OrderApp', []).controller('OrderCtrl', function ($scope) {
    $scope.id = Utils.GetUrlParameter('id');
    $scope.submit = function () {
        $scope.contact.orderId = parseInt($scope.id, 0);
        OrderDetail.addContactItem($scope.contact);
    };   
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
        OrderDetail.getDetailData(Utils.GetUrlParameter('id'));
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
            function (error) { alert(i18next.t("msg_service_error"));}
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
                    $scope.contact.ContactContent = "匯款日：\n末五碼：\n備註：";
                    $scope.contactList = ret.data.contactList;
                    $scope.resultAmt = 0;
                    $scope.ShipAmt = $scope.master.shippingAmount;

                    $scope.resultAmt = $scope.master.totalAmount;
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error"));}
        );
    },
};

window.onload = OrderDetail.doLoad;