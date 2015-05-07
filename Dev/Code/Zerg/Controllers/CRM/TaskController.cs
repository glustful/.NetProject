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
        /// <param name="taskSearchModel"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage TaskList([FromBody]TaskSearchModel taskSearchModel)
        {
            var condition = new TaskSearchCondition
            {
                OrderBy = EnumTaskSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_taskService.GetTasksByCondition(condition).ToList());
        }

        /// <summary>
        /// 添加和修改单个任务
        /// </summary>
        /// <param name="taskModel">任务数据模型</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddTask([FromBody]TaskModel taskModel)
        {
            if (!string.IsNullOrEmpty(taskModel.Taskname))
            {
                var model = new TaskEntity
                {
                    Id = taskModel.Id,
                    TaskPunishment = _taskPunishmentService.GetTaskPunishmentById(taskModel.TaskPunishmentId),
                    TaskAward = _taskAwardService.GetTaskAwardById(taskModel.TaskAwardId),
                    TaskTag = _taskTagService.GetTaskTagById(taskModel.TaskTagId),
                    TaskType = _taskTypeService.GetTaskTypeById(taskModel.TaskTypeId),
                    Taskname = taskModel.Taskname,
                    Describe = taskModel.Describe,
                    Endtime = taskModel.Endtime,
                    //Adduser = 
                    Addtime = DateTime.Now,
                    //Upuser = 
                    Uptime = DateTime.Now,
                };
                if (taskModel.Type == "add")
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
                if (taskModel.Type == "edit")
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

        /// <summary>
        /// 删除单个任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DelTask(int id)
        {
            try
            {
                _taskService.Delete(_taskService.GetTaskById(id));
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }
        }

        #endregion

        #region 任务类型 杨定鹏 2015年4月28日17:13:12
        /// <summary>
        /// 显示任务类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage TaskTypeList()
        {
            var condition = new TaskTypeSearchCondition
            {
                OrderBy = EnumTaskTypeSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_taskTypeService.GetTaskTypesByCondition(condition).ToList());
        }

        /// <summary>
        /// 添加和修改任务类型表
        /// </summary>
        /// <param name="taskTypeModel">任务类型数据模型</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddTaskTpye([FromBody]TaskTypeModel taskTypeModel)
        {
            if (!string.IsNullOrWhiteSpace(taskTypeModel.Name))
            {
                var model = new TaskTypeEntity
                {
                    Id =taskTypeModel.Id,
                    Name = taskTypeModel.Name,
                    Describe = taskTypeModel.Describe
                };
                if (taskTypeModel.Type == "add")
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
                if (taskTypeModel.Type == "edit")
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
        [HttpPost]
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
        /// 显示任务奖励列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage TaskAwardList()
        {
            var condition = new TaskAwardSearchCondition
            {
                OrderBy = EnumTaskAwardSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_taskAwardService.GetTaskAwardsByCondition(condition).ToList());
        }

        /// <summary>
        /// 添加和修改任务奖励
        /// </summary>
        /// <param name="taskAwardModel">任务奖励数据模型</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddTaskAward([FromBody]TaskAwardModel taskAwardModel)
        {
            if (!string.IsNullOrWhiteSpace(taskAwardModel.Name))
            {
                var model = new TaskAwardEntity
                {
                    Id = taskAwardModel.Id,
                    Name = taskAwardModel.Name,
                    Describe = taskAwardModel.Describe,
                    Value = taskAwardModel.Value
                };
                if (taskAwardModel.Value == "add")
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
                if (taskAwardModel.Value == "edit")
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
        [HttpPost]
        public HttpResponseMessage DelTaskAward(int id)
        {
            _taskAwardService.Delete(_taskAwardService.GetTaskAwardById(id));
            return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
        }

        #endregion

        #region 任务目标 杨定鹏 2015年4月29日15:57:37
        /// <summary>
        /// 显示任务目标列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage TaskTagList()
        {
            var condition = new TaskTagSearchCondition
            {
                OrderBy = EnumTaskTagSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_taskTagService.GetTaskTagsByCondition(condition).ToList());
        }

        /// <summary>
        /// 添加和修改任务目标
        /// </summary>
        /// <param name="taskTagModel">任务目标数据模型</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddTaskTag([FromBody]TaskTagModel taskTagModel)
        {
            if (!string.IsNullOrWhiteSpace(taskTagModel.Name))
            {
                var model = new TaskTagEntity
                {
                    Id = taskTagModel.Id,
                    Name = taskTagModel.Name,
                    Describe = taskTagModel.Describe,
                    Value = taskTagModel.Value
                };
                if (taskTagModel.Type == "add")
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
                if (taskTagModel.Type == "edit")
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
        [HttpPost]
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
        /// 显示任务惩罚列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage TaskPunishmentList()
        {
            var condition = new TaskPunishmentSearchCondition
            {
                OrderBy = EnumTaskPunishmentSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_taskPunishmentService.GetTaskPunishmentsByCondition(condition).ToList());
        }

        /// <summary>
        /// 添加和修改任务惩罚
        /// </summary>
        /// <param name="taskPunishmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddTaskPunishment([FromBody]TaskPunishmentModel taskPunishmentModel)
        {
            if (!string.IsNullOrWhiteSpace(taskPunishmentModel.Name))
            {
                var model = new TaskPunishmentEntity
                {
                    Id=taskPunishmentModel.Id,
                    Name = taskPunishmentModel.Name,
                    Describe = taskPunishmentModel.Describe,
                    Value = taskPunishmentModel.Value
                };
                if (taskPunishmentModel.Type == "add")
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
                if (taskPunishmentModel.Type == "edit")
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
        [HttpPost]
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
