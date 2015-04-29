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

        public CRMAdminController(ITaskService taskService, ITaskTypeService taskTypeService,ILevelService levelService)
        {
            _taskService = taskService;
            _taskTypeService = taskTypeService;
            _levelService=levelService;
        }

        #region 任务配置 杨定鹏 2015年4月28日10:04:08

        /// <summary>
        /// 返回任务列表
        /// </summary>
        /// <param name="id">分页ID</param>
        /// <param name="typeid">任务类型</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<TaskModel> TaskList(int id=1,int typeid=0)
        {
            var condition = new TaskSearchCondition();
            var type = _taskTypeService.GetTaskTypeById(typeid);
            var listCount = _taskService.GetTasksByCondition(condition).ToList().Where(p => p.TaskType == type).Select(a=>new TaskModel
            {
                Id=a.Id,
                TaskPunishment = a.TaskPunishment.Id,
                TaskAward = a.TaskAward.Id,
                TaskTag = a.TaskTag.Id,
                TaskType = a.TaskType.Id,
                Taskname = a.Taskname,
                Describe = a.Describe,
                Endtime = a.Endtime,
                Adduser = a.Adduser,
                Addtime = a.Addtime,
                Upuser = a.Upuser,
                Uptime = a.Uptime
            }).ToList();

            return listCount;
        }

        #region 任务类型 杨定鹏 2015年4月28日17:13:12

        /// <summary>
        /// 添加任务类型表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="describe"></param>
        /// <returns></returns>
        //public AccsessModel AddTaskTpye(string name, string describe)
        //{
        //    if (!string.IsNullOrWhiteSpace(name))
        //    {
        //        var model = new TaskTypeEntity()
        //        {
        //            Name = name,
        //            Describe = describe
        //        };
        //        _taskTypeService.Create(model);
        //    }
        //    else
        //    {
        //        return new AccsessModel {status = "添加失败", msg = "类型名称不能为空"};;
        //    }

        //    return new AccsessModel { status = "添加成功", msg = "ok" }; ;
        //}

        ///// <summary>
        ///// 删除任务类型
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public AccsessModel DelTaskType(int id)
        //{
        //    _taskTypeService.Delete(_taskTypeService.GetTaskTypeById(id));
        //    return new AccsessModel { status = "添加成功", msg = "ok" }; ;
        //}

        public List<TaskTypeModel> TaskTypeList()
        {
            var condition = new TaskTypeSearchCondition();
            var list = _taskTypeService.GetTaskTypesByCondition(condition).Select(a => new TaskTypeModel(
                )).ToList();

            return null;
        }

        #endregion
        #endregion

        #region 等级  李洪亮 2015 4 28

        public HttpResponseMessage SearchLevel()
        {
            return PageHelper.toJson("");
        }

        /// <summary>
        /// 新增商家  
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpResponseMessage AddLevel(string name)
        {
            return PageHelper.toJson(name);
        }


        /// <summary>
        /// 修改等级
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateLevel(string name)
        {
            return PageHelper.toJson(" ");
        }


        /// <summary>
        /// 删除等级
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpResponseMessage DeleteLevel(string name)
        {
            return PageHelper.toJson(" ");
        }
        
        #endregion


    }
}