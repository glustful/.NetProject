using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Event.Entity.Model;
using Event.Models;
using Event.Service.Crowd;
using Event.Service.Discount;
using Event.Service.Follower;
using Event.Service.PartImage;
using Event.Service.Phone;
using Zerg.Common;
using Event.Service.Participation;
using Event.Models;
namespace Zerg.Event.API.Crowd
{

    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [AllowAnonymous]
    public class CrowdApiController : ApiController
    {
        private readonly ICrowdService _crowdService;
        private readonly IPartImageService _partImageService;
        private readonly IDiscountService _discountService;
        private readonly IFollowerService _followerService;
        private readonly IPhoneService _phoneService;
        private readonly IParticipationService _participationService;
        public CrowdApiController(ICrowdService crowdService, IPartImageService partImageService, IDiscountService discountService, IFollowerService followerService, IPhoneService phoneService, IParticipationService participationService)
        {
            _discountService = discountService;
            _crowdService = crowdService;
            _partImageService = partImageService;
            _followerService = followerService;
            _phoneService = phoneService;
            _participationService = participationService;
        }
        #region 项目信息
        /// <summary>
        /// 查询项目表里的所有信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCrowdInfo( int status)
        {
           
            //查询项目表所有的数据
            var sech = new CrowdSearchCondition
            {
                OrderBy = EnumCrowdSearchOrderBy.OrderById,
                Statuss =status 
            };
            var list = _crowdService.GetCrowdsByCondition(sech).Select(p => new
            {
                //项目表
                p.Ttitle,
                p.Id,
                p.Status,
                p.Intro,
                p.Starttime,
                p.Endtime,
                p.Uptime,
                p.Upuser,
                p.Adduser,
                p.Addtime,
                p.crowdUrl 
            }).ToList().Select(a => new CrowdModel
            {
                //返回数据
                Id = a.Id,
                Ttitle = a.Ttitle,
                Intro = a.Intro,
                Endtime = a.Endtime,
                Starttime = a.Starttime,
                Status = a.Status,
                ImgList = _partImageService.GetPartImageByCrowdId(a.Id),
                Dislist = _discountService.GetDiscountByCrowdId(a.Id),
                //已参与众筹人数
                crowdNum = _participationService.GetParticipationCountByCrowdId(a.Id),
                //众筹最低优惠对应的人数
                crowdMaxNum = _discountService.GetDiscountMaxCountByCrowdId(a.Id),
                crowdUrl =a.crowdUrl ,
                i = 0
            }).Where(p => p.crowdMaxNum != 0);
            return PageHelper.toJson(new {list});
        }

        /// <summary>
        /// 依据项目状态来查询数据
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetCrowdByStatus(int status)
        {
            var sech = new CrowdSearchCondition
            {
                Statuss = status 
            };
            
                var list = _crowdService.GetCrowdsByCondition(sech).Select(p => new
                {
                    //项目表
                    p.Ttitle,
                    p.Id,
                    p.Status,
                    p.Intro,
                    p.Starttime,
                    p.Endtime,
                    p.Uptime,
                    p.Upuser,
                    p.Adduser,
                    p.Addtime
                }).ToList().Select(a => new CrowdModel
                {
                    //返回数据
                    Id = a.Id,
                    Ttitle = a.Ttitle,
                    Intro = a.Intro,
                    Endtime = a.Endtime,
                    Starttime = a.Starttime,
                    Status = a.Status,
                    ImgList = _partImageService.GetPartImageByCrowdId(a.Id),
                    Dislist = _discountService.GetDiscountByCrowdId(a.Id),
                });
                return PageHelper.toJson(new { list1 = list });
        }
        /// <summary>
        /// 添加新项目 po:现在暂时未实现添加图片
        /// </summary>
        /// <param name="crowdinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddCrowdInfo(CrowdModel crowdinfo)
        {
            if (crowdinfo != null)
            {
                var entity = new CrowdEntity
                {
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                    Adduser = crowdinfo.Adduser,
                    Upuser = crowdinfo.Upuser,
                    Ttitle = crowdinfo.Ttitle,
                    Intro = crowdinfo.Intro,
                    Starttime = crowdinfo.Starttime,
                    Endtime = crowdinfo.Endtime,
                    Status = crowdinfo.Status,
                    
                };
                try
                {
                    if (_crowdService.Create(entity) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }
        /// <summary>
        /// 修改项目信息
        /// </summary>
        /// <param name="crowdinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpCrowdInfo(CrowdModel crowdinfo)
        {
            if (crowdinfo != null)
            {
                var entity = new CrowdEntity
                {
                    Uptime = DateTime.Now,
                    Upuser = crowdinfo.Upuser,
                    Ttitle = crowdinfo.Ttitle,
                    Intro = crowdinfo.Intro,
                    Starttime = crowdinfo.Starttime,
                    Endtime = crowdinfo.Endtime,
                    Status = crowdinfo.Status,

                };
                try
                {
                    if (_crowdService.Update(entity) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据修改成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据修改失败！"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据修改错误！"));
        }

        /// <summary>
        /// 删除项目信息(传入项目ID即可)
        /// </summary>
        /// <param name="crowdinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteCrowdInfo(CrowdModel crowdinfo)
        {
            try
            {
                if (_crowdService.Delete(_crowdService.GetCrowdById(crowdinfo.Id)))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功！"));
                }
            }
            catch (Exception ex)
            {
                
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除失败！请检查参数"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除失败！请检查参数"));
        }

        #endregion
        #region 参加活动
        /// <summary>
        /// 添加参与人活动
        /// </summary>
        /// <param name="followerModel">参与人的信息</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddFollower(FollowerModel followerModel)
        {
            if (followerModel!=null)
            {
                var followerentity = new FollowerEntity
                {
                    Openid = followerModel.Openid,
                    Nickname = followerModel.Nickname,
                    Sex = followerModel.Sex,
                    City = followerModel.City,
                    Country = followerModel.Country,
                    Private = followerModel.Private,
                    Language = followerModel.Language,
                    Headimgurl = followerModel.Headimgurl,
                    Subscribetime = followerModel.Subscribe_time,
                    Unioid = followerModel.Unioid,
                    Remark = followerModel.Remark,
                    Groupid = followerModel.Groupid,
                    Adduser = followerModel.Adduser,
                    Addtime = DateTime.Now,
                    Upuser = followerModel.Upuser,
                    Uptime = DateTime.Now
                };
                var phoneentity = new PhoneEntity
                {
                    Follower = _followerService.Create(followerentity),
                    Openid = followerModel.Openid,
                    Phone = followerModel.Phone,
                    Adduser = followerModel.Adduser,
                    Addtime = DateTime.Now,
                    Upuser = followerModel.Upuser,
                    Uptime = DateTime.Now
                };
                try
                {
                    _phoneService.Create(phoneentity);
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            
        }
        #endregion
    }
}
