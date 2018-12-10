var app = angular.module('myApp', ['ui.uploader']);
app.controller('myCtrl', function ($scope, $http, uiUploader) {

    $scope.cusInfo = {};
    $scope.merchandises = [{ 'code': 'H', 'name': 'Hàng hóa' }, { 'code': 'T', 'name': 'Tài liệu' }];
    $scope.mailerTypes = [];
    $scope.payments = [];
    $scope.provinces = [];
    $scope.districts = [];
    $scope.wards = [];

    $scope.listMailers = [];

    $scope.getSender = function () {
        showLoader(true);

        $http({
            method: 'GET',
            url: '/user/getinfo'
        }).then(function sucess(response) {

            showLoader(false);

            var result = angular.fromJson(response.data);

            if (result.error === 1) {
                alert('Không lấy được dữ liệu');
            } else {
                $scope.cusInfo = result.data;

                if ($scope.cusInfo.Address === '' || $scope.cusInfo.ProvinceID === '' || $scope.cusInfo.DistrictID === '' || $scope.cusInfo.CustomerName === '') {
                    $scope.msg = 'Bạn cần cập nhật thông tin cá nhân trước khi tạo đơn hàng';

                    showModelFix('showInfo');
                } else {

                    $scope.firstLoad();
                }
            }

        }, function error() {
            showLoader(false);
            alert('Không tải được dữ liệu');
        });

    };

    $scope.getSender();


    $scope.firstLoad = function () {
        $http.get("/user/getprovince").then(function (response) {
            $scope.provinces = angular.fromJson(response.data);
        });
        $http.get("/donhang/GetSetting").then(function (response) {
            var result = angular.fromJson(response.data);

            $scope.mailerTypes = result.data.MailerTypes;

            $scope.payments = result.data.Payments;

        });
    };


    // upload file
    $scope.files = [];
    $scope.insertByExcel = function () {

        showLoader(true);
        uiUploader.startUpload({
            url: '/donhang/insertbyexcel',
            concurrency: 2,
            data: {
                senderID: $scope.cusInfo.CustomerCode
            },
            paramName: 'files',
            onProgress: function (file) {
                $scope.$apply();
            },
            onCompleted: function (file, response) {

                showLoader(false);
                var result = angular.fromJson(response);
                console.log(result);
                uiUploader.removeAll();
                inserByExcelElement.value = "";
                if (result.error === 1) {
                    alert(result.msg);
                } else {

                    for (var i = 0; i < result.data.length; i++) {
                        $scope.listMailers.push(result.data[i]);

                    }
                    $scope.$apply();
                }
            }
        });

    };


    var inserByExcelElement = document.getElementById('insertByExcel');
    inserByExcelElement.addEventListener('change', function (e) {
        uiUploader.removeAll();
        var files = e.target.files;
        uiUploader.addFiles(files);
        $scope.files = uiUploader.getFiles();
        $scope.$apply();

        $scope.insertByExcel();
    });

    
    $scope.mailer = {};
    $scope.currentIdx = -1;
    $scope.edit = function (idx) {
        $scope.currentIdx = idx;
        $scope.mailer = angular.copy($scope.listMailers[idx]);
        $scope.provinceChange('d');
        $scope.provinceChange('w');

        showModel('editmailer');

    };
    $scope.provinceChange = function (type) {

        var url = '';

        if (type === 'd') {
            url = '/user/getdistrict?provinceId=' + $scope.mailer.RecieverProvinceID
        } else {
            url = '/user/getward?districtId=' + $scope.mailer.RecieverDistrictID
        }

        $http.get(url).then(function (response) {
            if (type === 'd') {
                $scope.districts = angular.fromJson(response.data);
            } else {
                $scope.wards = angular.fromJson(response.data);
            }
        });

    };

    $scope.sendCreate = function () {

        if ($scope.listMailers.length === 0) {
            alert('Không có đơn để tạo');
        } else {
            showLoader(true);
            $http({
                method: "POST",
                url: "/donhang/AddListOrder",
                data: {
                    data: JSON.stringify($scope.listMailers)
                }
            }).then(function mySuccess(response) {
                showLoader(false);

                var result = angular.fromJson(response.data);

                if (result.error === 1) {
                    alert(result.msg);
                } else {
                    $scope.listMailers = [];
                    $scope.mailer = {};

                    alert('Đã tạo thành công');
                }

            }, function myError(response) {
                showLoader(false);
                alert('Connect error');

            });
        }

    };

    $scope.updateMailer = function () {
        $scope.listMailers[$scope.currentIdx] = angular.copy($scope.mailer);
        hideModel('editmailer');
    };

    $scope.calPrice = function () {
        showLoader(true);
        $http({
            method: "POST",
            url: "/donhang/CalBillPrice",
            data: {
                'weight': $scope.mailer.Weight,
                'customerId': $scope.mailer.SenderID,
                'provinceId': $scope.mailer.RecieverProvinceID,
                'serviceTypeId': $scope.mailer.MailerTypeID
            }
        }).then(function mySuccess(response) {
            showLoader(false);
            var result = angular.fromJson(response.data);
            $scope.mailer.PriceDefault = result.data.price;
            $scope.mailer.PriceCoD = result.data.codPrice;

            $scope.mailer.Amount = $scope.mailer.PriceDefault + $scope.mailer.PriceCoD + $scope.mailer.PriceService;

        }, function myError(response) {
            showLoader(false);
            alert('Connect error');

        });
    };
   
});
