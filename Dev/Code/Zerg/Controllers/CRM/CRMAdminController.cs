using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CRM.Entity.Model;
using CRM.Service.Task;
using CRM.Service.TaskAward;
using CRM.Service.TaskPunishment;
using CRM.Service.TaskTag;
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
        private readonly ITaskAwardService _taskAwardService;
        private readonly ITaskTagService _taskTagService;
        private readonly ITaskPunishmentService _taskPunishmentService;

        public CRMAdminController(ITaskService taskService, 
            ITaskTypeService taskTypeService,
            ILevelService levelService,
            ITaskAwardService taskAwardService,
            ITaskTagService taskTagService,
            ITaskPunishmentService taskPunishmentService)
        {
            _taskService = taskService;
            _taskTypeService = taskTypeService;
            _levelService=levelService;
            _taskAwardService = taskAwardService;
            _taskTagService = taskTagService;
            _taskPunishmentService = taskPunishmentService;
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
        /// 添加和修改任务类型表
        /// </summary>
        /// <param name="taskTypeModel">任务类型数据模型</param>
        /// <param name="type">操作状态，新增/修改</param>
        /// <returns></returns>
        public ResultModel AddTaskTpye(TaskTypeModel taskTypeModel,string type)
        {
            if (!string.IsNullOrWhiteSpace(taskTypeModel.Name))
            {
                var model = new TaskTypeEntity()
                {
                    Name = taskTypeModel.Name,
                    Describe = taskTypeModel.Describe
                };
                if (type == "add")
                {
                    _taskTypeService.Create(model);
                }
                if (type == "edit")
                {
                    _taskTypeService.Update(_taskTypeService.GetTaskTypeById(taskTypeModel.Id));
                }
            }
            else
            {
                return new ResultModel { Status = false, Msg = "类型名称不能为空" }; ;
            }
            return new ResultModel { Status = true, Msg = "操作成功" }; ;
        }

        /// <summary>
        /// 删除任务类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel DelTaskType(int id)
        {
            _taskTypeService.Delete(_taskTypeService.GetTaskTypeById(id));
            return new ResultModel { Status = true, Msg = "删除成功" }; ;
        }

        #endregion

        #region 任务奖励 杨定鹏 2015年4月29日15:15:41
        /// <summary>
        /// 添加和修改任务奖励
        /// </summary>
        /// <param name="taskAwardModel">任务奖励数据模型</param>
        /// <param name="type">操作状态，新增/修改</param>
        /// <returns></returns>
        public ResultModel AddTaskAward(TaskAwardModel taskAwardModel, string type)
        {
            if (!string.IsNullOrWhiteSpace(taskAwardModel.Name))
            {
                var model = new TaskAwardEntity()
                {
                    Name = taskAwardModel.Name,
                    Describe = taskAwardModel.Describe,
                    Value=taskAwardModel.Value
                };
                if (type == "add")
                {
                    _taskAwardService.Create(model);
                }
                if (type == "edit")
                {
                    _taskAwardService.Update(_taskAwardService.GetTaskAwardById(taskAwardModel.Id));
                }
            }
            else
            {
                return new ResultModel { Status = false, Msg = "类型名称不能为空" }; ;
            }
            return new ResultModel { Status = true, Msg = "操作成功" }; ;
        }

        /// <summary>
        /// 删除任务奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel DelTaskAward(int id)
        {
            _taskAwardService.Delete(_taskAwardService.GetTaskAwardById(id));
            return new ResultModel{Status = true,Msg = "删除成功"};
        }

        #endregion

        #region 任务目标 杨定鹏 2015年4月29日15:57:37
        /// <summary>
        /// 添加和修改任务目标
        /// </summary>
        /// <param name="taskTagModel">任务目标数据模型</param>
        /// <param name="type">操作状态 新增/修改</param>
        /// <returns></returns>
        public ResultModel AddTaskTag(TaskTagModel taskTagModel,string type)
        {
            if (!string.IsNullOrWhiteSpace(taskTagModel.Name))
            {
                var model = new TaskTagEntity()
                {
                    Name = taskTagModel.Name,
                    Describe = taskTagModel.Describe,
                    Value = taskTagModel.Value
                };
                if (type == "add")
                {
                    _taskTagService.Create(model);
                }
                if (type == "edit")
                {
                    _taskTagService.Update(_taskTagService.GetTaskTagById(taskTagModel.Id));
                }
            }
            else
            {
                return new ResultModel { Status = false, Msg = "类型名称不能为空" };
            }
            return new ResultModel { Status = true, Msg = "操作成功" };
        }

        /// <summary>
        /// 删除任务奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel DelTaskTag(int id)
        {
            _taskTagService.Delete(_taskTagService.GetTaskTagById(id));
            return new ResultModel { Status = true, Msg = "删除成功" };
        }
        #endregion

        #region 任务惩罚 杨定鹏 2015年4月29日16:21:10
        /// <summary>
        /// 添加和修改任务惩罚
        /// </summary>
        /// <param name="taskPunishmentModel"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //public ResultModel AddTaskPunishment(TaskPunishmentModel taskPunishmentModel, string type)
        //{
        //    if (!string.IsNullOrWhiteSpace(taskPunishmentModel.Name))
        //    {
        //        var model = new TaskPunishmentEntity()
        //        {
        //            Name = taskPunishmentModel.Name,
        //            Describe = taskPunishmentModel.Describe,
        //            Value = taskPunishmentModel.Value
        //        };
        //        if (type == "add")
        //        {
        //            _.Create(model);
        //        }
        //        if (type == "edit")
        //        {
        //            _taskTagService.Update(_taskTagService.GetTaskTagById(taskTagModel.Id));
        //        }
        //    }
        //    else
        //    {
        //        return new ResultModel { Status = false, Msg = "类型名称不能为空" };
        //    }
        //    return new ResultModel { Status = true, Msg = "操作成功" };
        //}
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