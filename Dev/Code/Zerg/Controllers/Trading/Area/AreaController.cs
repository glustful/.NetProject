using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using Trading.Entity.Entity.Area;
using Trading.Entity.Model;
using Trading.Service.Area;
using Zerg.Common;
using Zerg.Models.Trading.Area;
using Zerg.Models.Trading.Product;
using System.ComponentModel;

namespace Zerg.Controllers.Trading.Area
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("区域字典管理类（包括设置商品所在地）")]
    public class AreaController : ApiController
    {
        private readonly IAreaService _areaService;
        /// <summary>
        /// 区域字典初始化
        /// </summary>
        /// <param name="areaService"></param>
        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }
        /// <summary>
        /// 获取所有的区域分类
        /// </summary>
        /// <returns>区域分类</returns>
        [Description("获取所有的区域分类")]
        public HttpResponseMessage GetAllClassify()
        {
            AreaSearchCondition csc = new AreaSearchCondition()
            {
                OrderBy = EnumAreaSearchOrderBy.OrderById
            };
            return PageHelper.toJson(GetAllTree().ToList());
        }
        /// <summary>
        /// 获取区域树
        /// </summary>
        /// <returns>返回区域列表</returns>
        [Description("获取区域列表")]
        public List<TreeJsonModel> GetAllTree()
        {
            AreaSearchCondition csc = new AreaSearchCondition()
            {
                OrderBy = EnumAreaSearchOrderBy.OrderById
            };
            List<AreaEntity> ceListBuffer = new List<AreaEntity>();
            List<TreeJsonModel> treeJsonModelBuffer = new List<TreeJsonModel>();
            List<AreaEntity> ceList = _areaService.GetAreaByCondition(csc).ToList();
            foreach (var ce in ceList)
            {
                if (ce.ParentId == 0)
                {
                    ceListBuffer.Add(ce);//查找第一级；
                }
            }
            foreach (var ce in ceListBuffer)
            {
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.AreaName,
                    Id = ce.Id
                };
                treeJsonModelBuffer.Add(TJM);
                TJM.children = GetJsonFromTreeModel(TJM.Id);
            }
            return treeJsonModelBuffer;
        }
        /// <summary>
        /// 从区域树上获取节点列表
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <returns>节点列表</returns>
        [Description("获取区域树节点列表")]
        public List<TreeJsonModel> GetJsonFromTreeModel(int nodeId)
        {
            AreaSearchCondition csc = new AreaSearchCondition()
            {
                OrderBy = EnumAreaSearchOrderBy.OrderById
            };
            List<TreeJsonModel> datalist = new List<TreeJsonModel>();
            List<AreaEntity> ceList = _areaService.GetBySuperArea(nodeId).ToList();//找出该级的子集；
            foreach (var ce in ceList)
            {
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.AreaName,
                    Id = ce.Id
                };
                datalist.Add(TJM);
                TJM.children = GetJsonFromTreeModel(TJM.Id);//自迭代;

            }
            if (ceList.Count == 0)//若遍历到末端，则：
            {
                return null;
            }
            else
            {
                return datalist;
            }
        }
        public class TreeJsonModel
        {
            public string label { set; get; }
            public List<TreeJsonModel> children { set; get; }
            public int Id { set; get; }
        }

        //public HttpResponseMessage GetArea(int parentId=0)
        //{
        //    var areaCon = new AreaSearchCondition
        //    {
        //        ParentId = parentId
        //    };
        //    var areaList = _areaService.GetAreaByCondition(areaCon).Select(a=>new
        //    {
        //        Id=a.Id,
        //        AreaName=a.AreaName
        //    }).ToList();
        //    return PageHelper.toJson(areaList);
        //}
        /// <summary>
        /// 传入区域参数，添加区域，返回添加结果状态信息
        /// </summary>
        /// <param name="model">区域参数</param>
        /// <returns>添加区域结果状态信息</returns>
        [Description("添加区域")]
        [HttpPost]
        public HttpResponseMessage AddArea([FromBody]AreaModel model)
        {
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(model.AreaName);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
            }
            else
            {
                AreaEntity fatherArea = _areaService.GetAreaById(model.Id);
                int Level = 1;
                int parentId = 0;
                if (fatherArea != null) //有上级分类则次级排序加1；
                {
                    Level = fatherArea.Level + 1;
                    parentId = fatherArea.Id;
                }
                AreaEntity ce = new AreaEntity()
                {
                    AreaName = model.AreaName,
                    Level = Level,
                    ParentId = parentId
                };
                try
                {
                    _areaService.Create(ce);
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功！"));
                }
                catch (Exception error)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败！"));
                    ;
                }
            }
        }

        /// <summary>
        /// 传入区域id，删除区域，返回删除结果状态信息
        /// </summary>
        /// <param name="id">区域id</param>
        /// <returns>删除区域信息结果状态信息</returns>
        [Description("删除区域")]
        [HttpGet]
        public HttpResponseMessage Delete(int id)
        {
            var area = _areaService.GetAreaById(id);
            var SubArea = _areaService.GetBySuperArea(area.Id);
            if (SubArea.Count() == 0)
            {
                _areaService.Delete(area);
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "存在关联不能删除！"));
        }
    }
}
