app.config(function($ionicConfigProvider) {
           $ionicConfigProvider.views.maxCache(2);
           $ionicConfigProvider.tabs.position("bottom");
           $ionicConfigProvider.views.transition("ios");
           $ionicConfigProvider.backButton.text("返回");
           $ionicConfigProvider.backButton.previousTitleText(false);
           $ionicConfigProvider.tabs.style("standard");
           $ionicConfigProvider.navBar.alignTitle("center");
           });