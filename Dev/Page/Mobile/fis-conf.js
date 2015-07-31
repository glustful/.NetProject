/**
 * Created by Administrator on 2015/7/28.
 */
//?????????????????
// fis.config.set('roadmap.path', [{
//     reg: '**.css',
//     useSprite: true
// }]);
// ?????????????С???
//fis.config.set('settings.spriter.csssprites.margin', 20);

// ????????simple???????????????в????? npm install -g fis-postpackager-simple
fis.config.set('modules.postpackager', 'simple');
//???
fis.config.set('project.exclude',[
    'libs/vendor/jquery/src/**'
]);
//fis.config.merge({
//    roadmap : {
//        path : [
//            {
//                //???е?js???
//                reg : 'modules/**.js',
//                //??????/static/js/xxx????
//                release : '/static/js$&'
//            },
//            //{
//            //    //???е?css???
//            //    reg : '**.css',
//            //    //??????/static/css/xxx????
//            //    release : '/static/css$&'
//            //}
//        ]
//    }
//});
// ????????????????????
fis.config.set('pack', {
    'pkg/App.js': [
        'app.js',
        'Common/scripts/router.js',
        'Common/scripts/services/authentication.js',
        'Common/scripts/directives/access.js'
    ],
    'pkg/Vendor.js':[
        'libs/vendor/angular/angular.js',
        'libs/vendor/angular-cookies/angular-cookies.js',
        'libs/vendor/ui-router/release/angular-ui-router.min.js',
        'libs/vendor/oclazyload/dist/ocLazyLoad.js',
        'libs/vendor/ngstorage/ngStorage.js',
        'libs/vendor/jquery/dist/jquery.js',
        'libs/vendor/angular-bootstrap/ui-bootstrap-tpls.js'
    ],
    //'pkg/Controller.js':[
    //   // 'modules/addBroker/controller/AddBrokerController.js',
    //   // 'modules/BrandDetail/static/script/brandDetail.js',
    //    'modules/**.js'
    //],
    // ????????CSS??????
    '/pkg/style.css': [
        'libs/bootstrap/css/bootstrap.css',
        'Common/static/style/common.css',
        //'modules/**.css'
        'modules/Index/static/style/index.css',
        'modules/activity/static/style/activity.css',
        'modules/broker/static/style/broker.css',
        'modules/customerList/static/style/customerList.css',
        'modules/detail/static/style/detail.css',
        'modules/hero/static/style/hero.css',
        'modules/houseDetail/static/style/houseDetail.css',
        'modules/houses/static/style/houses.css',
        'modules/housesBuy/static/style/housesBuy.css',
        'modules/housesPic/static/style/housesPic.css',
        'modules/housesPicBuy/static/style/housesPicBuy.css',
        'modules/myInt/static/style/myInt.css',
        'modules/myPurse/static/style/myPurse.css',
        'modules/personal/static/style/personal.css',
        'modules/personalPage/static/style/personalPage.css',
        'modules/recommend/static/style/recommend.css',
        'modules/takeOff/static/style/takeOff.css',
        'modules/redPaper-1/static/style/redPaper.css',
        'modules/storeroom/static/style/storeroom.css',
        'modules/grabPacket/static/style/grabPacket.css',
        'modules/sendPacket/static/style/sendPacket.css',
        'modules/luckPacket/static/style/luckPacket.css',
        'modules/chip/static/style/chip.css',
        'modules/withdrawals/static/style/withdrawals.css',
        'modules/Auction/static/style/Auction.css',
        'modules/Coupons/static/style/Coupons.css',
        'modules/NoviceTask/static/style/NoviceTask.css',
        'modules/zhongtian_HouseDetail/static/style/zhongtian_HouseDetail.css',
        'modules/partner_list/static/style/partner.css',
        'modules/security_setting/static/style/setting.css',
        'modules/credit_add/static/style/credit_add.css',
        'modules/groom/static/style/groom.css',
        'modules/task/static/style/task.css',
        'modules/nominate/static/style/nominate.css',
        'modules/Login/static/style/login.css',
        'modules/Register/static/style/register.css',
        'modules/PasswordFound/static/style/PasswordFound.css',
        'modules/cash/static/style/cash.css',
        'modules/BrandDetail/static/style/BrandDetail.css',
        'modules/selectWithdraw/static/selectWithdraw.css'
    ]
});
// ????????simple????????????????
//fis.config.set('settings.postpackager.simple.autoCombine', true);