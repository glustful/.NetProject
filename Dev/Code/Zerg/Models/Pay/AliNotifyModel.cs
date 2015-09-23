using System;

namespace Zerg.Models.Pay
{
    public class AliNotifyModel
    {
        public DateTime notify_time { get; set; }

        public string notify_type { get; set; }

        public string notify_id { get; set; }
        public string sign_type { get; set; }
        public string sign { get; set; }
        public string out_trade_no { get; set; }
        public string subject { get; set; }
        public string payment_type { get; set; }
        public string trade_No { get; set; }
        public string trade_status { get; set; }
        public DateTime gmt_create { get; set; }
        public DateTime gmt_payment { get; set; }
        public DateTime gmt_close { get; set; }
        public string refund_status { get; set; }
        public DateTime gmt_refund { get; set; }
        public string seller_email { get; set; }
        public string buyer_id { get; set; }
        public decimal price { get; set; }
        public decimal total_fee { get; set; }
        public decimal quantity { get; set; }
        public string body { get; set; }
        public decimal discount { get; set; }
        public string is_total_fee_adjust { get; set; }
        public string use_coupon { get; set; }
        public string extra_common_param { get; set; }
        public string out_channel_type { get; set; }
        public string out_channel_amount { get; set; }
        public string out_channel_inst { get; set; }
        public string business_scene { get; set; }
    }
}