﻿var RegApp = angular.module('RegApp', []).controller('RegCtrl', ['$scope', '$sce', function ($scope, $sce) {
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
                                alert($scope.successMessage);
                                window.location = "index.html?tenantCode=" + Utils.TenantCode;
                                break;
                            case -1:
                                console.log(ret.data);
                                let data = ret.data;
                                if (data == "cellphone already exist.") {
                                    alert(i18next.t("member_exist_cellphone"));
                                } else if (data == "mail already exist.") {
                                    alert(i18next.t("member_exist_mail"));
                                } else {
                                    alert(i18next.t("msg_service_error"));
                                }
                                break;
                        }
                    },
                    function (error) { alert(i18next.t("msg_service_error")); }
                );
            } else {
                alert(i18next.t("mem_please_check_password"));
            }
        } else {
            alert(i18next.t("msg_error_member_not_agree"));
        }
    };
}]);


var Register = {

    columnMapping: {
        "姓名": "name",
        "性別": "gender",
        "電話": "phone",
        "LINE ID": "lineId",
        "密碼": "password",
        "手機號碼": "mobile",
        "地址": "addr",
        "Email": "Email",
        "傳真": "fax",

        "生日": "birthday",
        "FB帳號": "fbId",
        "備註": "Notes"

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
                    alert(i18next.t("msg_service_error"));
                } else {
                    let appElement = document.querySelector('[ng-controller=RegCtrl]');
                    let $scope = angular.element(appElement).scope();
                    let setting = [];
                    $scope.successMessage = i18next.t("register_success");
                    $scope.setting = {};
                    for (let i = 0; i < ret.data.length; i++) {
                        let item = ret.data[i];
                        if (!item.content) {
                            continue;
                        }
                        if (item.title == "MemberColumnSetting") {
                            setting = JSON.parse(item.content);

                        } else if (item.title == "MemJoinDescPage") {
                            $scope.successMessage = item.content;
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
            function (error) { alert(i18next.t("ajax error")) }
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
                    alert(i18next.t("msg_service_error"));
                } else {
                    let appElement = document.querySelector('[ng-controller=RegCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.bulletins = [];
                    for (let i = 0; i < ret.data.length; i++) {
                        let item = ret.data[i];
                        if (i18next.t("register_bulletin") == item.layout) {
                            $scope.bulletins.push(Utils.parseTextToHtml(item.content));
                        }
                    }
                    $scope.$apply();
                }
            },
            function (error) { alert(i18next.t("ajax error")) }
        );
    },
};
$(function () {
    $('input[name="daterange"]').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true, // 月份、年份有下拉選單可選擇
        autoUpdateInput: false,
        locale: {
            format: "YYYY-MM-DD"
        }
    }, function (start, label) {
        let appElement = document.querySelector('[ng-controller=RegCtrl]');
        let $scope = angular.element(appElement).scope();
        $scope.member.birthday = start.format('YYYY-MM-DD');
        $scope.$apply();
    });
});

window.onload = Register.doLoad;