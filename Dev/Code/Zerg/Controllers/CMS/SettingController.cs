using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CMS.Entity.Model;
using CMS.Service.Setting;
using Zerg.Common;
using Zerg.Models.CMS;
using System.ComponentModel;
namespace Zerg.Controllers.CMS
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("设置管理类")]
    public class SettingController : ApiController
    {
        private readonly ISettingService _settingService;
        /// <summary>
        /// 设置管理初始化
        /// </summary>
        /// <param name="settingService">settingService</param>
        /// 
        [Description("设置管理构造器")]
        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        /// <summary>
        /// 设置管理首页,根据页面数量设置,返回设置列表
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面记录</param>
        /// <returns>设置列表</returns>
        [HttpGet]
        [Description("设置管理首页,返回设置列表")]
        public HttpResponseMessage Index(string key = null, int page = 1, int pageSize = 10)
        {
            var settingCon = new SettingSearchCondition
            {
                Key = key,
                Page = page,
                PageCount = pageSize
            };
            var settingList = _settingService.GetSettingsByCondition(settingCon).Select(a => new SettingModel
            {
                Key = a.Key,
                Value = a.Value
            }).ToList();
            return PageHelper.toJson(settingList);
        }
        /// <summary>
        /// 传入设置Id,查询设置详情,返回设置详细信息
        /// </summary>
        /// <param name="id">设置ID</param>
        /// <returns>设置详细信息</returns>
        [HttpGet]
        [Description("检索返回设置详细信息")]
        public HttpResponseMessage Detailed(int id)
        {
            var setting = _settingService.GetSettingById(id);
            if (setting == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "该数据不存在"));
            }
            var settingDetail = new SettingModel
            {
                Key = setting.Key,
                Value = setting.Value
            };
            return PageHelper.toJson(settingDetail);
        }
        /// <summary>
        /// 传入设置参数,编辑修改设置节点,返回修改设置节点结果状态信息,成功提示"数据更新成功",失败提示"数据更新失败"
        /// </summary>
        /// <param name="model">设置节点参数</param>
        /// <returns>编辑修改设置节点结果状态信息</returns>
        [HttpPost]
        [Description("编辑修改设置节点信息")]
        public HttpResponseMessage Edit(List<SettingModel> model)
        {
            /*if (string.IsNullOrEmpty(model.Value)||string.IsNullOrEmpty(model.Value))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"数据不允许为空！"));
            }
            var createNew = false;
            var setting = _settingService.GetSettingById(model.Id) ?? _settingService.GetSettingByKey(model.Key);
            if (setting == null)
            {
                setting = new SettingEntity();
                createNew = true;
            }

            setting.Key = model.Key;
            setting.Value = model.Value;
            if (createNew)
            {
                if (_settingService.Create(setting).Id > 0)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功"));
                }
            }
            else
            {
                if (_settingService.Update(setting) != null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功"));
                }
            }*/
            var settings = new List<SettingEntity>();
            foreach (var setting in model)
            {
                //Todo:效率过低，建议一次查询
                var newSet = _settingService.GetSettingByKey(setting.Key) ?? new SettingEntity { Key = setting.Key };
                newSet.Value = setting.Value;
                settings.Add(newSet);
            }
            if (_settingService.CreateOrUpdateEntity(settings.ToArray()))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败"));
        }
        /// <summary>
        /// 传入设置节点参数,创建设置节点,返回创建设置节点结果状态,成功提示"数据添加成功",失败提示"数据添加失败"
        /// </summary>
        /// <param name="model">设置节点参数</param>
        /// <returns>创建设置节点结果状态信息</returns>
        /// 
        [Description("创建设置节点")]
        [HttpPost]
        public HttpResponseMessage Create(SettingModel model)
        {
            if (string.IsNullOrEmpty(model.Value))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不允许为空！"));
            }
            var setting = new SettingEntity
            {
                Key = model.Key,
                Value = model.Value
            };
            if (_settingService.Create(setting) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
            }
        }

        /// <summary>
        /// 传入设置节点Id,删除设置节点,返回删除结果状态信息,成功提示"数据删除成功",失败提示"数据删除失败"
        /// </summary>
        /// <param name="id">设置节点Id</param>
        /// <returns>删除设置节点结果状态信息</returns>
        [HttpPost]
        [Description("删除设置节点")]
        public HttpResponseMessage Delete(int id)
        {
            var setting = _settingService.GetSettingById(id);
            if (_settingService.Delete(setting))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败"));
            }
        }
    }
}
