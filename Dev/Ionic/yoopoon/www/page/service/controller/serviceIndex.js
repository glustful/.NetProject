/**
 * Created by chen on 2015/9/21.
 */
app.controller('serviceIndex', function($scope) {





    // узуж╡Ц

    var tip1 = document.getElementById("tiphidden1");
    var tip2 = document.getElementById("tiphidden2");
    $scope.hide=true;
    $scope.closetips = function() {
        tip1.style.display = "none";
        tip2.style.display = "none";
        localStorage.x1 = "none";
        $scope.hide=false;
    };
    $scope.hide=true;
    function save() {
        if (localStorage.x1) {
            tip1.style.display = "none";
            tip2.style.display = "none";
            $scope.hide=false;
        }
    }
    save();


});
