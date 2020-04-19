var MemberApp = angular.module('MemberApp', []).controller('MemberCtrl', function ($scope) {
    $scope.agree = true;
    $scope.checkPwd = '';
    $scope.member = {};
    $scope.update = function (selectedValue) {
        console.log(selectedValue);
        $scope.level2 = $scope.towns[selectedValue]
        console.log($scope.level2);
    };

    $scope.submit = function () {
        if ($scope.agree) {

            if ($scope.member.password !== null) {
                if ($scope.checkPwd !== $scope.member.password) {
                    alert(i18next.t("mem_please_check_password"));
                    return;
                }
            }

            if ($scope.SelArea && $scope.SelArea.includes(':')) {
                $scope.member.townId = parseInt($scope.SelArea.split(':')[0]);
                $scope.member.zipCode = parseInt($scope.SelArea.split(':')[1]);
            }

            Utils.ProcessAjax("/api/member/" + $scope.member.memberId, "POST", true, $scope.member,
                function (ret) {
                    switch (ret.code) {
                        case 1:
                            alert(i18next.t("swal_updateSuccess"));
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
            alert('not agree.');
        }
    };
});

var MemberInfo = {
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
        Utils.InitI18next("zh-TW", "memberInfo", MemberInfo.InitModule);
    },


    InitModule() {
        Utils.checkRole();
        NavBar.Init();
        MemberInfo.InitView();
    },

    InitView() {
        MemberInfo.getTenantSetting();
        MemberInfo.GetMemberData();
    },

    GetMemberData() {
        Utils.ProcessAjax("/api/member", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=MemberCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.member = ret.data;
                    $scope.$apply();
                    MemberInfo.getPlaces();
                } else {

                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) {
                alert(i18next.t("msg_service_error"));
            }
        );
    },

    getPlaces() {
        Utils.ProcessAjax("/api/Common/getPlaces", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=MemberCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.Citys = ret.data;
                    $scope.towns = [];
                    for (let i in ret.data) {
                        let item = ret.data[i];
                        $scope.towns[item.id] = item.areas;
                    }
                    MemberInfo.setCity();
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    setCity() {
        let appElement = document.querySelector('[ng-controller=MemberCtrl]');
        let $scope = angular.element(appElement).scope();
        $scope.level2 = $scope.towns[$scope.member.cityId];

        let zipAddress = $scope.member.townId + ':' + $scope.member.zipCode;
        $scope.SelArea = zipAddress;
        $scope.$apply();
    },
    changeType(element) {
        let input = $(element).prev();
        let type = input.attr("type");
        input.attr("type", (type == "password" ? "text" : "password"));
    },
    getTenantSetting() {
        Utils.ProcessAjax("/api/tenant/getTenantSetting", "GET", true, "",
            function (ret) {
                if (ret.code === -1) {
                    alert("service error");
                } else {
                    let appElement = document.querySelector('[ng-controller=MemberCtrl]');
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
                        let key = MemberInfo.columnMapping[item.name];
                        $scope.setting[key] = item;
                    }
                    $scope.$apply();
                }
            },
            function (error) { alert("ajax error") }
        );
    }
};

window.onload = MemberInfo.doLoad;