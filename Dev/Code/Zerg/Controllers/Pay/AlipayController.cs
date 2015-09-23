using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Community.Service.Order;
using Community.Service.ServiceOrder;
using Zerg.Models.Pay;
using System.Threading.Tasks;
using Community.Entity.Model.Order;
using Community.Entity.Model.ServiceOrder;

namespace Zerg.Controllers.Pay
{
    public class AlipayController : ApiController
    {
        private readonly IOrderService _orderService;
        private readonly IServiceOrderService _serviceOrderService;

        private string _partner = "2088311414553838";               //合作身份者ID
        private string _key = "";                   //商户的私钥
        private string _input_charset = "";         //编码格式
        private string _sign_type = "";             //签名方式
        private string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";

        public AlipayController(IOrderService orderService,IServiceOrderService serviceOrderService)
        {
            _orderService = orderService;
            _serviceOrderService = serviceOrderService;
        }

        [HttpPost]
        public async Task<string> Success([FromBody] AliNotifyModel model)
        {
            if (model.trade_status == "TRADE_FINISHED")
            {
                //判断该笔订单是否在商户网站中已经做过处理
                //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                //如果有做过处理，不执行商户的业务程序

                //注意：
                //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                return "success";
            }
            if (model.trade_status == "TRADE_SUCCESS")
            {
                //判断该笔订单是否在商户网站中已经做过处理
                //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                //如果有做过处理，不执行商户的业务程序

                //注意：
                //付款完成后，支付宝系统发送该交易状态通知
                return "success";
            }
            var isVerifyied = await Verify(model.sign, model.notify_id);
            var orderNo = model.out_trade_no;
            bool isSuccess = false;
            if (orderNo.StartsWith("O"))
            {
                var order = _orderService.GetOrderByNo(orderNo);
                order.UpdDate = DateTime.Now;
                order.UpdUser = 1; //默认1为管理员
                order.Status = EnumOrderStatus.Payed;
                if (isVerifyied)
                {
                    if (_orderService.Update(order) != null)
                        isSuccess = true;
                }
                   
            }
            else
            {
                var order = _serviceOrderService.GetServiceOrderByNo(orderNo);
                order.UpdTime = DateTime.Now;
                order.UpdUser = 1; //默认1为管理员
                order.Status = EnumServiceOrderStatus.Payed;
                if (_serviceOrderService.Update(order) != null)
                    isSuccess = true;
            }
            return isSuccess ? "success" : "fail";
        }

        private async Task<bool> Verify(string sign,string notify_id)
        {
            //todo:进行签名验证
            var veryfyUrl = Https_veryfy_url + "partner=" + _partner + "&notify_id=" + notify_id;

            string strResult;
            try
            {
                var myReq = (HttpWebRequest)WebRequest.Create(veryfyUrl);
                myReq.Timeout = 3000000;
                var myStream = await myReq.GetRequestStreamAsync();
                //Stream myStream = HttpWResp.GetResponseStream();
                var sr = new StreamReader(myStream, Encoding.Default);
                var strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
            }
            return strResult == "true";
        }
    }
}
