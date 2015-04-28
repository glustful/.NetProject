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
        public List<TaskModel> TaskList(int typeid)
        {
            //var list=new TaskSearchCondition();
            //var type=_taskTypeService.GetTaskTypeById(typeid);
            //var list1=_taskService.GetTasksByCondition(list).ToList().Where(p => p.TaskType == type);

            //TaskModel temp = (from t in list1 select t).ToList();

            return null;
        }

        #endregion
    }
}
