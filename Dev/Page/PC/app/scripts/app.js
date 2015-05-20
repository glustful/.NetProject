'use strict';

/**
 * @ngdoc overview
 * @name app
 * @description
 * # app
 *
 * Main module of the application.
 */
angular
  .module('app', [
    'ngAnimate',
    'ngAria',
    'ngCookies',
    'ngMessages',
    'ngResource',
    'ngSanitize',
    'ngTouch',
    'ngMaterial',
    'ngStorage',
    'ngStore',
    'ui.router',
    'ui.utils',
    'ui.bootstrap',
    'ui.load',
    'ui.jp',
    'pascalprecht.translate',
    'oc.lazyLoad',
    'angular-loading-bar'
  ]);

var SETTING = {
  ApiUrl:'http://localhost:50597/api'
};

console.log("////////////////////////////////////////////////////////////////////\n"+
    "//							_ooOoo_								  //\n"+
    "//						   o8888888o							  //\n"+
    "//						   88\" . \"88							  //\n"+
    "//						   (| ^_^ |)							  //\n"+
    "//						   O\\  =  /O							  //\n"+
    "//						____/`---'\\____							  //\n"+
    "//					  .'  \\\\|     |//  `.						  //\n"+
    "//					 /  \\\\|||  :  |||//  \\						  //\n"+
    "//				    /  _||||| -:- |||||-  \\						  //\n"+
    "//				    |   | \\\\\\  -  /// |   |						  //\n"+
    "//					| \\_|  ''\\---/''  |   |						  //\n"+
    "//					\\  .-\\__  `-`  ___/-. /						  //\n"+
    "//					\\  .-\\__  `-`  ___/-. /						  //\n"+
    "//				.\"\" '<  `.___\\_<|>_/___.'  >'\"\".				  //\n"+
    "//			  | | :  `- \\`.;`\\ _ /`;.`/ - ` : | |				  //\n"+
    "//			  \\  \\ `-.   \\_ __\\ /__ _/   .-` /  /                 //\n"+
    "//		========`-.____`-.___\\_____/___.-`____.-'========		  //\n"+
    "//				             `=---='                              //\n"+
    "//		^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^        //\n"+
    "//         佛祖保佑       永无BUG		永不修改		  	  	          //\n"+
    "////////////////////////////////////////////////////////////////////\n");