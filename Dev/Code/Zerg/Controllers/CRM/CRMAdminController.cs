using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CRM.Entity.Model;
using CRM.Service.Task;
using CRM.Service.TaskType;
using Zerg.Models.CRM;
using System.Net.Http;
using System;
using System.Web.Script.Serialization;
using Zerg.Models;
using CRM.Service.Level;
using Zerg.Common;
using Webdiyer.WebControls.Mvc;
namespace Zerg.Controllers
{

    /// <summary>
    /// CRM  管理员
    /// </summary>
    public class CRMAdminController : ApiController
    {
        private readonly ITaskService _taskService;
        private readonly ITaskTypeService _taskTypeService;
        private readonly ILevelService _levelService;

        public CRMAdminController(ITaskService taskService, ITaskTypeService taskTypeService, ILevelService levelService)
        {
            _taskService = taskService;
            _taskTypeService = taskTypeService;
            _levelService = levelService;
        }

        #region 任务配置 杨定鹏 2015年4月28日10:04:08

        /// <summary>
        /// 返回任务列表z
        /// </summary>
        /// <param name="id">分页ID</param>
        /// <param name="typeid">任务类型</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<TaskModel> TaskList(int typeid)
        {
            var condition = new TaskSearchCondition();
            var type = _taskTypeService.GetTaskTypeById(typeid);
            var listCount = _taskService.GetTasksByCondition(condition).ToList().Where(p => p.TaskType == type);
            //var model=new TaskModel
            //{
            //    Id=
            //}

            return null;
        }

        #endregion


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
