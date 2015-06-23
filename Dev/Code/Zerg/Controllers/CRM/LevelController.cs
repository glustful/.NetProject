using CRM.Entity.Model;
using CRM.Service.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zerg.Common;
using System.Web.Http.Cors;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
   [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 等级设置
    /// </summary>
   [Description("等级设置类")]
    public class LevelController : ApiController
    {
        private readonly ILevelService _levelService;
        /// <summary>
        /// 等级设置初始化
        /// </summary>
        /// <param name="levelService">levelService</param>
        public LevelController(ILevelService levelService)
        {
            _levelService = levelService;
        }



        #region 等级明细  李洪亮 2015 4 28


        /// <summary>
        /// 传入等级名称，检索等级信息，返回等级信息
        /// </summary>
        /// <param name="name">等级名称</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>等级信息</returns>
       
        [Description("检索返回等级信息")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchLevel(string name, int page = 1, int pageSize = 10)
        {
            var leSearchCon = new LevelSearchCondition
            {   
                Name=name,
                Page =page,
                PageCount = pageSize,
                OrderBy = EnumLevelSearchOrderBy.OrderById,
                isDescending = true
            };
            var levelList = _levelService.GetLevelsByCondition(leSearchCon).ToList();

            var levelListCount = _levelService.GetLevelCount(leSearchCon);
            return PageHelper.toJson(new { List = levelList, Condition = leSearchCon, totalCount = levelListCount });
          
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetLevel(string id)
        {
           var Level=_levelService.GetLevelById(Convert.ToInt32(id));         
            return PageHelper.toJson(Level);

        }



        /// <summary>
        /// 传入等级参数，新增等级，返回新增等级结果状态信息  
        /// </summary>
        /// <param name="Level">等级参数</param>
        /// <returns>返回新增等级结果状态信息</returns>
         
        [Description("新增等级信息")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DoCreate( LevelEntity Level)
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
        /// 传入等级参数，修改等级，返回修改等级结果状态信息  
        /// </summary>
        /// <param name="Level">等级参数</param>
        /// <returns>修改等级结果状态信息 </returns>
         
        [Description("修改等级信息")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DoEdit([FromBody] LevelEntity Level)
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
        ///传入等级ID，删除等级，返回删除等级结果状态信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns><删除等级结果状态信息 /returns>
        [Description("删除等级")]
         
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
