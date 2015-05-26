﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Trading.Entity.Model;
using Trading.Service.Product;
using Trading.Service.ProductBrand;
using Trading.Service.ProductDetail;
using Trading.Service.ProductParameter;
using Trading.Service.Classify;
using Trading.Service.Parameter;
using Trading.Service.ParameterValue;
using System.Web.Http;
using Zerg.Models.Trading.Product;
using Zerg.Common;
using System.Net;
using System.Web.Http.Cors;

namespace Zerg.Controllers.Trading.Product
{
    public class ClassifyController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IProductBrandService _productBrandService;
        private readonly IProductDetailService _productDetailService;
        private readonly IProductParameterService _productParameterService;
        private readonly IClassifyService _classifyService;
        private readonly IParameterService _parameterService;
        private readonly IParameterValueService _parameterValueService;
        /// <summary>
        /// 构造函数（操作函数注入）
        /// </summary>
        public ClassifyController(
            IProductService productService,
            IProductBrandService productBrandService,
            IProductDetailService productDetailService,
            IProductParameterService productParameterService,
            IClassifyService classifyService,
            IParameterValueService parameterValueService,
            IParameterService parameterService)
        {
            _productService = productService;
            _productBrandService = productBrandService;
            _productDetailService = productDetailService;
            _productParameterService = productParameterService;
            _classifyService = classifyService;
            _parameterService = parameterService;
            _parameterValueService = parameterValueService;
        }


        #region 商品分类管理
        /// <summary>
        /// 添加分类；
        /// </summary>
        /// <param name="classify">所要添加的分类</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string AddClassify([FromBody]ClassifyModel classify)
        {
            ClassifyEntity superCe = _classifyService.GetClassifyById(classify.ClassifyId);
            int sort = 0;
            if (superCe != null)//有上级分类则次级排序加1；
            {
                sort = superCe.Sort + 1;
            }
            ClassifyEntity ce = new ClassifyEntity()
            {
                Classify = superCe,
                Name = classify.Name,
                Sort = sort,
                Addtime = DateTime.Now,
                Adduser = classify.Adduser,
                Updtime = DateTime.Now,
                Upduser = classify.Upduser

            };
            try
            {
                _classifyService.Create(ce);
                return "添加分类成功";
            }
            catch (Exception error)
            {
                return "添加分类失败";
            }
        }
        /// <summary>
        /// 获取所有分类(使用Angular中Tree的数据格式)；
        /// </summary>
        /// <param name="pageindex">当前翻页页数</param>
        /// <returns>查询结果</returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetAllClassify()
        {
            ClassifySearchCondition csc = new ClassifySearchCondition()
            {
                OrderBy = EnumClassifySearchOrderBy.OrderById
            };
            return PageHelper.toJson(GetAllTree().ToList());
        }
        /// <summary>
        /// 根据Id查名称
        /// </summary>
        /// <param name="classifyId">分类id</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string GetClassifyNameById(int classifyId)
        {
            try
            {
                ClassifyEntity ce=_classifyService.GetClassifyById(classifyId);

                return ce.Name;
            }
            catch (Exception e)
            {
                return "获取失败";
            }
        }
        /// <summary>
        /// 获取某根节点下的树状图数据（使用AngluarJs是里的Tree数据格式）；
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetNextNodesById(int nodeId)
        {
            try
            {
                List<TreeJsonModel> treeJsonModelBuffer = new List<TreeJsonModel>();
                ClassifyEntity ce = _classifyService.GetClassifyById(nodeId);
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.Name,
                    Id = ce.Id
                };

                TJM.children = GetJsonFromTreeModel(TJM.Id);
                foreach (var ceJson in TJM.children)
                {
                    treeJsonModelBuffer.Add(ceJson);
                }
                return PageHelper.toJson(treeJsonModelBuffer.ToList());
            }
            catch (Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent("获取失败");    // 响应内容
                return response;
            }

        }
        /// <summary>
        /// 删除分类；
        /// </summary>
        /// <param name="classifyId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string DelClassify(int classifyId)
        {
            try{
                if (_classifyService.Delete(_classifyService.GetClassifyById(classifyId)))
                {
                    return "删除成功";
                }
                else {
                    return "删除失败，此分类下有子分类或商品，您不能删除此分类！";
                }
              
            }catch(Exception e){
                return "删除失败";
            }
        }
        /// <summary>
        /// 获取分类下的次级分类；
        /// </summary>
        /// <param name="classifyId">上一级的分类id</param>
        /// <param name="pageindex">查询的页数</param>
        /// <returns>查询结果</returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetSubClassify(int classifyId, int pageindex)
        {
            return PageHelper.toJson(_classifyService.GetClassifysBySuperClassify(classifyId).ToList());
        }

        /// <summary>
        /// 查询该分类下的末级分类（在此分类下才可添加商品）；
        /// </summary>
        /// <param name="classifyId"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetActivityClassify(int classifyId)
        {
            return PageHelper.toJson(GetTreeAllEndPoints(classifyId).ToList());
        }

        #endregion

        #region 商品分类参数管理
        /// <summary>
        /// 添加分类参数；
        /// </summary>
        /// <param name="pageindex">当前翻页页数</param>
        /// <returns>查询结果</returns>
        [System.Web.Http.HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string AddParameter([FromBody]ParameterModel parameter)
        {
            ClassifyEntity ce = _classifyService.GetClassifyById(parameter.ClassifyId);
            ParameterEntity pe = new ParameterEntity()
            {
                Upduser = parameter.Upduser,
                Updtime = DateTime.Now,
                Sort = parameter.Sort,
                Name = parameter.Name,
                Classify = ce,
                Adduser = parameter.Adduser,
                Addtime = DateTime.Now,
            };
            try
            {
                _parameterService.Create(pe);
                return "添加参数" + pe.Name + "成功";
            }
            catch (Exception e)
            {
                return "添加参数" + pe.Name + "失败";
            }
        }
        /// <summary>
        /// 添加分类参数值；
        /// </summary>
        /// <param name="pageindex">当前翻页页数</param>
        /// <returns>查询结果</returns>
        [System.Web.Http.HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string AddParameterValue([FromBody]ParameterValueModel parameterValueModel)
        {
            ParameterEntity pe = _parameterService.GetParameterById(parameterValueModel.ParameterId);
            ParameterValueEntity pev = new ParameterValueEntity()
            {
                Upduser = parameterValueModel.Upduser,
                Updtime = DateTime.Now,
                Sort = parameterValueModel.Sort,
                Parametervalue = parameterValueModel.Parametervalue,
                Parameter = pe,
                Adduser = parameterValueModel.Adduser,
                Addtime = DateTime.Now,
            };
            try
            {
                _parameterValueService.Create(pev);
                return "添加参数值" + pev.Parametervalue + "成功";
            }
            catch (Exception e)
            {
                return "添加参数值" + pev.Parametervalue + "失败";
            }
        }

        /// <summary>
        /// 添加分类参数值；
        /// </summary>
        /// <param name="pageindex">当前翻页页数</param>
        /// <returns>查询结果</returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string AddProductParameterVaule(int parameterValueId, int productId)
        {
            try
            {
                ProductEntity PE = _productService.GetProductById(productId);
                ParameterValueEntity PVE = _parameterValueService.GetParameterValueById(parameterValueId);
                ParameterEntity ParE = PVE.Parameter;
                ProductParameterEntity PPE = new ProductParameterEntity()
                {
                    Addtime = DateTime.Now,
                    Adduser = PE.Adduser,
                    Parameter = ParE,
                    ParameterValue = PVE,
                    Product = PE,
                    Sort = 0,
                    Updtime = DateTime.Now,
                    Upduser = PE.Upduser
                };
                _productParameterService.Create(PPE);
                return "绑定商品属性值成功";
            }catch(Exception e){
                return "绑定商品属性值失败";
            }
          }
        /// <summary>
        /// 删除参数；
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string DelParameter(int parameterId)
        {
            ParameterEntity pe = _parameterService.GetParameterById(parameterId);
            try
            {
                List<ParameterValueEntity> parList = _parameterValueService.GetParameterValuesByParameter(parameterId).ToList();
                foreach (var parameter in parList)
                {//先把该参数下的所有参数值删除；
                    _parameterValueService.Delete(parameter);
                }
                _parameterService.Delete(pe);//删除该参数；
                return "删除参数成功";
            }
            catch (Exception e)
            {
                return "删除参数失败";
            }
        }
        /// <summary>
        /// 删除参数；
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string DelParameterValue(int parameterValueId)
        {
            ParameterValueEntity pve = _parameterValueService.GetParameterValueById(parameterValueId);
            try
            {
                _parameterValueService.Delete(pve);
                return "删除参数值成功";
            }
            catch (Exception e)
            {
                return "删除参数值失败";
            }
        }

        /// <summary>
        /// 根据分类查询分类参数列表；
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetParameterByClassify(int classifyId)
        {
            return PageHelper.toJson(_parameterService.GetParameterEntitysByClassifyId(classifyId).ToList());
        }

        /// <summary>
        /// 根据分类查参数值列表；
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetParameterValueByParameter(int parameterId)
        {
            return PageHelper.toJson(_parameterValueService.GetParameterValuesByParameter(parameterId).ToList());
        }
        #endregion

        #region 公用方法
        List<ClassifyEntity> _CEList = new List<ClassifyEntity>();
        /// <summary>
        /// 获取分类树枝下的每个终节点；
        /// </summary>
        /// <param name="ClassfiyId"></param>
        /// <returns></returns>
        public List<ClassifyEntity> GetTreeAllEndPoints(int ClassfiyId)
        {
            _CEList.Clear();
            RecursionTree(ClassfiyId);// 递归遍历树状节点,并找出末端节点；
            return _CEList;
        }
        /// <summary>
        /// 递归遍历树状节点,并找出末端节点；
        /// </summary>
        /// <param name="nodeId"></param>
        public void RecursionTree(int nodeId)
        {
            List<ClassifyEntity> CEList = _classifyService.GetClassifysBySuperClassify(nodeId).ToList<ClassifyEntity>();
            foreach (var ce in CEList)
            {//遍历；
                RecursionTree(ce.Id);//递归；
            }
            if (CEList.Count == 0)//若无子节点，说明已经探底；
            {
                _CEList.Add(_classifyService.GetClassifyById(nodeId));//记录下末端节点；
            }
        }
        #endregion
       
        #region 获取支持angularjs的tree模型列表；
        /// <summary>
        /// 支持angularjs中tree插件的模型；
        /// </summary>
        public class TreeJsonModel
        {
            public string label { set; get; }
            public List<TreeJsonModel> children { set; get; }
            public int Id { set; get; }
        }


        /// <summary>
        /// 自上而下获取树状根节点列表；
        /// </summary>
        /// <returns></returns>
        public List<TreeJsonModel> GetAllTree()
        {
            ClassifySearchCondition csc = new ClassifySearchCondition()
            {
                OrderBy = EnumClassifySearchOrderBy.OrderById
            };
            List<ClassifyEntity> ceListBuffer = new List<ClassifyEntity>();
            List<TreeJsonModel> treeJsonModelBuffer = new List<TreeJsonModel>();
            List<ClassifyEntity> ceList = _classifyService.GetClassifysByCondition(csc).ToList();
            foreach (var ce in ceList)
            {
                if (ce.Classify == null)
                {
                    ceListBuffer.Add(ce);//查找第一级；
                }
            }
            foreach (var ce in ceListBuffer)
            {
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.Name,
                    Id = ce.Id
                };
                treeJsonModelBuffer.Add(TJM);
                TJM.children = GetJsonFromTreeModel(TJM.Id);
            }
            return treeJsonModelBuffer;
        }

        /// <summary>
        /// 自迭代获取所有树子节点；
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public List<TreeJsonModel> GetJsonFromTreeModel(int nodeId)
        {
            ClassifySearchCondition csc = new ClassifySearchCondition()
            {
                OrderBy = EnumClassifySearchOrderBy.OrderById
            };
            List<TreeJsonModel> datalist = new List<TreeJsonModel>();
            List<ClassifyEntity> ceList = _classifyService.GetClassifysBySuperClassify(nodeId).ToList();//找出该级的子集；
            foreach (var ce in ceList)
            {
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.Name,
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
        #endregion

        #region 获取树状json格式的分类参数
        /// <summary>
        /// 获取树状json格式的分类参数
        /// </summary>
        /// <param name="classifyId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetParameterTreeData(int classifyId)
        {

                List<ParameterTreeModel> PTMList = new List<ParameterTreeModel>();
               var PList= _parameterService.GetParameterEntitysByClassifyId(classifyId);
                foreach(var p in PList){
                    ParameterTreeModel PT = new ParameterTreeModel()
                    {
                        Name = p.Name,
                        Id = p.Id
                    };
                    List<ParameterValueEntity> PVList=_parameterValueService.GetParameterValuesByParameter(p.Id).ToList();
                    List<ParameterValueTreeModel> PVTMList=new List<ParameterValueTreeModel>();
                    foreach(var pv in PVList){
                        ParameterValueTreeModel PVTM=new ParameterValueTreeModel(){
                            Value=pv.Parametervalue,
                            Id=pv.Id
                        };
                        PVTMList.Add(PVTM);
                    };
                    PT.ValueList = PVTMList;
                    PTMList.Add(PT);
                }
                return PageHelper.toJson(PTMList.ToList());
  
        }
        /// <summary>
        /// 分类模型
        /// </summary>
        public class ParameterTreeModel
        {
            public string Name { set;get;}//分类名称；
            public int Id{set;get;}//分类Id；
            public List<ParameterValueTreeModel> ValueList { set; get; }//分类参数值；
        }
        /// <summary>
        /// 分类值模型
        /// </summary>
        public class ParameterValueTreeModel
        {
            public string Value { set; get; }//分类值名称；
            public int Id { set; get; }//分类值Id；
        }
        #endregion
    }
}