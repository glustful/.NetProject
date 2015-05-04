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
        public HttpResponseMessage SearchLevel([FromBody] string pageindex)
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
        public HttpResponseMessage AddLevel([FromBody] LevelEntity Level)
        {

            if (!string.IsNullOrEmpty(Level.Name) && !string.IsNullOrEmpty(Level.Describe) && !string.IsNullOrEmpty(Level.Url))
            {
                var levelModel = new LevelEntity
                {
                    Name = Level.Name,
                    Describe = Level.Describe,
                    Url = Level.Url,
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
        public HttpResponseMessage UpdateLevel([FromBody] LevelEntity Level)
        {
            if (Level != null && !string.IsNullOrEmpty(Level.Id.ToString()) && PageHelper.ValidateNumber(Level.Id.ToString()) && !string.IsNullOrEmpty(Level.Name) && !string.IsNullOrEmpty(Level.Describe) && !string.IsNullOrEmpty(Level.Url))
            {
                var levelModel = _levelService.GetLevelById(Level.Id);
                levelModel.Uptime = DateTime.Now;
                levelModel.Name = Level.Name;
                levelModel.Describe = Level.Describe;
                levelModel.Url = Level.Url;

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
        public HttpResponseMessage DeleteLevel([FromBody] string id)
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
