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
using CRM.Service.LevelConfig;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// 等级配置设置
    /// </summary>
    public class LevelConfigController : ApiController
    {

         private readonly ILevelConfigService _levelconfigService;

        public LevelConfigController(ILevelConfigService levelconfigService)
        {
            _levelconfigService = levelconfigService;
        }


        #region 等级明细  李洪亮 2015 4 28



        [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchLevelConfig(string pageindex)
        {
            var leconfigSearchCon = new LevelConfigSearchCondition
            {
                OrderBy = EnumLevelConfigSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_levelconfigService.GetLevelConfigsByCondition(leconfigSearchCon).ToPagedList(Convert.ToInt32(pageindex) + 1, 10).ToList());
        }

        /// <summary>
        /// 新增等级设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddLevelconfig(string name, string desc, string val)
        {

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(desc) && !string.IsNullOrEmpty(val))
            {
                var levelconfigModel = new LevelConfigEntity
                {
                    Name = name,
                    Describe = desc,
                    Value=val,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,

                };

                try
                {
                    if (_levelconfigService.Create(levelconfigModel) != null)
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
        /// 修改等级设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateLevelconfig(string id, string name, string desc, string val)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(desc) && !string.IsNullOrEmpty(val))
            {
                var levelconfigModel = _levelconfigService.GetLevelConfigById(Convert.ToInt32(id));
              
                levelconfigModel.Name = name;
                levelconfigModel.Describe = desc;
                levelconfigModel.Value = val;
                levelconfigModel.Uptime = DateTime.Now;
                try
                {
                    if (_levelconfigService.Update(levelconfigModel) != null)
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
        /// 删除等级设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteLevelConfig(string id)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                if (_levelconfigService.Delete(_levelconfigService.GetLevelConfigById(Convert.ToInt32(id))))
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
