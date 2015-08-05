using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.LevelConfig;
using Zerg.Common;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 等级配置设置
    /// </summary>
    [Description("等级配置类")]
    public class LevelConfigController : ApiController
    {

        private readonly ILevelConfigService _levelconfigService;
        /// <summary>
        /// 等级配置初始化
        /// </summary>
        /// <param name="levelconfigService">levelconfigService</param>
        public LevelConfigController(ILevelConfigService levelconfigService)
        {
            _levelconfigService = levelconfigService;
        }


        #region 等级配置  李洪亮 2015 4 28


        /// <summary>
        /// 传入等级配置参数，获取等级配置信息，返回等级配置信息
        /// </summary>
        /// <param name="name">等级配置参数</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>等级配置信息</returns>

        [Description("检索返回等级配置信息")]
        [HttpGet]
        public HttpResponseMessage SearchLevelConfig(EnumLevelConfigSearchOrderBy orderByAll = EnumLevelConfigSearchOrderBy.OrderById, bool isDes = true, string name = null, int page = 1, int pageSize = 10)
        {
            var leconfigSearchCon = new LevelConfigSearchCondition
            {
                Name = name,
                Page = Convert.ToInt32(page),
                PageCount = pageSize,
                OrderBy =orderByAll ,
                isDescending = isDes
            };
            var levelconfigList = _levelconfigService.GetLevelConfigsByCondition(leconfigSearchCon).Select(
                p => new
                {
                    p.Id,
                    p.Name,
                    p.Describe,
                    p.Value,
                    p.Addtime
                }
                ).ToList();
            var levelconfigListCount = _levelconfigService.GetLevelConfigCount(leconfigSearchCon);
            return PageHelper.toJson(new { List = levelconfigList, Condition = leconfigSearchCon, totalCount = levelconfigListCount });

        }
        /// <summary>
        /// 传入等级配置ID，检索等级配置信息，返回等级配置信息。
        /// </summary>
        /// <param name="id">等级配置ID</param>
        /// <returns>等级配置信息</returns>

        [Description("检索返回等级配置信息")]
        [HttpGet]
        public HttpResponseMessage GetLevelConfig(string id)
        {
            var LevelConfig = _levelconfigService.GetLevelConfigById(Convert.ToInt32(id));
            return PageHelper.toJson(LevelConfig);

        }

        /// <summary>
        /// 传入等级配置参数，新增等级设置，返回等级配置结果状态信息
        /// </summary>
        /// <param name="name">等级参数</param>
        /// <returns>新增等级配置结果状态信息</returns>

        [Description("增加等级配置")]
        [HttpPost]
        public HttpResponseMessage DoCreate([FromBody] LevelConfigEntity LevelConfig)
        {

            if (!string.IsNullOrEmpty(LevelConfig.Name) && !string.IsNullOrEmpty(LevelConfig.Describe) && !string.IsNullOrEmpty(LevelConfig.Value))
            {
                var levelconfigModel = new LevelConfigEntity
                {
                    Name = LevelConfig.Name,
                    Describe = LevelConfig.Describe,
                    Value = LevelConfig.Value,
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
        /// 传入等级配置参数，修改等级设置，返回修改结果状态信息
        /// </summary>
        /// <param name="LevelConfig">等级配置参数</param>
        /// <returns>修改等级配置结果状态信息</returns>

        [Description("修改等级配置")]
        [HttpPost]
        public HttpResponseMessage DoEdit([FromBody] LevelConfigEntity LevelConfig)
        {
            if (LevelConfig != null && !string.IsNullOrEmpty(LevelConfig.Id.ToString()) && PageHelper.ValidateNumber(LevelConfig.Id.ToString()) && !string.IsNullOrEmpty(LevelConfig.Name) && !string.IsNullOrEmpty(LevelConfig.Describe) && !string.IsNullOrEmpty(LevelConfig.Value))
            {
                var levelconfigModel = _levelconfigService.GetLevelConfigById(LevelConfig.Id);

                levelconfigModel.Name = LevelConfig.Name;
                levelconfigModel.Describe = LevelConfig.Describe;
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
        /// 传入等级配置ID，删除等级设置，返回删除结果状态信息
        /// </summary>
        /// <param name="id">等级配置ID</param>
        /// <returns>删除等级配置结果状态信息</returns>

        [Description("删除等级配置")]
        [HttpPost]
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
