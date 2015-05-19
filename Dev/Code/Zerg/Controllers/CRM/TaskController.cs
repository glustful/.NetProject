using System;
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
using CRM .Service .TaskList ;
using Zerg.Common;
using Zerg.Models.CRM;
using CRM.Service.Broker;

namespace Zerg.Controllers.CRM
{
     [EnableCors("*", "*", "*")]
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
        private readonly ITaskListService _taskListService;
         private readonly IBrokerService _brokerService;

        public TaskController(ITaskService taskService, 
            ITaskTypeService taskTypeService, 
            ITaskAwardService taskAwardService, 
            ITaskTagService taskTagService, 
            ITaskPunishmentService taskPunishmentService,
            ITaskListService taskListService,
            IBrokerService  brokerService
            )
        {
            _taskService = taskService;
            _taskTypeService = taskTypeService;
            _taskAwardService = taskAwardService;
            _taskTagService = taskTagService;
            _taskPunishmentService = taskPunishmentService;
            _taskListService = taskListService;
            _brokerService = brokerService;
        }

        #region 单个任务配置 杨定鹏 2015年4月28日10:04:08

        /// <summary>
        /// 返回任务列表
        /// </summary>
        /// <param name="taskSearchModel"></param>
        /// <returns></returns>
        [HttpGet]
        //public HttpResponseMessage TaskList(TaskSearchCondition searchCondition)
        //{
        //    if (searchCondition == null)
        //    {
        //        return PageHelper.toJson(PageHelper.ReturnValue(false, "condition is null"));
        //    }

        //    var condition = new TaskSearchCondition
        //    {
        //        OrderBy = EnumTaskSearchOrderBy.OrderById,
        //        Taskname = searchCondition.Taskname

        //    };
        //    return PageHelper.toJson(_taskService.GetTasksByCondition(condition).ToList());
        //}
      

        public HttpResponseMessage TaskList(string Taskname, int page, int pageSize)
        {                     
            var taskcondition = new TaskSearchCondition
            {
                OrderBy = EnumTaskSearchOrderBy.OrderById,
                Taskname = Taskname,
                Page = page,
                PageCount = pageSize 
              
            };
            var taskList = _taskService.GetTasksByCondition(taskcondition).Select(p => new
            {
                Taskname=p.Taskname ,
                Name=p.TaskType.Name ,
                Endtime=p.Endtime ,
                Adduser=p.Adduser ,
                Id=p.Id 
            }).ToList ();
            var taskCount = _taskService.GetTaskCount(taskcondition);
            return PageHelper.toJson(new { list = taskList, totalCount = taskCount, condition=taskcondition  });
        }
         /// <summary>
         /// 返回任务详情
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         [HttpGet]
         public HttpResponseMessage TaskDetail( int id)   
         {
             var task=_taskService.GetTaskById(id);
             var model = new TaskModel();
              model.Id=task.Id;
              model.Taskname = task.Taskname;
              model.tagName =task.TaskTag.Name;
              model.Endtime = task.Endtime;
              model.awardName = task.TaskAward.Name;
              model.Describe =task.Describe;
              model.TaskPunishment = task.TaskPunishment.Name;
              model.TaskType = task.TaskType.Name;
              model.TaskAwardId = task.TaskAward.Id;
              model.TaskPunishmentId = task.TaskPunishment.Id;
              model.TaskTagId = task.TaskTag.Id;
            model .TaskTypeId =task .TaskType .Id ;
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
         /// 查找同一任务列表
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         [HttpGet]
        public HttpResponseMessage taskListBytaskId(int id, int page, int pageSize)
        {
            var taskCondition = new TaskListSearchCondition
            {
                TaskId = id,
                Page =page ,
                PageCount =pageSize 
            };
            var tasklistone = _taskListService.GetTaskListsByCondition(taskCondition).Select(p => new
            {
                Taskname=p.Task .Taskname ,
                Brokername=p.Broker .Brokername ,
                Taskschedule=p.Taskschedule ,
                Endtime=p.Task .Endtime 
            }).ToList ();

            var taskCount = _taskListService.GetTaskListCount(taskCondition);
            return PageHelper.toJson(new { list = tasklistone, totalCount = taskCount ,condition=taskCondition });


        }
     /// <summary>
     /// 查询任务列表
     /// </summary>
     /// <param name="brokerName"></param>
     /// <param name="id"></param>
     /// <param name="page"></param>
     /// <param name="pageSize"></param>
     /// <returns></returns>
         [HttpGet]
         public HttpResponseMessage taskListByuser(string brokerName, int id, int page, int pageSize)
         {
           
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
             });

             var taskCount = _taskListService.GetTaskListCount(taskCondition);
             return PageHelper.toJson(new { list = tasklistone, totalCount = taskCount,condition=taskCondition  });


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
                    Adduser = 1,
                    Addtime = DateTime.Now,
                    Upuser = 1,
                    Uptime = DateTime.Now,
                };
                var mo1 = new TaskSearchCondition
                {
                    Taskname = taskModel.Taskname
                };
                if (taskModel.Type == "add")
                {
                    int taskCount = _taskService.GetTaskCount(mo1);
                  if (taskCount == 0) { 
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
                    var cond=new TaskListSearchCondition (){
                    TaskId =taskModel .Id 
                };
                int tlistcout = _taskListService.GetTaskListCount(cond);
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "不能修改，已经有人接手任务")); }
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
                    mdel.Endtime = taskModel.Endtime;
                    mdel.Adduser = 1;
                    mdel.Addtime = DateTime.Now;
                    mdel.Upuser = 1;
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
                var cond=new TaskListSearchCondition (){
                    TaskId =id
                };
                int tlistcout = _taskListService.GetTaskListCount(cond);
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "不能删除，已经有人接手任务")); }
                else { 
                _taskService.Delete(_taskService.GetTaskById(id));
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));}
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
            var typelist = _taskTypeService.GetTaskTypesByCondition(condition).Select(p => new
            {
                Id=p.Id ,
                Name =p.Name 
            }).ToList();
            return PageHelper.toJson(typelist);
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
        [HttpGet]
        public HttpResponseMessage DelTaskType(int id)
        {
            try
            {
                 var cond=new TaskSearchCondition (){
                    typeId =id
                };
                int tlistcout = _taskService.GetTaskCount(cond);
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "不能删除，已有任务中正使用该任务类型")); }
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
        /// 显示任务奖励列表
        /// </summary>
        /// <returns></returns>
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
                if (taskAwardModel.Type == "add")
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
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "不能删除，已有任务中正使用该任务奖励")); }
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
        /// <returns></returns>
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
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "不能删除，已有任务中正使用该任务目标")); }
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
        /// <returns></returns>
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
                if (tlistcout > 0) { return PageHelper.toJson(PageHelper.ReturnValue(false, "不能删除，已有任务中正使用该任务惩罚")); }
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
