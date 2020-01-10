var app = angular.module('myApp', []);

app.controller('myCtrl', function ($scope, $http) {

    $scope.mailerId = trackId;

    $scope.mailer = {};

    $scope.tracks = [];

    $scope.images = [];

    $scope.tracuu = function () {
        $scope.mailer = {};

        $scope.tracks = [];

        $http.get("/tracuu/GetInfo?mailerId=" + $scope.mailerId).then(function (response) {

            var result = angular.fromJson(response.data);

            if (result.error === 1) {
                alert('Không tìm thấy mã vận đơn');
            } else {
                $scope.mailer = result.data.info;
                $scope.tracks = result.data.tracks;
                $scope.images = result.data.images;
            }

        });
    };

    $scope.findService = function (id) {

        if (id === 'SN') {
            return 'Chuyển phát nhanh';
        } else if (id === 'ST') {
            return 'Chuyển phát tiết kiệm';
        } else {
            return '';
        }
    };

    if ($scope.mailerId !== '') {
        $scope.tracuu();
    }


});