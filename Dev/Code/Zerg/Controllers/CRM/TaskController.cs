﻿using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls.Expressions;
using CRM.Entity.Model;
//using CRM.Service.BLPay;
using CRM.Service.Task;
using CRM.Service.TaskAward;
using CRM.Service.TaskPunishment;
using CRM.Service.TaskTag;
using CRM.Service.TaskType;
using CRM.Service.TaskList;
using Zerg.Common;
using Zerg.Models.CRM;
using CRM.Service.Broker;
using System.Text.RegularExpressions;
using YooPoon.WebFramework.User.Entity;
using YooPoon.Core.Site;
using System.ComponentModel;
using YooPoon.WebFramework.User.Services;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]

    /// <summary>
    /// CRM 任务管理明细
    /// </summary>
    [Description("CRM任务管理类")]
    public class TaskController : ApiController
    {
        private readonly ITaskService _taskService;
        private readonly ITaskTypeService _taskTypeService;
        private readonly ITaskAwardService _taskAwardService;
        private readonly ITaskTagService _taskTagService;
        private readonly ITaskPunishmentService _taskPunishmentService;
        private readonly ITaskListService _taskListService;
        private readonly IBrokerService _brokerService;
        private readonly IWorkContext _workContext;
        private readonly IUserService _userservice;

        /// <summary>
        /// CRM任务管理初始化
        /// </summary>
        /// <param name="taskService">taskService</param>
        /// <param name="taskTypeService">taskTypeService</param>
        /// <param name="taskAwardService">taskAwardService</param>
        /// <param name="taskTagService">taskTagService</param>
        /// <param name="taskPunishmentService">taskPunishmentService</param>
        /// <param name="taskListService">taskListService</param>
        /// <param name="brokerService">brokerService</param>
        /// <param name="workContext">workContext</param>
        public TaskController(ITaskService taskService,
            ITaskTypeService taskTypeService,
            ITaskAwardService taskAwardService,
            ITaskTagService taskTagService,
            ITaskPunishmentService taskPunishmentService,
            ITaskListService taskListService,
            IBrokerService brokerService,
            IWorkContext workContext,
            IUserService userService
            )
        {
            _taskService = taskService;
            _taskTypeService = taskTypeService;
            _taskAwardService = taskAwardService;
            _taskTagService = taskTagService;
            _taskPunishmentService = taskPunishmentService;
            _taskListService = taskListService;
            _brokerService = brokerService;
            _workContext = workContext;
            _userservice = userService;
        }

        #region 单个任务配置 杨定鹏 2015年4月28日10:04:08

       
        /// <summary>
        /// 返回任务列表
        /// </summary>
        /// <param name="Taskname">任务名称</param>
        /// <param name="page">第几页</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="orderByAll">排序参数｛任务名称（OrderByTaskname），任务类型（OrderByName），结束时间（OrderByEndtime），创建人（OrderByAdduser）｝</param>
        /// <param name="isDes">是否降序</param>
        /// <returns>任务列表</returns>
        [Description("返回PC端任务列表")]
        [HttpGet]
        public HttpResponseMessage TaskList(string Taskname="", int page=1, int pageSize=10, EnumTaskSearchOrderBy orderByAll = EnumTaskSearchOrderBy .OrderById, bool isDes = true)
        {
            //验证是否有非法字符
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");

            if (!string.IsNullOrEmpty(Taskname))
            {
                var m = reg.IsMatch(Taskname);
                if (!m)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "搜索输入存在非法字符！"));
                }
            }
            var taskcondition = new TaskSearchCondition
            {
                OrderBy =orderByAll,
                Taskname = Taskname,
                Page = page,
                PageCount = pageSize,
                IsDescending =isDes 

            };
            try
            {

                var taskList = _taskService.GetTasksByCondition(taskcondition).Select(p => new
                {
                    Taskname = p.Taskname,
                    Name = p.TaskType.Name,
                    Endtime = p.Endtime,
                    Adduser = p.Adduser,
                    Id = p.Id
                }).ToList().Select(p => new
                {
                    Taskname = p.Taskname,
                    Name = p.Name,
                    Endtime = p.Endtime.ToShortDateString(),
                    //  Adduser = _brokerService.GetBrokerByUserId(p.Adduser).Brokername,
                    Adduser = _userservice.GetUserById(p.Adduser) == null ? p.Adduser.ToString() : _userservice.GetUserById(p.Adduser).UserName,
                    Id = p.Id
                }).ToList();
                var taskCount = _taskService.GetTaskCount(taskcondition);
                if (taskCount > 0)
                {
                    return PageHelper.toJson(new { list = taskList, totalCount = taskCount, condition = taskcondition });
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "不存在数据！"));
                }
            }
            catch {
                return PageHelper.toJson(PageHelper.ReturnValue(false , "出现数据错误！"));
            }
        }
        /// <summary>
        /// 查询返回手机断任务列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="type">类型</param>
        /// <returns>手机端任务列表</returns>
        [Description("查询返回手机端任务列表")]
        [HttpGet]
        public HttpResponseMessage TaskListMobile(int page, string type)
        {

            int pageSize = 10;

            var taskcondition = new TaskSearchCondition
            {
                OrderBy = EnumTaskSearchOrderBy.OrderById,
                Page = page,
                PageCount = pageSize,


            };
            if (type == "today")///查找今天的任务，否则查询所有任务
            {
                //taskcondition.AddtimeBegin = DateTime.Today;
                //taskcondition.AddtimeEnd = DateTime.Today.AddDays(1);
                taskcondition.PageCount = 5;
                taskcondition.Page = 1;
                taskcondition.IsDescending = true;//降序
            }
            var taskList = _taskService.GetTasksByCondition(taskcondition).Select(p => new
            {
                Taskname = p.Taskname,
                Name = p.TaskType.Name,
                awardname = p.TaskAward.Name,
                awardvalue = p.TaskAward.Value,
                Endtime = p.Endtime,
                Adduser = p.Adduser,
                Id = p.Id,

            }).ToList();
            var taskCount = _taskService.GetTaskCount(taskcondition);
            if (taskCount > 0)
            {
                return PageHelper.toJson(new { list = taskList, totalCount = taskCount, condition = taskcondition });
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "不存在数据！"));
            }
        }
        /// <summary>
        /// 传入任务id，返回任务详情
        /// </summary>
        /// <param name="Id">任务id</param>
        /// <returns>任务详情</returns>
        [Description("传入任务id，返回任务详情")]

        [HttpGet]
        public HttpResponseMessage TaskDetail(int Id)
        {
            var task = _taskService.GetTaskById(Id);
            var model = new TaskModel();
            model.Id = task.Id;
            model.Taskname = task.Taskname;
            model.tagName = task.TaskTag.Name;
            model.Endtime = task.Endtime.ToShortDateString();
            model.awardName = task.TaskAward.Name;
            model.Describe = task.Describe;
            model.TaskPunishment = task.TaskPunishment.Name;
            model.TaskType = task.TaskType.Name;
            model.TaskAwardId = task.TaskAward.Id;
            model.TaskPunishmentId = task.TaskPunishment.Id;
            model.TaskTagId = task.TaskTag.Id;
            model.TaskTypeId = task.TaskType.Id;
            return PageHelper.toJson(model);
            //var taskcondition = new TaskSearchCondition
            //{
            //    OrderBy = EnumTaskSearchOrderBy.OrderById,
            //    Id = id

            //};
            //var taskdetail = _taskService.GetTasksByCondition(taskcondition).Select(p => new
            //{
            //    Taskname = p.Taskname,
            //    tagName = p.TaskTag.Name,
            //    Endtime = p.Endtime,
            //    awardName = p.TaskAward.Name,
            //    Describe = p.Describe,
            //    TaskPunishment = p.TaskPunishment.Name,
            //    TaskType = p.TaskType.Name
            //});

            //return PageHelper.toJson(new { taskd = taskdetail });

        }
        /// <summary>
        /// 查找同一个任务列表
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>任务列表</returns>
        [HttpGet]
        public HttpResponseMessage taskListBytaskId(int id, int page, int pageSize)
        {

            var taskCondition = new TaskListSearchCondition
            {
                TaskId = id,
                Page = page,
                PageCount = pageSize
            };

            var tasklistone = _taskListService.GetTaskListsByCondition(taskCondition).Select(p => new
            {
                Taskname = p.Task.Taskname,
                Brokername = p.Broker.Brokername,
                Taskschedule = p.Taskschedule,
                Endtime = p.Task.Endtime.ToShortDateString(),
            }).ToList();

            var taskCount = _taskListService.GetTaskListCount(taskCondition);
            return PageHelper.toJson(new { list = tasklistone, totalCount = taskCount, condition = taskCondition });


        }
        /// <summary>
        /// 查询任务列表
        /// </summary>
        /// <param name="brokerName">经纪人名称</param>
        /// <param name="id">任务id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>任务列表</returns>
        [Description("查询任务列表")]
        [HttpGet]
        public HttpResponseMessage taskListByuser(string brokerName, int id, int page, int pageSize)
        {
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");

            if (!string.IsNullOrEmpty(brokerName))
            {
                var m = reg.IsMatch(brokerName);
                if (!m)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "搜索输入存在非法字符！"));
                }
            }
            var taskCondition = new TaskListSearchCondition
            {
                TaskId = id,
                Page = page,
                PageCount = pageSize,
                BrokerName = brokerName
            };
            var tasklistone = _taskListService.GetTaskListsByCondition(taskCondition).Select(p => new
            {
                Taskname = p.Task.Taskname,
                Brokername = p.Broker.Brokername,
                Taskschedule = p.Taskschedule,
                Endtime = p.Task.Endtime
            }).ToList().Select(
            p => new
            {
                Taskname = p.Taskname,
                Brokername = p.Brokername,
                Taskschedule = p.Taskschedule,
                Endtime = p.Endtime.ToString()
            });

            var taskCount = _taskListService.GetTaskListCount(taskCondition);
            if (taskCount > 0)
            {
                return PageHelper.toJson(new { list = tasklistone, totalCount = taskCount, condition = taskCondition });
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "不存在数据！"));
            }


        }
        /// <summary>
        /// 添加和修改单个任务
        /// </summary>
        /// <param name="taskModel">任务数据模型</param>
        /// <returns>添加和修改结果状态信息</returns>
        [Description("添加和修改单个任务")]
        [HttpPost]
        public HttpResponseMessage AddTask([FromBody]TaskModel taskModel)
        {
            if (!string.IsNullOrEmpty(taskModel.Taskname))
            {
                //用正则表达式验证是否存在非法字符/^(\d{4})-(0\d{1}|1[0-2])-(0\d{1}|[12]\d{1}|3[01])$/
                Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
                var m = reg.IsMatch(taskModel.Taskname);
                if (!m)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "任务名称存在非法字符！"));
                }

                //验证日期格式
                if (taskModel.Endtime == null)
                {

                    return PageHelper.toJson(PageHelper.ReturnValue(false, "结束时间格式错误！"));
                }

                if (DateTime.Today >= Convert.ToDateTime(taskModel.Endtime))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "结束时间至少是明天！"));
                }
                if (!string.IsNullOrEmpty(taskModel.Describe))
                {
                    var m1 = reg.IsMatch(taskModel.Describe);

                    if (!m1)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "描述存在非法字符！"));
                    }
                }
                var user = (UserBase)_workContext.CurrentUser;
                var model = new TaskEntity
                {
                    Id = user.Id,
                    TaskPunishment = _taskPunishmentService.GetTaskPunishmentById(taskModel.TaskPunishmentId),
                    TaskAward = _taskAwardService.GetTaskAwardById(taskModel.TaskAwardId),
                    TaskTag = _taskTagService.GetTaskTagById(taskModel.TaskTagId),
                    TaskType = _taskTypeService.GetTaskTypeById(taskModel.TaskTypeId),
                    Taskname = taskModel.Taskname,
                    Describe = taskModel.Describe,
                    Endtime = Convert.ToDateTime(taskModel.Endtime),
                    Adduser = _workContext.CurrentUser.Id,
                    Addtime = DateTime.Now,
                    Upuser = 0,
                    Uptime = DateTime.Now,
                };
                var mo1 = new TaskSearchCondition
                {
                    TasknameRe = taskModel.Taskname
                };
                if (taskModel.Type == "add")
                {
                    //判断是否存在同名名称
                    int taskCount = _taskService.GetTaskCount(mo1);
                    if (taskCount == 0)
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
                    else
                    { return PageHelper.toJson(PageHelper.ReturnValue(false, "任务名称已存在，请换名称")); }
                }
                if (taskModel.Type == "edit")
                {
                    var cond = new TaskListSearchCondition()
                    {
                        TaskId = taskModel.Id
                    };
                    //判断任务是否被接手

                    int tlistcout = _taskListService.GetTaskListCount(cond);
                    //判断是否存在同名名称
                    var mo11 = new TaskSearchCondition
                    {
                        TasknameRe = taskModel.Taskname,
                        Id = taskModel.Id
                    };
                    int tasknameCount = _taskService.GetTaskCount(mo11);
                    if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "不能修改，已经有人接手任务")); }

                    else if (tasknameCount > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "任务名称已存在，请换名称")); }
                    else
                    {
                        var mdel = _taskService.GetTaskById(taskModel.Id);
                        mdel.Id = taskModel.Id;
                        mdel.TaskPunishment = _taskPunishmentService.GetTaskPunishmentById(taskModel.TaskPunishmentId);
                        mdel.TaskAward = _taskAwardService.GetTaskAwardById(taskModel.TaskAwardId);
                        mdel.TaskTag = _taskTagService.GetTaskTagById(taskModel.TaskTagId);
                        mdel.TaskType = _taskTypeService.GetTaskTypeById(taskModel.TaskTypeId);
                        mdel.Taskname = taskModel.Taskname;
                        mdel.Describe = taskModel.Describe;
                        mdel.Endtime = Convert.ToDateTime(taskModel.Endtime);
                        mdel.Upuser = _workContext.CurrentUser.Id;
                        mdel.Uptime = DateTime.Now;
                        try
                        {
                            _taskService.Update(mdel);
                            return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
                        }
                        catch (Exception)
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
                        }
                    }
                }
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "任务名称不能为空！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证失败"));
        }
        /// <summary>
        /// 传入任务参数，添加任务，返回添加结果状态信息
        /// </summary>
        /// <param name="taskListModel">任务列表参数</param>
        /// <returns>任务列表</returns>
        [Description("添加任务")]
        [HttpPost]
        public HttpResponseMessage AddTaskList([FromBody]TaskListModel taskListModel)
        {



            var model = new TaskListEntity
            {
                Task = _taskService.GetTaskById(taskListModel.TaskId),
                Broker = _brokerService.GetBrokerByUserId(taskListModel.UserId),
                Taskschedule = taskListModel.Taskschedule,


            };
            //var mo1 = new TaskListSearchCondition
            //{
            //    TaskId  = taskListModel.TaskId 
            //};
            //if (taskListModel.Type == "add")
            //{
            //    //判断是否存在同名名称
            //    int taskCount = _taskListService.GetTaskListCount(mo1);
            //    if (taskCount == 0)
            //    {
            try
            {
                if (_taskListService.Create(model) != null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功"));
                }
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败"));
            }
            //    }
            //    else
            //    { return PageHelper.toJson(PageHelper.ReturnValue(false, "任务名称已存在，请换名称")); }
            //}
            //if (taskListModel.Type == "edit")
            //{
            //    var cond = new TaskListSearchCondition()
            //    {
            //        TaskId = taskListModel.Id
            //    };
            //    //判断任务是否被接手

            //    int tlistcout = _taskListService.GetTaskListCount(cond);
            //    //判断是否存在同名名称
            //    var mo11 = new TaskSearchCondition
            //    {
            //        TasknameRe = taskListModel.Taskname,
            //        Id = taskListModel.Id
            //    };
            //    int tasknameCount = _taskService.GetTaskCount(mo11);
            //    if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "不能修改，已经有人接手任务")); }

            //    else if (tasknameCount > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "任务名称已存在，请换名称")); }
            //    else
            //    {
            //        var mdel = _taskService.GetTaskById(taskListModel.Id);
            //        mdel.Id = taskListModel.Id;
            //        mdel.TaskPunishment = _taskPunishmentService.GetTaskPunishmentById(taskListModel.TaskPunishmentId);
            //        mdel.TaskAward = _taskAwardService.GetTaskAwardById(taskListModel.TaskAwardId);
            //        mdel.TaskTag = _taskTagService.GetTaskTagById(taskListModel.TaskTagId);
            //        mdel.TaskType = _taskTypeService.GetTaskTypeById(taskListModel.TaskTypeId);
            //        mdel.Taskname = taskListModel.Taskname;
            //        mdel.Describe = taskListModel.Describe;
            //        mdel.Endtime = taskListModel.Endtime;
            //        mdel.Adduser = 1;
            //        mdel.Addtime = DateTime.Now;
            //        mdel.Upuser = 1;
            //        mdel.Uptime = DateTime.Now;
            //        try
            //        {
            //            _taskService.Update(mdel);
            //            return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
            //        }
            //        catch (Exception)
            //        {
            //            return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
            //        }
            //    }
            //}

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证失败"));
        }

        /// <summary>
        /// 传入任务ID，删除任务，返回删除结果状态信息
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns>删除结果状态信息</returns>
        [Description("删除单个任务")]
        [HttpGet]
        public HttpResponseMessage DelTask(int id)
        {
            try
            {
                var cond = new TaskListSearchCondition()
                {
                    TaskId = id
                };
                int tlistcout = _taskListService.GetTaskListCount(cond);
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "不能删除，已经有人接手任务")); }
                else
                {
                    _taskService.Delete(_taskService.GetTaskById(id));
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
                }
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }
        }

        #endregion

        #region 任务类型 杨定鹏 2015年4月28日17:13:12
        /// <summary>
        /// 检索当前任务，返回任务类型列表
        /// </summary>
        /// <returns>任务类型列表</returns>
        [Description("检索返回当前任务类型列表")]
        [HttpGet]
        public HttpResponseMessage TaskTypeList()
        {
            var condition = new TaskTypeSearchCondition
            {
                OrderBy = EnumTaskTypeSearchOrderBy.OrderById
            };
            var typelist = _taskTypeService.GetTaskTypesByCondition(condition).Select(p => new
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            return PageHelper.toJson(typelist);
        }

        /// <summary>
        /// 添加和修改任务类型表
        /// </summary>
        /// <param name="taskTypeModel">任务类型数据模型</param>
        /// <returns>添加和修改结果状态信息</returns>
        [Description("添加或者修改任务类型列表")]
        [HttpPost]
        public HttpResponseMessage AddTaskTpye([FromBody]TaskTypeModel taskTypeModel)
        {
            if (!string.IsNullOrWhiteSpace(taskTypeModel.Name))
            {
                Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
                var m = reg.IsMatch(taskTypeModel.Name);
                if (!m)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "类型名称存在非法字符！"));
                }
                if (taskTypeModel.Describe != "")
                {
                    var m1 = reg.IsMatch(taskTypeModel.Describe);

                    if (!m1)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "描述存在非法字符！"));
                    }
                }
                var model = new TaskTypeEntity
                {
                    Id = taskTypeModel.Id,
                    Name = taskTypeModel.Name,
                    Describe = taskTypeModel.Describe
                };
                var mo1 = new TaskTypeSearchCondition
             {
                 NameRe = taskTypeModel.Name
             };

                if (taskTypeModel.Type == "add")
                {
                    int taskTypeCount = _taskTypeService.GetTaskTypeCount(mo1);
                    if (taskTypeCount > 0)
                    { return PageHelper.toJson(PageHelper.ReturnValue(false, "名称重复，请更换")); }
                    else
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
        /// 传入任务类型ID，删除任务类型，返回删除结果状态信息
        /// </summary>
        /// <param name="id">任务类型ID</param>
        /// <returns>删除结果状态信息</returns>
        [HttpGet]
        public HttpResponseMessage DelTaskType(int id)
        {
            try
            {
                var cond = new TaskSearchCondition()
                {
                    typeId = id
                };
                int tlistcout = _taskService.GetTaskCount(cond);
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "该类型使用中，删除失败")); }
                else
                {
                    _taskTypeService.Delete(_taskTypeService.GetTaskTypeById(id));
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
                }
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }
        }

        #endregion

        #region 任务奖励 杨定鹏 2015年4月29日15:15:41
        /// <summary>
        /// 检索返回当前任务奖励列表
        /// </summary>
        /// <returns>任务奖励列表</returns>
        [Description("检索返回当前任务奖励列表")]
        [HttpGet]
        public HttpResponseMessage TaskAwardList()
        {
            //var condition = new TaskAwardSearchCondition
            //{
            //    OrderBy = EnumTaskAwardSearchOrderBy.OrderById
            //};
            //return PageHelper.toJson(_taskAwardService.GetTaskAwardsByCondition(condition).ToList());
            var condition = new TaskAwardSearchCondition
            {
                OrderBy = EnumTaskAwardSearchOrderBy.OrderById
            };
            var typelist = _taskAwardService.GetTaskAwardsByCondition(condition).Select(p => new
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            return PageHelper.toJson(typelist);
        }

        /// <summary>
        /// 添加和修改任务奖励，返回结果状态信息
        /// </summary>
        /// <param name="taskAwardModel">任务奖励数据模型</param>
        /// <returns>添加或者修改任务奖励结果状态信息</returns>
        [HttpPost]
        public HttpResponseMessage AddTaskAward([FromBody]TaskAwardModel taskAwardModel)
        {
            if (!string.IsNullOrWhiteSpace(taskAwardModel.Name))
            {
                Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
                var m = reg.IsMatch(taskAwardModel.Name);
                if (!m)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "奖励名称存在非法字符！"));
                }
                if (!string.IsNullOrEmpty(taskAwardModel.Describe))
                {
                    var m1 = reg.IsMatch(taskAwardModel.Describe);

                    if (!m1)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "描述存在非法字符！"));
                    }
                }
                var model = new TaskAwardEntity
                {
                    Id = taskAwardModel.Id,
                    Name = taskAwardModel.Name,
                    Describe = taskAwardModel.Describe,
                    Value = taskAwardModel.Value
                };
                var mo1 = new TaskAwardSearchCondition
                {
                    NameRe = taskAwardModel.Name
                };
                if (taskAwardModel.Type == "add")
                {
                    int taskTypeCount = _taskAwardService.GetTaskAwardCount(mo1);
                    if (taskTypeCount > 0)
                    { return PageHelper.toJson(PageHelper.ReturnValue(false, "名称重复，请更换")); }
                    else
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
        /// 传入任务奖励ID，删除任务奖励，返回删除结果状态信息
        /// </summary>
        /// <param name="id">任务奖励ID</param>
        /// <returns>删除结果状态信息</returns>
        [Description("传入任务奖励ID，删除任务奖励，返回删除结果状态信息")]
        [HttpGet]
        public HttpResponseMessage DelTaskAward(int id)
        {
            try
            {
                var cond = new TaskSearchCondition()
                {
                    awardId = id
                };
                int tlistcout = _taskService.GetTaskCount(cond);
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "该奖励使用中，删除失败")); }
                else
                {
                    _taskAwardService.Delete(_taskAwardService.GetTaskAwardById(id));
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
                }
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }


        }

        #endregion

        #region 任务目标 杨定鹏 2015年4月29日15:57:37
        /// <summary>
        /// 显示任务目标列表
        /// </summary>
        /// <returns>任务目标列表</returns>
        [Description("检索返回任务目标列表")]
        [HttpGet]
        public HttpResponseMessage TaskTagList()
        {
            //var condition = new TaskTagSearchCondition
            //{
            //    OrderBy = EnumTaskTagSearchOrderBy.OrderById
            //};
            //return PageHelper.toJson(_taskTagService.GetTaskTagsByCondition(condition).ToList());
            var condition = new TaskTagSearchCondition
            {
                OrderBy = EnumTaskTagSearchOrderBy.OrderById
            };
            var typelist = _taskTagService.GetTaskTagsByCondition(condition).Select(p => new
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            return PageHelper.toJson(typelist);
        }

        /// <summary>
        /// 添加和修改任务目标
        /// </summary>
        /// <param name="taskTagModel">任务目标数据模型</param>
        /// <returns>添加和修改结果状态信息</returns>
        [Description("添加或者修改任务目标")]
        [HttpPost]
        public HttpResponseMessage AddTaskTag([FromBody]TaskTagModel taskTagModel)
        {
            if (!string.IsNullOrWhiteSpace(taskTagModel.Name))
            {
                Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
                var m = reg.IsMatch(taskTagModel.Name);
                if (!m)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "目标名称存在非法字符！"));
                }
                if (!string.IsNullOrEmpty(taskTagModel.Describe))
                {
                    var m1 = reg.IsMatch(taskTagModel.Describe);

                    if (!m1)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "描述存在非法字符！"));
                    }
                }
                var model = new TaskTagEntity
                {
                    Id = taskTagModel.Id,
                    Name = taskTagModel.Name,
                    Describe = taskTagModel.Describe,
                    Value = taskTagModel.Value
                };
                var mo1 = new TaskTagSearchCondition
               {
                   NameRe = taskTagModel.Name
               };
                if (taskTagModel.Type == "add")
                {
                    int taskTagCount = _taskTagService.GetTaskTagCount(mo1);
                    if (taskTagCount > 0)
                    { return PageHelper.toJson(PageHelper.ReturnValue(false, "名称重复，请更换")); }
                    else
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
        /// 传入任务id奖励，删除任务奖励，返回删除结果状态信息
        /// </summary>
        /// <param name="id">任务奖励ID</param>
        /// <returns>任务奖励删除结果状态信息</returns>
        [Description("删除任务奖励")]
        [HttpGet]
        public HttpResponseMessage DelTaskTag(int id)
        {

            try
            {
                var cond = new TaskSearchCondition()
                {
                    tagId = id
                };
                int tlistcout = _taskService.GetTaskCount(cond);
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "该目标使用中，删除失败")); }
                else
                {
                    _taskTagService.Delete(_taskTagService.GetTaskTagById(id));
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
                }
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
        /// <returns>任务惩罚列表</returns>
        [HttpGet]
        public HttpResponseMessage TaskPunishmentList()
        {
            //var condition = new TaskPunishmentSearchCondition
            //{
            //    OrderBy = EnumTaskPunishmentSearchOrderBy.OrderById
            //};
            //return PageHelper.toJson(_taskPunishmentService.GetTaskPunishmentsByCondition(condition).ToList());
            var condition = new TaskPunishmentSearchCondition
            {
                OrderBy = EnumTaskPunishmentSearchOrderBy.OrderById
            };
            var typelist = _taskPunishmentService.GetTaskPunishmentsByCondition(condition).Select(p => new
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            return PageHelper.toJson(typelist);
        }

        /// <summary>
        /// 添加和修改任务惩罚，返回添加或者修改结果状态信息
        /// </summary>
        /// <param name="taskPunishmentModel">taskPunishmentModel</param>
        /// <returns>添加或修改任务惩罚</returns>
        [Description("添加或修改任务惩罚")]
        [HttpPost]
        public HttpResponseMessage AddTaskPunishment([FromBody]TaskPunishmentModel taskPunishmentModel)
        {
            if (!string.IsNullOrWhiteSpace(taskPunishmentModel.Name))
            {
                Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
                var m = reg.IsMatch(taskPunishmentModel.Name);
                if (!m)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "惩罚名称存在非法字符！"));
                }
                if (!string.IsNullOrEmpty(taskPunishmentModel.Describe))
                {
                    var m1 = reg.IsMatch(taskPunishmentModel.Describe);

                    if (!m1)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "描述存在非法字符！"));
                    }
                }
                var model = new TaskPunishmentEntity
                {
                    Id = taskPunishmentModel.Id,
                    Name = taskPunishmentModel.Name,
                    Describe = taskPunishmentModel.Describe,
                    Value = taskPunishmentModel.Value
                };
                var mo1 = new TaskPunishmentSearchCondition
                {
                    NameRe = taskPunishmentModel.Name
                };
                if (taskPunishmentModel.Type == "add")
                {
                    int taskPunishCount = _taskPunishmentService.GetTaskPunishmentCount(mo1);
                    if (taskPunishCount > 0)
                    { return PageHelper.toJson(PageHelper.ReturnValue(false, "名称重复，请更换")); }
                    else
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
        /// c传入任务惩罚ID，删除任务惩罚，返回删除结果状态信息
        /// </summary>
        /// <param name="id">任务惩罚ID</param>
        /// <returns>删除结果状态信息</returns>
        [Description("添加或修改任务惩罚")]
        [HttpGet]
        public HttpResponseMessage DelTaskPunishment(int id)
        {
            try
            {
                var cond = new TaskSearchCondition()
                {
                    punishId = id
                };
                int tlistcout = _taskService.GetTaskCount(cond);
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "该惩罚使用中，删除失败")); }
                else
                {
                    _taskPunishmentService.Delete(_taskPunishmentService.GetTaskPunishmentById(id));
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
                }
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }

        }

        #endregion
    }
}
