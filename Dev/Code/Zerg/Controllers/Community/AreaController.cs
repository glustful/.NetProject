using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Area;
using Community.Service.Area;
using Zerg.Models.Community;
using System;
using System.ComponentModel;
using Zerg.Common;
using System.Net.Http;
using System.Web.Http.Cors;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class CommunityAreaController : ApiController
    {
        private readonly IAreaService _areaService;
        [Description("Area��ʼ��������")]
        public CommunityAreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }
        /// <summary>
        /// ����ID��ѯ��Ϣ
        /// </summary>
        /// <param name="id">ID����</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            AreaModel model=null;
            if (id > 0)
            {
                var entity = _areaService.GetAreaById(id);
                if (entity != null)
                {
                    model = new AreaModel
                    {
                        Id = entity.Id,
                        Codeid = entity.CodeId,
                        Adddate = entity.AddDate,
                        Name = entity.Name,
                        //ParentName = entity.Parent.Name,
                        Parent = entity.Parent == null ? null: new AreaModel { Id = entity.Parent.Id, Adddate = entity.Parent.AddDate, Name = entity.Parent.Name },
                    };
                    if (model.Parent != null)
                    {
                        model.Parent = new AreaModel
                        {
                            Name = entity.Parent.Name,
                            Adddate = entity.Parent.AddDate,
                            Codeid = entity.Parent.CodeId,
                            Id = entity.Parent.Id,
                        };
                    }
                }
                else
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "���ݿ�û�д˼�¼��"));
            }
            return PageHelper.toJson(model);
        }
        /// <summary>
        /// ���ݲ�ͬ������ѯ��Ϣ
        /// </summary>
        /// <param name="condition">����,father ��true ��ȡһ��������father��false��fatherid��ֵ����0��ȡ�¼�����</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get([FromUri]AreaSearchCondition condition)
        {
            var models = _areaService.GetAreasByCondition(condition).ToList().Select(c => new AreaModel
            {
                Id = c.Id,
                Codeid = c.CodeId,
                Adddate = c.AddDate,
                Name = c.Name,
                //ParentName = c.Parent.Name,
                Parent = c.Parent==null?null:new AreaModel { Id=c.Parent.Id,Adddate=c.Parent.AddDate,Name = c.Parent.Name }
            }).ToList();
            var totalCount = _areaService.GetAreaCount(condition);
            return PageHelper.toJson(new { List = models, Condition = condition, TotalCount = totalCount });
        }
        public  HttpResponseMessage getAreaByFatherId(int id)
        {

            return null;
        }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="model">��Ϣ����</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]AreaModel model)
        {
            AreaEntity father = null;
            if (model.Parent != null && model.Parent.Id > 0)
            {
                father = _areaService.GetAreaById(model.Parent.Id);

            }
            var entity = new AreaEntity
            {
                CodeId = model.Codeid,
                AddDate = DateTime.Now,
                //Parent = model.Parent,
                Parent = father,
                Name = model.Name,
            };
            if (_areaService.Create(entity).Id > 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "��ӳɹ���"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "���ʧ�ܣ�"));
        }

        /// <summary>
        /// �޸���Ϣ
        /// </summary>
        /// <param name="model">�޸Ĳ���</param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage Put(AreaModel model)
        {
            AreaEntity entity = _areaService.GetAreaById(model.Id);
            if (entity == null)
               return PageHelper.toJson(PageHelper.ReturnValue(false, "���ݿ�û�д˼�¼��")); 

            if (model.Parent != null && model.Parent.Id != entity.Parent.Id)
            {
                var father = _areaService.GetAreaById(Convert.ToInt32( model.Parent.Id));
                entity.Parent = father;
            }

            entity.CodeId = model.Codeid;
            entity.AddDate = DateTime.Now;
            //var father = _areaService.GetAreasByCondition(new AreaSearchCondition { Name = model.ParentName }).FirstOrDefault();
            //entity.Parent = father;
            entity.Name = model.Name;
            if (_areaService.Update(entity) != null)
                return PageHelper.toJson(PageHelper.ReturnValue(true, "�޸ĳɹ���")); ;
                return PageHelper.toJson(PageHelper.ReturnValue(false, "�޸�ʧ�ܣ�"));

        }
        /// <summary>
        /// ����IDɾ����Ϣ
        /// </summary>
        /// <param name="id">ID����</param>
        /// <returns></returns>
        //public bool Delete(int id)
        //{
        //    AreaEntity entity =null ;
        //    if ( _areaService.GetAreaById(id)== null)
        //        return false;
        //    if (_areaService.Delete(entity))
        //        return true;
        //    return false;
        //}

        /// <summary>
        /// ����IDɾ����Ϣ
        /// </summary>
        /// <param name="id">ID����</param>
        /// <returns></returns>
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            AreaEntity entity = _areaService.GetAreaById(id);
            if (entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false,"���ݿ�û�д˼�¼��"));
            if (_areaService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "ɾ���ɹ���")); 
            return PageHelper.toJson(PageHelper.ReturnValue(false, "ɾ��ʧ�ܣ�"));
        }
    }
}