
var app = angular.module("mainModule", ['ngAnimate', 'ngSanitize', 'ui.bootstrap',"chart.js"])
  
  .controller("myController", function ($scope, $http) {
      var count = [];
      var label = [];
      $http.get('EmployeeService.asmx/GetAllEmployeesYearCount')
      .then(function (responce) {
          var x = responce.data;
          for (let value in x) {
              count.push(x[value].Count);
              label.push(x[value].Gender);
          }
          $scope.labels = label;
          $scope.data = count;
      });
      //$scope.labels = label;
      //$scope.series = count;

      //$scope.data = count;
      $scope.options = {
          customClass: getDayClass,
          showWeeks: true
      };
      $scope.setDate = function (year, month, day) {
          $scope.dt = new Date(year, month, day);
      };

      var tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      var afterTomorrow = new Date(tomorrow);
      afterTomorrow.setDate(tomorrow.getDate() + 1);
      $scope.events = [
        {
            date: tomorrow,
            status: 'full'
        },
        {
            date: afterTomorrow,
            status: 'partially'
        }
      ];

      function getDayClass(data) {
          var date = data.date,
            mode = data.mode;
          if (mode === 'day') {
              var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

              for (var i = 0; i < $scope.events.length; i++) {
                  var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                  if (dayToCheck === currentDay) {
                      return $scope.events[i].status;
                  }
              }
          }

          return '';
      }

      $scope.format = 'yyyy/MM/dd';

      $scope.open = function () {
          //$http.get('EmployeeService.asmx/GetAllEmployees')
          //.then(function (responce) {
          //    $scope.employee = responce.data;
          //});
          var val = !validatedate($scope.format);
          if (val) {
              alert('Invalid Date');
          }
          if(!val){
              $http({
                  url: 'EmployeeService.asmx/GetAllEmployeeByDate',
                  method: "POST",
                  data: { date: JSON.stringify($scope.format) },
                  contentType: "application/json; charset=utf-8",
                  dataType: "json"
              }).then(function (response) {
                  $scope.employee =response.data
              })}
      }

      function validatedate(dateString)
      {  
          if (!/^\d{4}\/\d{1,2}\/\d{1,2}$/.test(dateString))
              return false;

          // Parse the date parts to integers
          var parts = dateString.split("/");
          var day = parseInt(parts[2], 10);
          var month = parseInt(parts[1], 10);
          var year = parseInt(parts[0], 10);

          // Check the ranges of month and year
          if (year < 1000 || year > 3000 || month == 0 || month > 12)
              return false;

          var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

          // Adjust for leap years
          if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
              monthLength[1] = 29;

          // Check the range of the day
          return day > 0 && day <= monthLength[month - 1];
      }
      
      //$http.post('EmployeeService.asmx/GetAllDepartment')
      //.then(function (responce) {
      //    $scope.department = responce.data;
      //});
  });

