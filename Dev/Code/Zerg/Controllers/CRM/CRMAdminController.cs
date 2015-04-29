using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using CRM.Entity.Model;
using CRM.Service.MessageDetail;
using CRM.Entity.Model;
using CRM.Service.Task;
using CRM.Service.TaskType;
using Zerg.Models.CRM;
using System;
using CRM.Service.MessageConfig;


namespace Zerg.Controllers
{

    /// <summary>
    /// CRM  管理员
    /// </summary>
    public class CRMAdminController : ApiController
    {

        private readonly ITaskService _taskService;
        private readonly ITaskTypeService _taskTypeService;
        private readonly IMessageDetailService _messageDetailService;
        private readonly IMessageConfigService _MessageConfigService;
        public CRMAdminController(IMessageDetailService messageDetailService,ITaskService taskService,  ITaskTypeService taskTypeService ,IMessageConfigService messageConfigService)
        {
            _messageDetailService = messageDetailService;
            _taskService = taskService;
            _taskTypeService = taskTypeService;
            _MessageConfigService = messageConfigService;
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



        #region   短信  黄秀宇   2015.4.28
        /// <summary>
        /// 查询短信列表
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PageCount"></param>
        /// <param name="isDescending"></param>
        /// <param name="Ids"></param>
        /// <param name="AddtimeBegin"></param>
        /// <param name="AddtimeEnd"></param>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public List<MessageDetailEntity> MessageDetailSearchCondition(int[] Ids, DateTime AddtimeBegin, DateTime AddtimeEnd, int Page = 1, int PageCount = 1, bool isDescending = true)
        {
            var mDetail = new MessageDetailSearchCondition()
            {

                Page = Page,
                PageCount = PageCount,
                isDescending = isDescending,
                Ids = Ids,
                AddtimeBegin = AddtimeBegin,
                AddtimeEnd = AddtimeEnd
            };

            var test = _messageDetailService.GetMessageDetailsByCondition(mDetail);
            var temp = (from uu in test select uu).ToList();

            return temp;
        }
       
       
        ///// <summary>
        ///// 删除短信
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[System.Web.Http.HttpGet]
        //public bool MessageDetailDelete(int id = 0)
        //{
        //    var mDetailDel = new MessageDetailEntity()
        //    {
        //        Id = id
        //    };
        //    return _messageDetailService.Delete(mDetailDel);

        //}
      
        
        /// <summary>
        /// /发送短信
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="content"></param>
        /// <param name="sender"></param>
        /// <param name="addtime"></param>
        /// <param name="mobile"></param>
        [System.Web.Http.HttpGet]
        public void MessageDetailCreate(string Title, string content, string sender, string mobile)
        {
            var MessageDetailInsert = new MessageDetailEntity()
            {
                Title = Title,
                Content = content,
                Sender = sender,
                Mobile = mobile

            };
            _messageDetailService.Create(MessageDetailInsert);
            //if ( !=null)
            //{

            //}



        }
       /// <summary>
       /// 查询短信配置
       /// </summary>
       /// <param name="Page">页码</param>
       /// <param name="PageCount">每页大小</param>
       /// <param name="isDescending">是否降序</param>
       /// <returns></returns>
        [System .Web .Http .HttpGet ]
        public List <MessageConfigEntity >MessageConfigSearchCondition(int Page = 1, int PageCount = 1, bool isDescending = true)
    {
        var mConfig = new MessageConfigSearchCondition()
        {
            Page = Page,
            PageCount = PageCount,
            isDescending = isDescending

        };
          var test=   _MessageConfigService .GetMessageConfigsByCondition (mConfig);
          var temp = (from uu in test select uu).ToList();
          return temp;
    }
      /// <summary>
      /// 添加短信配置模板
      /// </summary>
      /// <param name="Name">模板名称</param>
      /// <param name="Template">配置模板</param>
        [System .Web .Http .HttpGet ]
        public void MessageConfigCreate(string Name, string Template)
        {
            var MessageConfigInsert = new MessageConfigEntity()
            {
                Name = Name,
                Template = Template
            };
            _MessageConfigService.Create(MessageConfigInsert);
        }
      /// <summary>
      /// 删除短信配置
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        [System .Web .Http .HttpGet ]
        public bool MessageConfigDelete(int id=0)
        {
            var mConfigDel = new MessageConfigEntity()
            {
                Id = id
            };
            return _MessageConfigService.Delete(mConfigDel);
        }
        /// <summary>
        /// 跟新短信配置模板
        /// </summary>
        /// <param name="Name">模板名称</param>
        /// <param name="Template">配置模板</param>
        [System .Web .Http .HttpGet ]
        public void  MessageConfigUpdate(string Name,string Template)
        {
            var mConfigUpdate = new MessageConfigEntity()
            {
                Name = Name,
                Template = Template
            };
            _MessageConfigService.Update(mConfigUpdate);
        }
        #endregion

    }
}
