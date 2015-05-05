using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CMS.Entity.Model;
using CMS.Service.Setting;
using Zerg.Common;
using Zerg.Models.CMS;

namespace Zerg.Controllers.CMS
{
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
            var settingCon=new SettingSearchCondition{
               Key=key,
               Page=page,
               PageCount=pageSize
            };
            var settingList = _settingService.GetSettingsByCondition(settingCon).Select(a => new SettingModel { 
             Id=a.Id,
             Key=a.Key,
             Value=a.Value
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
                Id = setting.Id,
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
        public HttpResponseMessage Edit(SettingModel model)
        {
            var setting = _settingService.GetSettingById(model.Id);
            setting.Key = model.Key;
            setting.Value = model.Value;
            if (_settingService.Update(setting) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功"));
            }
            else 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"数据更新失败"));
            }
        }
        /// <summary>
        /// 新建设置
        /// </summary>
        /// <param name="model">设置参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Create(SettingModel model)
        {
            var setting = new SettingEntity
            {
                Key = model.Key,
                Value = model.Value
            };
            if (_settingService.Create(setting)!= null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            else {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
            }
        }

        /// <summary>
        /// 删除设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Delete(int id) {
            var setting = _settingService.GetSettingById(id);
            if (_settingService.Delete(setting))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
            }
            else {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败"));
            }
        }
    }
}
