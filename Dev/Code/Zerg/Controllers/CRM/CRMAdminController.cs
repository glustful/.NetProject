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
                    _taskTypeService.Create(model);
                }
                if (type == "edit")
                {
                    _taskTypeService.Update(_taskTypeService.GetTaskTypeById(taskTypeModel.Id));
                }
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "类型名称不能为空！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(true, "操作成功"));
        }

        /// <summary>
        /// 删除任务类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage DelTaskType(int id)
        {
            _taskTypeService.Delete(_taskTypeService.GetTaskTypeById(id));
            return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
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