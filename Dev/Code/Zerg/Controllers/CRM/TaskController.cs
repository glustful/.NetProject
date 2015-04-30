using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CRM.Entity.Model;
using CRM.Service.Task;
using CRM.Service.TaskAward;
using CRM.Service.TaskPunishment;
using CRM.Service.TaskTag;
using CRM.Service.TaskType;
using Webdiyer.WebControls.Mvc;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// CRM 任务管理明细
    /// </summary>
    public class TaskController : ApiController
    {
        private readonly ITaskService _taskService;
        private readonly ITaskTypeService _taskTypeService;
        private readonly ITaskAwardService _taskAwardService;
        private readonly ITaskTagService _taskTagService;
        private readonly ITaskPunishmentService _taskPunishmentService;

        public TaskController(ITaskService taskService, 
            ITaskTypeService taskTypeService, 
            ITaskAwardService taskAwardService, 
            ITaskTagService taskTagService, 
            ITaskPunishmentService taskPunishmentService
            )
        {
            _taskService = taskService;
            _taskTypeService = taskTypeService;
            _taskAwardService = taskAwardService;
            _taskTagService = taskTagService;
            _taskPunishmentService = taskPunishmentService;
        }

        #region 单个任务配置 杨定鹏 2015年4月28日10:04:08

        /// <summary>
        /// 返回任务列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage TaskList(string pageindex)
        {
            var condition = new TaskSearchCondition
            {
                OrderBy = EnumTaskSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_taskService.GetTasksByCondition(condition).ToPagedList(Convert.ToInt32(pageindex) + 1, 10).ToList());
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddTsk(TaskModel taskModel, string type)
        {
            if (!string.IsNullOrEmpty(taskModel.Taskname))
            {
                var model = new TaskEntity
                {

                };
                if (type == "add")
                {
                    try
                    {
                        if (_taskService.Create(model) != null)
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功"));
                        }
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败"));
                    }
                }
                if (type == "edit")
                {
                    try
                    {
                        _taskService.Update(_taskService.GetTaskById(taskModel.Id));
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
                    }
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证失败"));
        }

        #endregion

        #region 任务类型 杨定鹏 2015年4月28日17:13:12
        /// <summary>
        /// 添加和修改任务类型表
        /// </summary>
        /// <param name="taskTypeModel">任务类型数据模型</param>
        /// <param name="type">操作状态，新增/修改</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddTaskTpye(TaskTypeModel taskTypeModel, string type)
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
                    try
                    {
                        if (_taskTypeService.Create(model) != null)
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功"));
                        }
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败"));
                    }
                }
                if (type == "edit")
                {
                    try
                    {
                        _taskTypeService.Update(_taskTypeService.GetTaskTypeById(taskTypeModel.Id));
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
                    }
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "类型名称不能为空！"));
        }

        /// <summary>
        /// 删除任务类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DelTaskType(int id)
        {
            try
            {
                _taskTypeService.Delete(_taskTypeService.GetTaskTypeById(id));
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }
        }

        #endregion

        #region 任务奖励 杨定鹏 2015年4月29日15:15:41
        /// <summary>
        /// 添加和修改任务奖励
        /// </summary>
        /// <param name="taskAwardModel">任务奖励数据模型</param>
        /// <param name="type">操作状态，新增/修改</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddTaskAward(TaskAwardModel taskAwardModel, string type)
        {
            if (!string.IsNullOrWhiteSpace(taskAwardModel.Name))
            {
                var model = new TaskAwardEntity()
                {
                    Name = taskAwardModel.Name,
                    Describe = taskAwardModel.Describe,
                    Value = taskAwardModel.Value
                };
                if (type == "add")
                {
                    try
                    {
                        _taskAwardService.Create(model);
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功"));
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败"));
                    }
                }
                if (type == "edit")
                {
                    try
                    {
                        _taskAwardService.Update(_taskAwardService.GetTaskAwardById(taskAwardModel.Id));
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
                    }
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "类型名称不能为空"));
        }

        /// <summary>
        /// 删除任务奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DelTaskAward(int id)
        {
            _taskAwardService.Delete(_taskAwardService.GetTaskAwardById(id));
            return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
        }

        #endregion

        #region 任务目标 杨定鹏 2015年4月29日15:57:37
        /// <summary>
        /// 添加和修改任务目标
        /// </summary>
        /// <param name="taskTagModel">任务目标数据模型</param>
        /// <param name="type">操作状态 新增/修改</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddTaskTag(TaskTagModel taskTagModel, string type)
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
                    try
                    {
                        _taskTagService.Create(model);
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功"));
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败"));
                    }
                }
                if (type == "edit")
                {
                    try
                    {
                        _taskTagService.Update(_taskTagService.GetTaskTagById(taskTagModel.Id));
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "操作成功"));
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败"));
                    }
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "类型名称不能为空"));
        }

        /// <summary>
        /// 删除任务奖励
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DelTaskTag(int id)
        {
            try
            {
                _taskTagService.Delete(_taskTagService.GetTaskTagById(id));
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }
        }
        #endregion

        #region 任务惩罚 杨定鹏 2015年4月29日16:21:10
        /// <summary>
        /// 添加和修改任务惩罚
        /// </summary>
        /// <param name="taskPunishmentModel"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddTaskPunishment(TaskPunishmentModel taskPunishmentModel, string type)
        {
            if (!string.IsNullOrWhiteSpace(taskPunishmentModel.Name))
            {
                var model = new TaskPunishmentEntity()
                {
                    Name = taskPunishmentModel.Name,
                    Describe = taskPunishmentModel.Describe,
                    Value = taskPunishmentModel.Value
                };
                if (type == "add")
                {
                    try
                    {
                        _taskPunishmentService.Create(model);
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功"));
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败"));
                    }
                }
                if (type == "edit")
                {
                    try
                    {
                        _taskPunishmentService.Update(_taskPunishmentService.GetTaskPunishmentById(taskPunishmentModel.Id));
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "修改失败"));
                    }
                    catch (Exception)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
                    }
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "类型名称不能为空"));
        }

        /// <summary>
        /// 删除任务惩罚
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DelTaskPunishment(int id)
        {
            try
            {
                _taskPunishmentService.Delete(_taskPunishmentService.GetTaskPunishmentById(id));
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }
        }

        #endregion
    }
}
