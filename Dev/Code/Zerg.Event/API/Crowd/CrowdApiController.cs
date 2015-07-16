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
        #region 项目信息 黄秀宇2015.07.14
        /// <summary>
        /// 查询众筹表里的所有信息mobile端
        /// </summary>
        /// <param name="status">众筹状态，-1所有，1众筹中，0选房中</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCrowdInfo(int status = -1)
        {

            //查询项目表所有的数据
            var sech = new CrowdSearchCondition
            {
                OrderBy = EnumCrowdSearchOrderBy.OrderById,
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
                p.Addtime,
                p.crowdUrl
            }).ToList().Select(a => new
            {
                //返回数据
                Id = a.Id,
                Ttitle = a.Ttitle,
                Intro = a.Intro,
                Endtime = a.Endtime,
                Starttime = a.Starttime,
                Status = a.Status,
                ImgList = _partImageService.GetPartImageByCrowdId(a.Id),//查询众筹对应的图片
                // Dislist = _discountService.GetDiscountByCrowdId(a.Id),//查询众筹人数
                ////已参与众筹人数
                //crowdNum = _participationService.GetParticipationCountByCrowdId(a.Id),
                ////众筹最低优惠对应的人数
                //crowdMaxNum = _discountService.GetDiscountMaxCountByCrowdId(a.Id),
                crowdUrl = a.crowdUrl,
                //Addtime=a.Addtime.ToString("yyyy-MM-dd") ,
                Addtime = a.Addtime.ToString("yyyy-MM-dd"),
                i = 0
            });
            //.Where(p => p.crowdMaxNum != 0);
            return PageHelper.toJson(new { list });
        }
        #endregion
        #region 查询项目表里的所有信息pc端 黄秀宇 2015.07.14
        /// <summary>
        /// 查询项目表里的所有信息pc端
        /// </summary>
        /// <param name="status">项目状态，-1为所有，1为众筹中，0位选房中</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCrowdInfoPC(int status = -1)
        {

            //查询项目表所有的数据
            var sech = new CrowdSearchCondition
            {
                OrderBy = EnumCrowdSearchOrderBy.OrderById,
                Statuss = status
            };
            var list = _crowdService.GetCrowdsByCondition(sech).Select(p => new
            {

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
            }).ToList().Select(a => new
            {
                Id = a.Id,
                Ttitle = a.Ttitle,
                Intro = a.Intro,
                Endtime = a.Endtime,
                Starttime = a.Starttime,
                Status = a.Status,
                //ImgList = _partImageService.GetPartImageByCrowdId(a.Id),
                //Dislist = _discountService.GetDiscountByCrowdId(a.Id),
                ////已参与众筹人数
                //crowdNum = _participationService.GetParticipationCountByCrowdId(a.Id),
                ////众筹最低优惠对应的人数
                //crowdMaxNum = _discountService.GetDiscountMaxCountByCrowdId(a.Id),
                crowdUrl = a.crowdUrl,
                Addtime = a.Addtime.ToString("yyyy-MM-dd"),
                i = 0
            });
            //.Where(p => p.crowdMaxNum != 0);
            return PageHelper.toJson(new { list });
        }
        #endregion
        #region 根据众筹id来查询众筹 黄秀宇 2015.07.14
        /// <summary>
        /// 根据众筹id来查询众筹
        /// </summary>
        /// <param name="id">众筹id</param>
        /// <param name="status">状态，-1代表所有，1众筹中，0选房中</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCrowdByCrowdId(int id, int status = -1)
        {
            var sech = new CrowdSearchCondition
            {
                Id = id,
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
                p.Addtime,
                p.crowdUrl
            }).ToList().Select(a => new
            {
                //返回数据
                Id = a.Id,
                Ttitle = a.Ttitle,
                Intro = a.Intro,
                Endtime = a.Endtime,
                Starttime = a.Starttime,
                Status = a.Status,
                ImgList = _partImageService.GetPartImageByCrowdId(a.Id),
                //Dislist = _discountService.GetDiscountByCrowdId(a.Id),
                //已参与众筹人数
                //  crowdNum = _participationService.GetParticipationCountByCrowdId(a.Id),
                //众筹最低优惠对应的人数
                //  crowdMaxNum = _discountService.GetDiscountMaxCountByCrowdId(a.Id),
                crowdUrl = a.crowdUrl,
                //Addtime=a.Addtime.ToString("yyyy-MM-dd") ,
                Addtime = a.Addtime.ToString("yyyy-MM-dd"),
                i = 0
            });
            return PageHelper.toJson(new { list });
        }
        #endregion
        #region 添加和修改众筹 黄秀宇 2015.07.14
        /// <summary>
        /// 添加和修改众筹
        /// </summary>
        /// <param name="crowdModel">crowdModel.Ttitle众筹标题，crowdModel.Intro众筹描述，crowdModel .crowdUrl众筹地址</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddCrowdInfo([FromBody]CrowdModel crowdModel)
        {
            if (crowdModel != null)
            {
                var entity = new CrowdEntity
                {
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                    Adduser = 1,
                    Upuser = 1,
                    Ttitle = crowdModel.Ttitle,
                    Intro = crowdModel.Intro,
                    Starttime = DateTime.Now,
                    Endtime = DateTime.Now,
                    Status = 1,
                    crowdUrl = crowdModel.crowdUrl
                };
                var entityImg = new PartImageEntity
                {
                    Orderby = 1,
                    Imgurl = "",
                    Adduser = 2,
                    Addtime = DateTime.Now,
                    Uptime = DateTime.Now,
                    Upuser = 2,
                    Crowd = null
                };
                try
                {
                    int id;
                    if (crowdModel.Id > 0) //修改众筹
                    {
                        id = crowdModel.Id;
                        entity = _crowdService.GetCrowdById(id);
                        entity.Intro = crowdModel.Intro;
                        entity.Ttitle = crowdModel.Ttitle;
                        entity.crowdUrl = crowdModel.crowdUrl;
                        _crowdService.Update(entity);//更新众筹表crowd
                        var enImg = new PartImageSearchCondition
                        {
                            OrderBy = EnumPartImageSearchOrderBy.OrderById,
                            CrowdId = id
                        };
                        var model = _partImageService.GetPartImagesByCondition(enImg);//根据众筹id查询众筹图片
                        foreach (var p in model)
                        {
                            _partImageService.Delete(p);//删除众筹图片
                        }
                        if (crowdModel.ImgList1.Count > 0)
                        {
                            entityImg.Crowd = _crowdService.GetCrowdById(id);
                            for (int i = 0; i < crowdModel.ImgList1.Count; i++)//添加众筹图片
                            {
                                entityImg.Imgurl = crowdModel.ImgList1[i];

                                _partImageService.Create(entityImg);
                            }

                        }
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据保存成功！"));

                    }
                    else//新增众筹
                    {
                        id = _crowdService.Create(entity).Id;
                        //插入众筹图片
                        if (crowdModel.ImgList1.Count > 0)
                        {
                            entityImg.Crowd = _crowdService.GetCrowdById(id);
                            for (int i = 0; i < crowdModel.ImgList1.Count; i++)
                            {
                                entityImg.Imgurl = crowdModel.ImgList1[i];

                                _partImageService.Create(entityImg);
                            }

                        }
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
        #endregion
        /// <summary>
        /// 修改项目信息
        /// </summary>
        /// <param name="crowdinfo"></param>
        /// <returns></returns>
        //[HttpPost]
        //public HttpResponseMessage UpCrowdInfo(CrowdModel crowdinfo)
        //{
        //    if (crowdinfo != null)
        //    {
        //        var entity = new CrowdEntity
        //        {
        //            Uptime = DateTime.Now,
        //            Upuser = crowdinfo.Upuser,
        //            Ttitle = crowdinfo.Ttitle,
        //            Intro = crowdinfo.Intro,
        //            Starttime = crowdinfo.Starttime,
        //            Endtime = crowdinfo.Endtime,
        //            Status = crowdinfo.Status,

        //        };
        //        try
        //        {
        //            if (_crowdService.Update(entity) != null)
        //            {
        //                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据修改成功！"));
        //            }
        //        }
        //        catch
        //        {
        //            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据修改失败！"));
        //        }
        //    }
        //    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据修改错误！"));
        //}
        #region 删除众筹 黄秀宇 2015.07.14
        /// <summary>
        /// 删除众筹
        /// </summary>
        /// <param name="id">众筹id</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteCrowdInfo(int id)
        {
            try
            {
                var model = _partImageService.GetPartImageByCrowdId(id);
                foreach (var p in model)
                {
                    _partImageService.Delete(p);//删除众筹图片
                }
                if (_crowdService.Delete(_crowdService.GetCrowdById(id)))//删除众筹信息
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功！"));
                }
            }
            catch (Exception ex)
            {

                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！请检查参数"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！请检查参数"));
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
            if (followerModel != null)
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
