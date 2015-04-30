using CRM.Entity.Model;
using CRM.Service.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zerg.Common;
using Webdiyer.WebControls.Mvc;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// 等级设置
    /// </summary>
    public class LevelController : ApiController
    {
        private readonly ILevelService _levelService;

        public LevelController(ILevelService levelService)
        {
            _levelService = levelService;
        }



        #region 等级明细  李洪亮 2015 4 28



        [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchLevel(string pageindex)
        {
            var leSearchCon = new LevelSearchCondition
            {
                OrderBy = EnumLevelSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_levelService.GetLevelsByCondition(leSearchCon).ToPagedList(Convert.ToInt32(pageindex) + 1, 10).ToList());
        }

        /// <summary>
        /// 新增等级  
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddLevel(string name, string desc, string imgurl)
        {

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(desc) && !string.IsNullOrEmpty(imgurl))
            {
                var levelModel = new LevelEntity
                {
                    Name = name,
                    Describe = desc,
                    Url = imgurl,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,

                };

                try
                {
                    if (_levelService.Create(levelModel) != null)
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
        /// 修改等级
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateLevel(string id, string name, string desc, string imgurl)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(desc) && !string.IsNullOrEmpty(imgurl))
            {
                var levelModel = _levelService.GetLevelById(Convert.ToInt32(id));
                levelModel.Uptime = DateTime.Now;
                levelModel.Name = name;
                levelModel.Describe = desc;
                levelModel.Url = imgurl;

                try
                {
                    if (_levelService.Update(levelModel) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                }


            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));

        }


        /// <summary>
        /// 删除等级
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteLevel(string id)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                if (_levelService.Delete(_levelService.GetLevelById(Convert.ToInt32(id))))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据成功删除！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
                }
            }

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }

        #endregion



    }
}
