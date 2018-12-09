var app = angular.module('myApp', ['ui.bootstrap', 'myDirective', 'myKeyPress', 'ui.select2', 'ui.mask']);

app.controller('myCtrl', function ($scope, $http, $rootScope) {

    $scope.numPages;
    $scope.itemPerPage;
    $scope.totalItems;
    $scope.currentPage = 1;
    $scope.maxSize = 5;

    $scope.status = [{ "code": 0, "name": "KHỞI TẠO" }, { "code": 1, "name": "ĐANG GỬI LIÊN TUYÊN" }, { "code": 2, "name": "ĐÃ NHẬN" }, { "code": 3, "name": "ĐANG PHÁT" }, { "code": 4, "name": "ĐÃ PHÁT" }, { "code": 5, "name": "CHUYỂN HOÀN" }, { "code": 6, "name": "CHƯA PHÁT ĐƯỢC" }, { "code": 7, "name": "ĐANG ĐI LẤY HÀNG" }, { "code": 8, "name": "ĐÃ LẤY HÀNG" }, { "code": 9, "name": "GIAO ĐỐI TÁC PHÁT" }, { "code": 10, "name": "HỦY ĐƠN" }];
    $scope.status.unshift({ "code": -1, "name": "TẤT CẢ" });
    $scope.findStatus = function (code) {
        for (var i = 0; i < $scope.status.length; i++) {
            if ($scope.status[i].code === code)
                return $scope.status[i].name;
        }

    };


    $scope.pageChanged = function () {
        $scope.GetData();
    };

    $scope.searchInfo = {
        "search": "",
        "customerId": info,
        "fromDate": fromDate,
        "toDate": toDate,
        "status": -1,
        "page": $scope.currentPage
    };

    $scope.mailers = [];
    $scope.GetData = function () {
        $scope.searchInfo.page = $scope.currentPage;

        showLoader(true);
        $http({
            method: "POST",
            url: "/donhang/GetData",
            data: {
                data: JSON.stringify($scope.searchInfo)
            }
        }).then(function mySuccess(response) {
            showLoader(false);

            var result = angular.fromJson(response.data);
            if (result.error === 0) {
                $scope.itemPerPage = result.pageSize;
                $scope.totalItems = result.toltalSize;
                $scope.numPages = Math.round($scope.totalItems / $scope.itemPerPage);

                $scope.mailers = result.data;

            } else {
                alert(result.msg);
            }

        }, function myError(response) {
            showLoader(false);
            alert('Connect error');
        });
    }
    $scope.GetData();
    $scope.checkAll = function () {
        for (var i = 0; i < $scope.mailers.length; i++) {
            $scope.mailers[i].isCheck = $scope.checkMailers;
        }
    };


});