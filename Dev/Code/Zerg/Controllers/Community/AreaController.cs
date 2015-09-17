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
        [Description("Area初始化构造器")]
        public CommunityAreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }
        /// <summary>
        /// 根据ID查询信息
        /// </summary>
        /// <param name="id">ID参数</param>
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
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据库没有此记录！"));
            }
            return PageHelper.toJson(model);
        }
        /// <summary>
        /// 根据不同条件查询信息
        /// </summary>
        /// <param name="condition">条件,father 传true 获取一级地区，father传false跟fatherid的值大于0获取下级地区</param>
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
        /// 添加信息
        /// </summary>
        /// <param name="model">信息参数</param>
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
                return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败！"));
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="model">修改参数</param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage Put(AreaModel model)
        {
            AreaEntity entity = _areaService.GetAreaById(model.Id);
            if (entity == null)
               return PageHelper.toJson(PageHelper.ReturnValue(false, "数据库没有此记录！")); 

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
                return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功！")); ;
                return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败！"));

        }
        /// <summary>
        /// 根据ID删除信息
        /// </summary>
        /// <param name="id">ID参数</param>
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
        /// 根据ID删除信息
        /// </summary>
        /// <param name="id">ID参数</param>
        /// <returns></returns>
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            AreaEntity entity = _areaService.GetAreaById(id);
            if (entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false,"数据库没有此记录！"));
            if (_areaService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功！")); 
            return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败！"));
        }
    }
}