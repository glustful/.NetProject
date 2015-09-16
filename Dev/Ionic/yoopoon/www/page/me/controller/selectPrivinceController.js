app.controller('selectAddress', function($scope, $stateParams) {

    $scope.chats = [
        {
            id: 0,
            name: '北京市',
            lastText: 'You on your way?',
            face: 'https://pbs.twimg.com/profile_images/514549811765211136/9SgAuHeY.png'
        },
        {
            id: 1,
            name: '天津市',
            lastText: 'You on your way?',
            face: 'https://pbs.twimg.com/profile_images/514549811765211136/9SgAuHeY.png'
        }
    ];
    alert($scope.chats.name);

    $scope.goBack = function () {
        window.history.go(-1);
    };
})