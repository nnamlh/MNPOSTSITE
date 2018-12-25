var app = angular.module('myApp', ['ui.bootstrap', 'ui.mask', 'myDirective']);

app.controller('myCtrl', function ($scope, $http) {
    // phan trang
    $scope.documents = [];
    $scope.status = [
        {
            "code": 0,
            "name": "CHƯA THANH TOÁN"
        },
        {
            "code": 1,
            "name": "ĐÃ THANH TOÁN"
        }
    ];
    $scope.getData = function () {
        showLoader(true);

        $http.get("/donhang/GetDebitNoPaid").then(function (response) {
            showLoader(false);
            var result = angular.fromJson(response.data);
            $scope.documents = result.data;
        });

    };

    $scope.getData();

    $scope.document = {};
    $scope.showDetail = function (idx) {

        $scope.document = angular.copy($scope.documents[idx]);
        $('.nav-tabs a[href="#tab_chitiet"]').tab('show');

        $scope.GetDetail();

    };

    $scope.GetDocument = function () {
        showLoader(true);

        $http.get("/donhang/FindDebit?month=" + $scope.document.DMonthn + "&year=" + $scope.document.DYear).then(function (response) {
            showLoader(false);
            
            var result = angular.fromJson(response.data);
            $scope.document = result.data;
            console.log($scope.document);
            if ($scope.document !== null) {
                $scope.GetDetail();
            }
        });
    };
    $scope.details = [];
    $scope.GetDetail = function () {
        showLoader(true);

        $http.get("/donhang/DebitDetail?documentId=" + $scope.document.DocumentID).then(function (response) {
            showLoader(false);

            var result = angular.fromJson(response.data);
            $scope.details = result.data;
        });
    };

});