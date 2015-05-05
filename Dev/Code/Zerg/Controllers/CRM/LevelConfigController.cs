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


        #region 等级配置  李洪亮 2015 4 28



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
        public HttpResponseMessage AddLevelconfig([FromBody] LevelConfigEntity  LevelConfig )
        {

            if (!string.IsNullOrEmpty(LevelConfig.Name) && !string.IsNullOrEmpty(LevelConfig.Describe) && !string.IsNullOrEmpty(LevelConfig.Value))
            {
                var levelconfigModel = new LevelConfigEntity
                {
                    Name = LevelConfig.Name,
                    Describe = LevelConfig.Describe,
                    Value=LevelConfig.Value,
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
        public HttpResponseMessage UpdateLevelconfig([FromBody] LevelConfigEntity  LevelConfig )
        {
            if (LevelConfig != null && !string.IsNullOrEmpty(LevelConfig.Id.ToString()) && PageHelper.ValidateNumber(LevelConfig.Id.ToString()) && !string.IsNullOrEmpty(LevelConfig.Name) && !string.IsNullOrEmpty(LevelConfig.Describe) && !string.IsNullOrEmpty(LevelConfig.Value))
            {
                var levelconfigModel = _levelconfigService.GetLevelConfigById(LevelConfig.Id);
              
                levelconfigModel.Name = LevelConfig.Name;
                levelconfigModel.Describe =LevelConfig.Describe;
                levelconfigModel.Value = LevelConfig.Value;
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
        public HttpResponseMessage DeleteLevelConfig([FromBody] string id)
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
