using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CRM.Entity.Model;
using CRM.Service.Task;
using CRM.Service.TaskType;
using Zerg.Models.CRM;

namespace Zerg.Controllers
{

    /// <summary>
    /// CRM  管理员
    /// </summary>
    public class CRMAdminController : ApiController
    {
        private readonly ITaskService _taskService;
        private readonly ITaskTypeService _taskTypeService;

        public CRMAdminController(ITaskService taskService,
            ITaskTypeService taskTypeService)
        {
            _taskService = taskService;
            _taskTypeService = taskTypeService;
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
    }
}
