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
