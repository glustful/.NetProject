/**
 * Created by Yunjoy on 2015/8/12.
 */

app.service("openModal", ['$state','$modal',
    function ($state, $modal) {
        var statusModal = {
            title:"",
            warning:"",
            content:"",
            close:"",
            ok:""
        };

        this.openModal = function (id,title,warning,content,close,ok,okFn,closeFn) {
        statusModal = {
            title:title,
            warning:warning,
            content:content,
            close:close,
            ok:ok
        };

        var modalInstance = $modal.open({
            templateUrl: 'app/common/layout/statusModal/view/statusModal.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                statusModal: function () {
                    return statusModal;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            //确认
            if(typeof  okFn=="function"){
                okFn();
            }
        }, function (selectedItem) {
            //取消
            if(typeof  closeFn=="function"){
                closeFn();
            }
        });
    };

}]);