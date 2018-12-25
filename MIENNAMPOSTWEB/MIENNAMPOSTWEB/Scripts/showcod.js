var app = angular.module('myApp', ['ui.bootstrap', 'ui.mask', 'myDirective']);


app.controller('myCtrl', function ($scope, $http) {
    // phan trang
    $scope.numPages;
    $scope.itemPerPage;
    $scope.totalItems;
    $scope.currentPage = 1;
    $scope.maxSize = 5;
    $scope.documents = [];
    $scope.document = {};
    $scope.details = [];
    $scope.status = [
        {
            "code": 0,
            "name": "CHƯA THANH TOÁN"
        },
        {
            "code": 1,
            "name": "LẬP PHIẾU THANH TOÁN"
        },
        {
            "code": 2,
            "name": "ĐÃ THANH TOÁN"
        }
    ];

    $scope.ECoDStatus = [
        {
            "code": 0,
            "name": "CHƯA THU"
        },
        {
            "code": 1,
            "name": "ĐÃ THU"
        }
    ];


    $scope.pageChanged = function () {
        $scope.GetData();
    };

    $scope.searchInfo = {
        "customerId": info,
        "fromDate": fromDate,
        "toDate": toDate,
        "page": $scope.currentPage
    };

    $scope.GetData = function () {
        $scope.searchInfo.page = $scope.currentPage;

        showLoader(true);
        $http({
            method: "POST",
            url: "/donhang/GetCoDBill",
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

                $scope.documents = result.data;

            } else {
                alert(result.msg);
            }

        }, function myError(response) {
            showLoader(false);
            alert('Connect error');
        });
    };
    $scope.GetData();


    $scope.reportCoD = function () {
        $http.get('/donhang/ReportCoD').then(function (response) {

            var result = angular.fromJson(response.data);

            $scope.countMailer = result.data.countMailer;
            $scope.sumCoD = result.data.sumCoD;

        });
    };

    $scope.reportCoD();


    $scope.showDetails = function (idx) {
        $scope.document = angular.copy($scope.documents[idx]);
        showLoader(true);
        $http.get("/donhang/GetCoDBillDetail?documentId=" + $scope.document.DocumentID).then(function (response) {
            $('.nav-tabs a[href="#tab_chitiet"]').tab('show');
            showLoader(false);
            var result = angular.fromJson(response.data);
            $scope.details = angular.copy(result.data);
        });
    };


});