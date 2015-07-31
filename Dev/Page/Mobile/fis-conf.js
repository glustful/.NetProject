/**
 * Created by Administrator on 2015/7/28.
 */
// 下面的开启simple插件，注意需要先进行插件安装 npm install -g fis-postpackager-simple
//fis.config.set('modules.postpackager', 'simple');
////排除
//fis.config.set('project.exclude', [
//    'libs/vendor/jquery/src/**'
//]);
//
//// 取消下面的注释设置打包规则
//fis.config.set('pack', {
//    'pkg/App.js': [
//        'app.js',
//        'Common/scripts/router.js',
//        'Common/scripts/services/authentication.js',
//        'Common/scripts/directives/access.js'
//    ],
//    'pkg/Vendor.js': [
//        'libs/vendor/angular/angular.js',
//        'libs/vendor/angular-cookies/angular-cookies.js',
//        'libs/vendor/ui-router/release/angular-ui-router.min.js',
//        'libs/vendor/oclazyload/dist/ocLazyLoad.js',
//        'libs/vendor/ngstorage/ngStorage.js',
//        'libs/vendor/jquery/dist/jquery.js',
//        'libs/vendor/angular-bootstrap/ui-bootstrap-tpls.js'
//    ],
//    // 下面设置CSS打包规则
//    '/pkg/style.css': [
//        'libs/bootstrap/css/bootstrap.css',
//        'Common/static/style/common.css',
//        //'modules/**.css'
//        'modules/Index/static/style/index.css',
//        'modules/activity/static/style/activity.css',
//        'modules/broker/static/style/broker.css',
//        'modules/customerList/static/style/customerList.css',
//        'modules/detail/static/style/detail.css',
//        'modules/hero/static/style/hero.css',
//        'modules/houseDetail/static/style/houseDetail.css',
//        'modules/houses/static/style/houses.css',
//        'modules/housesBuy/static/style/housesBuy.css',
//        'modules/housesPic/static/style/housesPic.css',
//        'modules/housesPicBuy/static/style/housesPicBuy.css',
//        'modules/myInt/static/style/myInt.css',
//        'modules/myPurse/static/style/myPurse.css',
//        'modules/personal/static/style/personal.css',
//        'modules/personalPage/static/style/personalPage.css',
//        'modules/recommend/static/style/recommend.css',
//        'modules/takeOff/static/style/takeOff.css',
//        'modules/redPaper-1/static/style/redPaper.css',
//        'modules/storeroom/static/style/storeroom.css',
//        'modules/grabPacket/static/style/grabPacket.css',
//        'modules/sendPacket/static/style/sendPacket.css',
//        'modules/luckPacket/static/style/luckPacket.css',
//        'modules/chip/static/style/chip.css',
//        'modules/withdrawals/static/style/withdrawals.css',
//        'modules/Auction/static/style/Auction.css',
//        'modules/Coupons/static/style/Coupons.css',
//        'modules/NoviceTask/static/style/NoviceTask.css',
//        'modules/zhongtian_HouseDetail/static/style/zhongtian_HouseDetail.css',
//        'modules/partner_list/static/style/partner.css',
//        'modules/security_setting/static/style/setting.css',
//        'modules/credit_add/static/style/credit_add.css',
//        'modules/groom/static/style/groom.css',
//        'modules/task/static/style/task.css',
//        'modules/nominate/static/style/nominate.css',
//        'modules/Login/static/style/login.css',
//        'modules/Register/static/style/register.css',
//        'modules/PasswordFound/static/style/PasswordFound.css',
//        'modules/cash/static/style/cash.css',
//        'modules/BrandDetail/static/style/BrandDetail.css',
//        'modules/selectWithdraw/static/selectWithdraw.css'
//    ]
//});

/**
 * 以下是fis3的配置
 * 使用前请确保安装了fis3，及postpackager-loader
 * npm install -g fis3-postpackager-loader
 **/
fis.set('project.files', ['Common/**', 'app.js', 'index.html', 'modules/**']);
fis.media("p")
    .match('::package', {
        postpackager: fis.plugin('loader')
    }).match('*.{js,css,png}', {
        useHash: true
    }).match('::image', {
        useHash: true
    }).match('**.js', {
        // fis-optimizer-uglify-js 插件进行压缩，已内置
        optimizer: fis.plugin('uglify-js')
    })
    .match('**.css', {
        optimizer: fis.plugin('clean-css')
    })
/**
 * application部分
 */
    .match('app.js',{
        packTo: 'application.js'
    })
    .match('Common/scripts/router.js',{
        packTo: 'application.js'
    })
    .match('Common/scripts/services/authentication.js',{
        packTo: 'application.js'
    })
    .match('Common/scripts/directives/access.js',{
        packTo: 'application.js'
    })
/**
 * vendor部分
 **/
    .match('libs/vendor/angular/angular.js', {
        packTo: 'vendor.js'
    })
    .match('libs/vendor/angular-cookies/angular-cookies.js', {
        packTo: 'vendor.js'
    })
    .match('libs/vendor/ui-router/release/angular-ui-router.min.js', {
        packTo: 'vendor.js'
    })
    .match('libs/vendor/oclazyload/dist/ocLazyLoad.js', {
        packTo: 'vendor.js'
    })
    .match('libs/vendor/ngstorage/ngStorage.js', {
        packTo: 'vendor.js'
    })
    .match('libs/vendor/jquery/dist/jquery.js', {
        packTo: 'vendor.js'
    })
    .match('libs/vendor/angular-bootstrap/ui-bootstrap-tpls.js', {
        packTo: 'vendor.js'
    })
/**
 * CSS部分
 **/
    .match('libs/bootstrap/css/bootstrap.css', {
        packTo: 'style.css'
    })
    .match('Common/**/*.css', {
        packTo: 'style.css'
    })
    .match('modules/Index/static/style/index.css',{
        packTo: 'style.css'
    })
    .match('modules/activity/static/style/activity.css',{
        packTo: 'style.css'
    })
    .match('modules/broker/static/style/broker.css',{
        packTo: 'style.css'
    })
    .match('modules/customerList/static/style/customerList.css',{
        packTo: 'style.css'
    })
    .match('modules/detail/static/style/detail.css',{
        packTo: 'style.css'
    })
    .match('modules/hero/static/style/hero.css',{
        packTo: 'style.css'
    })
    .match('modules/houseDetail/static/style/houseDetail.css',{
        packTo: 'style.css'
    })
    .match('modules/houses/static/style/houses.css',{
        packTo: 'style.css'
    })
    .match('modules/housesBuy/static/style/housesBuy.css',{
        packTo: 'style.css'
    })
    .match('modules/housesPic/static/style/housesPic.css',{
        packTo: 'style.css'
    })
    .match('modules/housesPicBuy/static/style/housesPicBuy.css',{
        packTo: 'style.css'
    })
    .match('modules/myInt/static/style/myInt.css',{
        packTo: 'style.css'
    })
    .match('modules/myPurse/static/style/myPurse.css',{
        packTo: 'style.css'
    })
    .match('modules/personal/static/style/personal.css',{
        packTo: 'style.css'
    })
    .match('modules/personalPage/static/style/personalPage.css',{
        packTo: 'style.css'
    })
    .match('modules/recommend/static/style/recommend.css',{
        packTo: 'style.css'
    })
    .match('modules/takeOff/static/style/takeOff.css',{
        packTo: 'style.css'
    })
    .match('modules/storeroom/static/style/storeroom.css',{
        packTo: 'style.css'
    })
    .match('modules/grabPacket/static/style/grabPacket.css',{
        packTo: 'style.css'
    })
    .match('modules/sendPacket/static/style/sendPacket.css',{
        packTo: 'style.css'
    })
    .match('modules/luckPacket/static/style/luckPacket.css',{
        packTo: 'style.css'
    })
    .match('modules/chip/static/style/chip.css',{
        packTo: 'style.css'
    })
    .match('modules/withdrawals/static/style/withdrawals.css',{
        packTo: 'style.css'
    })
    .match('modules/Auction/static/style/Auction.css',{
        packTo: 'style.css'
    })
    .match('modules/Coupons/static/style/Coupons.css',{
        packTo: 'style.css'
    })
    .match('modules/NoviceTask/static/style/NoviceTask.css',{
        packTo: 'style.css'
    })
    .match('modules/zhongtian_HouseDetail/static/style/zhongtian_HouseDetail.css',{
        packTo: 'style.css'
    })
    .match('modules/partner_list/static/style/partner.css',{
        packTo: 'style.css'
    })
    .match('modules/security_setting/static/style/setting.css',{
        packTo: 'style.css'
    })
    .match('modules/credit_add/static/style/credit_add.css',{
        packTo: 'style.css'
    })
    .match('modules/groom/static/style/groom.css',{
        packTo: 'style.css'
    })
    .match('modules/task/static/style/task.css',{
        packTo: 'style.css'
    })
    .match('modules/nominate/static/style/nominate.css',{
        packTo: 'style.css'
    })
    .match('modules/Login/static/style/login.css',{
        packTo: 'style.css'
    })
    .match('modules/Register/static/style/register.css',{
        packTo: 'style.css'
    })
    .match('modules/PasswordFound/static/style/PasswordFound.css',{
        packTo: 'style.css'
    })
    .match('modules/cash/static/style/cash.css',{
        packTo: 'style.css'
    })
    .match('modules/BrandDetail/static/style/BrandDetail.css',{
        packTo: 'style.css'
    })
    .match('modules/selectWithdraw/static/selectWithdraw.css',{
        packTo: 'style.css'
    })
    .match('modules/presentProcess/static/presentProcess.css',{
        packTo: 'style.css'
    });
