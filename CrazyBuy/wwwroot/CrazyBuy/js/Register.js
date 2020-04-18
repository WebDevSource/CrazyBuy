var RegApp = angular.module('RegApp', []).controller('RegCtrl', ['$scope', '$sce', function ($scope, $sce) {
    $scope.agree = false;
    $scope.checkPwd = '';
    $scope.member = {};
    $scope.update = function (selectedValue) {
        $scope.level2 = $scope.towns[selectedValue]
    };
    $scope.to_trusted = function (html_code) {
        return $sce.trustAsHtml(html_code);
    }
    $scope.submit = function () {
        if ($scope.agree) {
            if ($scope.checkPwd === $scope.member.password) {
                $scope.member.memberCode = Utils.FormatDate(new Date(), "yyyyMMddHHmmssSSS");
                $scope.member.tenantType = '批發商';
                $scope.member.status = '正常';
                $scope.member.creator = 1;
                if ($scope.SelArea && $scope.SelArea.includes(':')) {
                    $scope.member.townId = parseInt($scope.SelArea.split(':')[0]);
                    $scope.member.zipCode = parseInt($scope.SelArea.split(':')[1]);
                }
                Utils.ProcessAjax("/api/member/" + Utils.GetTenantId(), "PUT", true, $scope.member,
                    function (ret) {
                        switch (ret.code) {
                            case 1:
                                alert(i18next.t("register_success"));
                                window.location = "index.html?tenantCode=" + Utils.TenantCode;
                                break;
                            case -1:
                                console.log(ret.data);
                                alert(i18next.t("msg_service_error"));
                                break;
                        }
                    },
                    function (error) { alert(i18next.t("msg_service_error")); }
                );
            } else {
                alert('please check password.');
            }
        } else {
            alert('not agree.');
        }
    };
}]);


var Register = {

    columnMapping: {
        "姓名": "name",
        "性別": "gender",
        "電話": "phone",
        "Line ID": "lineId",
        "密碼": "password",
        "手機號碼": "mobile",
        "地址": "addr",
        "Email": "Email",
        "傳真": "fax",

    },

    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "register", Register.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Register.InitView();

    },

    InitView() {
        Register.getTenantSetting();
        Register.getPlaces();
        Register.getBulletin();
    },

    getTenantSetting() {
        Utils.ProcessAjax("/api/tenant/getTenantSetting", "GET", true, "",
            function (ret) {
                if (ret.code === -1) {
                    alert("service error");
                } else {
                    let appElement = document.querySelector('[ng-controller=RegCtrl]');
                    let $scope = angular.element(appElement).scope();
                    let setting = [];
                    $scope.setting = {};
                    for (let i = 0; i < ret.data.length; i++) {

                        let item = ret.data[i];
                        if (item.title == "MemberColumnSetting") {
                            if (item.content) {
                                setting = JSON.parse(item.content);
                                break;
                            }
                        }
                    }
                    for (let i in setting) {
                        let item = setting[i];
                        let key = Register.columnMapping[item.name];
                        $scope.setting[key] = item;
                    }
                    $scope.$apply();
                }
            },
            function (error) { alert("ajax error") }
        );
    },
    getPlaces() {
        Utils.ProcessAjax("/api/Common/getPlaces", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=RegCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.Citys = ret.data;
                    $scope.towns = [];
                    for (let i in ret.data) {
                        let item = ret.data[i];
                        $scope.towns[item.id] = item.areas;
                    }
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },
    changeType(element) {
        let input = $(element).prev();
        let type = input.attr("type");
        input.attr("type", (type == "password" ? "text" : "password"));
    },

    getBulletin() {
        Utils.ProcessAjax("/api/tenant/getBulletin", "GET", true, "",
            function (ret) {
                if (ret.code === -1) {
                    alert("service error");
                } else {
                    let appElement = document.querySelector('[ng-controller=RegCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.bulletins = [];
                    for (let i = 0; i < ret.data.length; i++) {
                        let item = ret.data[i];
                        if (i18next.t("register_bulletin") == item.layout) {
                            $scope.bulletins.push(item.content);
                        }
                    }
                    $scope.$apply();
                }
            },
            function (error) { alert("ajax error") }
        );
    },
};

window.onload = Register.doLoad;