using System.ComponentModel;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.PartnerList;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Zerg.Common;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 合伙人  李洪亮  2015-05-05
    /// </summary>
    public class PartnerListController : ApiController
    {
        private readonly IPartnerListService _partnerlistService;
        private readonly IBrokerService _brokerService;
        private readonly IWorkContext _workContext;
        public PartnerListController(IPartnerListService partnerlistService, IBrokerService brokerService, IWorkContext workContext)
        {
            _partnerlistService = partnerlistService;
            _brokerService = brokerService;
            _workContext = workContext;
        }

        #region 合伙人详情


        /// <summary>
        /// 查询经纪人及他所属的合伙人
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage SearchPartnerList(EnumPartnerType status, string name = null, int page = 1, int pageSize = 10)
        {
            var brokerSearchCondition = new BrokerSearchCondition
            {
                Brokername = name,
                Status = status,
                Page = Convert.ToInt32(page),
                PageCount =pageSize
            };
            var partnerList = _brokerService.GetBrokersByCondition(brokerSearchCondition).Select(p => new
            {
                p.Id,
                p.PartnersName,
                p.PartnersId,
                BrokerName = p.Brokername,
                Phone = p.Phone,
                Regtime = p.Regtime,
                Agentlevel = p.Agentlevel,
                Headphoto=p.Headphoto,
                status=EnumPartnerType.同意
            });
            var partnerListCount = _brokerService.GetBrokerCount(brokerSearchCondition);
            return PageHelper.toJson(new { List = partnerList, Condition = brokerSearchCondition, totalCount = partnerListCount });
             
        }

        /// <summary>
        /// 查询经纪人下的合伙人List
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <returns></returns>
        [Description("查询经纪人下的合伙人List")]
        [HttpGet]
        public HttpResponseMessage PartnerListDetailed(int userId)
        {
            var partnerlistsearchcon = new PartnerListSearchCondition
            {
                Brokers = _brokerService.GetBrokerByUserId(userId),
                Status=EnumPartnerType.同意,
            };

            var partnerList = _partnerlistService.GetPartnerListsByCondition(partnerlistsearchcon).Where(p => p.Broker.UserId == userId).Select(p => new
                {
                 Name=p.Brokername,
                 AddTime =p.Addtime,
                 regtime=p.Regtime, 
                 Phone=p.Phone,
                Headphoto= p.Broker .Headphoto ,
                Id=p.Id,
                PartnerId=p.PartnerId
              
                }).ToList();

            return PageHelper.toJson(new { list = partnerList });

        }

        /// <summary>
        /// 新增合伙人
        /// </summary>
        /// <param name="partnerList"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddPartnerList([FromBody] PartnerListEntity partnerList)
        {
            var sech = new BrokerSearchCondition
            {
                Phone = partnerList.Phone
            };
              var user = (UserBase)_workContext.CurrentUser;
              if (user != null)
              {
                  var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前用户
                  if (broker != null)
                  {
                           var getbroker = _brokerService.GetBrokersByCondition(sech).FirstOrDefault();//通过手机号查询经纪人
                           if (getbroker != null)
                           {
                               //判断是否是本身
                               if (broker.Id==getbroker.Id)
                               {
                                   return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起，不能添加本身"));
                               }
                               //判断要添加的这个经纪人是否有上家
                               if (getbroker.PartnersId== null || getbroker.PartnersId==0)//查询他的上家是否存在
                               {                                                                  
                                   //1 添加到partnerList表中  
                                                                   
                                   var entity = new PartnerListEntity();
                                   entity.PartnerId = getbroker.Id;     //添加的下家
                                   entity.Brokername =getbroker.Brokername;
                                   entity.Phone = getbroker.Phone;
                                   entity.Agentlevel = getbroker.Level.Name;
                                   entity.Regtime = getbroker.Regtime;

                                   entity.Adduser = broker.Id;
                                   entity.Addtime = DateTime.Now;
                                   entity.Upuser = broker.Id;
                                   entity.Uptime = DateTime.Now;                               
                                   entity.Broker = broker;                                                               
                                   entity.Status = EnumPartnerType.默认;
                                   //判断当前用户的合伙人个数是否》=3
                                   
                                   PartnerListSearchCondition plsearCon=new PartnerListSearchCondition
                                   {
                                        Brokers=broker,
                                        Status = EnumPartnerType.同意
                                   };
                                if( _partnerlistService.GetPartnerListCount(plsearCon)>=3)
                                {
                                    return   PageHelper.toJson(PageHelper.ReturnValue(true, "对不起，您的合伙人数已满 不能添加"));
                                }

                                   if (_partnerlistService.Create(entity) != null)
                                   {
                                   
                                         return PageHelper.toJson(PageHelper.ReturnValue(true, "邀请成功！等待对方同意"));                                  
                                  
                                   }else
                                   {
                                       return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新失败！请与客服联系"));   
                                   }
                                  

                               }else
                               {
                                   return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户已经是别人的合伙人了！"));

                               }


                           }else
                           {
                               return PageHelper.toJson(PageHelper.ReturnValue(false, "该经纪人不存在"));
                           }
                  }
              }
              return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起，请登录"));






            //var list = _brokerService.GetBrokersByCondition(sech).FirstOrDefault();//通过手机号查询经纪人
            //if (list != null)
            //{
            //    if (list.PartnersId != 0)//查询他的上家是否存在
            //    {
            //        if (partnerList != null)
            //        {
            //            var entity = new PartnerListEntity();
            //            entity.PartnerId = list.Id;     //添加的下家
            //            entity.Phone = partnerList.Phone;

            //            if (list.Id == _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Id)
            //            {
            //                return PageHelper.toJson(PageHelper.ReturnValue(false, "不能添加自身！"));
            //            }

            //            //上家的属性
            //                 entity.Agentlevel = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Level.Name;
            //                 entity.Brokername = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Brokername;

            //                 entity.Regtime = DateTime.Now;
            //                 entity.Broker = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id);

            //                 entity.Upuser = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Id;
            //                 entity.Uptime = DateTime.Now;
            //                 entity.Adduser = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Id;
            //                 entity.Addtime = DateTime.Now;
            //                 entity.Status = EnumPartnerType.默认;

            //            try
            //            {
            //                if (_partnerlistService.Create(entity) != null)
            //                {
            //                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！等待对方同意"));
            //                }
            //            }
            //            catch
            //            {
            //                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
            //            }
            //        }
            //        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            //    }
            //    return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户已经是别人的合伙人了！"));
            //}
            //return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户不存在"));


        }

        /// <summary>
        /// 查询经纪人收到的邀请
        /// </summary>
        /// <param name="brokerId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetInviteForBroker(int brokerId=0)
        {
            if (brokerId == 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));

            if (_partnerlistService.GetInviteForBroker(_brokerService.GetBrokerByUserId(brokerId).Id,EnumPartnerType.默认).Any())
          {
              var list = _partnerlistService.GetInviteForBroker(_brokerService.GetBrokerByUserId(brokerId).Id, EnumPartnerType.默认).Select(a=>new
              {
                  a.Id,
                  a.PartnerId,
                  HeadPhoto = a.Broker.Headphoto,
                  BrokerName = a.Broker.Brokername,
                  AddTime = a.Addtime
              }).ToList().Select(b=>new
              {
                  b.Id,
                  b.PartnerId,
                  b.HeadPhoto,
                  b.BrokerName,
                  AddTime = b.AddTime.ToString("yyy-mm-dd")
              });

              return PageHelper.toJson(new { list });
          }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "当前没有邀请"));
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
       [HttpGet]
        public HttpResponseMessage SetPartner(EnumPartnerType status,int id=0)
        {
            if (id == 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));

            var model = _partnerlistService.GetPartnerListById(id);
            model.Status = status;
           if(status==EnumPartnerType.拒绝)
           {
               _partnerlistService.Update(model);
               return   PageHelper.toJson(PageHelper.ReturnValue(true, "拒绝成功！"));
           }

           else
           {



             var user = (UserBase)_workContext.CurrentUser;
             if (user != null)
             {
                 var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前用户
                 if (broker != null)
                 {
                       //判断要添加的这个经纪人是否有上家
                     if (broker.PartnersId == null || broker.PartnersId == 0)//查询他的上家是否存在
                     {

                         // 更改状态为同意 1 ，  
                         if (_partnerlistService.Update(model) != null)
                         {
                             //2更新 当前要添加的这个经纪人的PartnersId字段为当前用户
                             broker.PartnersId = model.Broker.Id;
                             broker.PartnersName = model.Broker.Brokername;
                             if (_brokerService.Update(broker) != null)
                             {

                                 return PageHelper.toJson(PageHelper.ReturnValue(true, "邀请成功！"));
                             }

                         }
                     }
                     else
                     {
                         model.Status = EnumPartnerType.拒绝;
                         _partnerlistService.Update(model);
                         return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起,您已经是别人的合伙人了！"));
                     }

                 }
             }
             return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起，请先登录"));

           }
           return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起，数据错误"));
        }


        #endregion


    }
}
