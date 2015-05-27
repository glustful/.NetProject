using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CMS.Entity.Model;
using CMS.Service.Setting;
using Zerg.Common;
using Zerg.Models.CMS;

namespace Zerg.Controllers.CMS
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class SettingController : ApiController
    {
        private readonly ISettingService _settingService;
        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面记录</param>
        /// <returns></returns>
        [HttpGet]
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
        /// 设置详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
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
        /// 保存修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
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
            if(_settingService.CreateOrUpdateEntity(settings.ToArray()))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败"));
        }
        /// <summary>
        /// 新建设置
        /// </summary>
        /// <param name="model">设置参数</param>
        /// <returns></returns>
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
        /// 删除设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
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
