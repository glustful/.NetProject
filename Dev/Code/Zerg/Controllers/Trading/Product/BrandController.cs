using System;
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
using Trading.Service.BrandParameter;
using System.Web.Http;
using Zerg.Models.Trading.Product;
using Zerg.Common;
using System.Web.Http.Cors;

namespace Zerg.Controllers.Trading.Product
{
    [System.Web.Http.AllowAnonymous]
    public class BrandController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IProductBrandService _productBrandService;
        private readonly IProductDetailService _productDetailService;
        private readonly IProductParameterService _productParameterService;
        private readonly IClassifyService _classifyService;
        private readonly IBrandParameterService _brandParameterService;
        /// <summary>
        /// 构造函数（操作函数注入）
        /// </summary>
        public BrandController(
            IProductService productService,
            IProductBrandService productBrandService,
            IProductDetailService productDetailService,
            IProductParameterService productParameterService,
            IClassifyService classifyService,
            IBrandParameterService brandParameterService)
        {
            _productService = productService;
            _productBrandService = productBrandService;
            _productDetailService = productDetailService;
            _productParameterService = productParameterService;
            _classifyService = classifyService;
            _brandParameterService = brandParameterService;
        }
        #region 商品项目管理
        /// <summary>
        /// 添加品牌项目
        /// </summary>
        /// <param name="productBrandModel">品牌项目和数据模型</param>
        /// <returns>添加结果</returns>
        [System.Web.Http.HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string AddProductBrand([FromBody]ProductBrandModel productBrandModel)
        {
            ProductBrandEntity PBE = new ProductBrandEntity()
            {
                Addtime = DateTime.Now,
                Adduser = productBrandModel.Adduser,
                Bimg = productBrandModel.Bimg,
                Bname = productBrandModel.Bname,
                Updtime = DateTime.Now,
                Upduser = productBrandModel.Upduser,
            };
            try
            {
                _productBrandService.Create(PBE);
                return "添加品牌项目" + PBE.Bname + "成功";
            }
            catch (Exception e)
            {
                return "添加品牌项目" + PBE.Bname + "失败";
            }
        }
        /// <summary>
        /// 删除品牌；
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string DelBrandById(int brandId)
        {
            try
            {
                if(_productBrandService.Delete(_productBrandService.GetProductBrandById(brandId))){
                    return "删除品牌成功";
                }else{
                      return "删除品牌失败，该品牌可能有商品已被添加";
                }
            }catch(Exception e){
                  return "删除品牌失败";
            }
            
        } 
        /// <summary>
        /// 添加品牌项目参数
        /// </summary>
        /// <param name="productBrandModel">品牌项目参数数据模型</param>
        /// <returns>添加结果</returns>
        [System.Web.Http.HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string AddProductBrandParameter([FromBody]ProductBrandParameterModel productBrandParameterModel)
        {
            ProductBrandEntity PBE = _productBrandService.GetProductBrandById(productBrandParameterModel.ProductBrandId);
            BrandParameterEntity BPE = new BrandParameterEntity()
             {
                 Addtime = DateTime.Now,
                 Adduser = productBrandParameterModel.Adduser,
                 Updtime = DateTime.Now,
                 Upduser = productBrandParameterModel.Upduser,
                 Parametername = productBrandParameterModel.Parametername,
                 Parametervaule = productBrandParameterModel.Parametervaule,
                 ProductBrand = PBE
             };
            try
            {
                _brandParameterService.Create(BPE);
                return "添加品牌项目" + PBE.Bname + "成功";
            }
            catch (Exception e)
            {
                return "添加品牌项目" + PBE.Bname + "失败";
            }
        }
        /// <summary>
        /// 删除商品参数值
        /// </summary>
        /// <param name="brandParameterId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string DelBrandParameter(int brandParameterId)
        {
            try
            {
                if (_brandParameterService.Delete(_brandParameterService.GetBrandParameterById(brandParameterId)))
                {
                    return "删除成功";
                }else{
                    return "无法删除，可能该项目下有商品";
                }
            }
            catch (Exception e)
            {
                 return "无法删除";
            }
        }
        /// <summary>
        /// 获取所有品牌；
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetAllBrand()
        {
            ProductBrandSearchCondition PBSC = new ProductBrandSearchCondition()
            {
                OrderBy = EnumProductBrandSearchOrderBy.OrderById
            };
            var brandList = _productBrandService.GetProductBrandsByCondition(PBSC).Select(a => new
            {
                a.Id,
                a.Bimg,
                a.Bname,
                a.SubTitle,
                a.Content,
                ProductPramater = a.ParameterEntities.Select(p => new { p.Parametername,p.Parametervaule})
            }).ToList();
            return PageHelper.toJson(brandList);
            //return PageHelper.toJson(_productBrandService.GetProductBrandsByCondition(PBSC).ToList());
        }
        /// <summary>
        /// 根据条件查询品牌
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage SearchBrand(string condition,int page,int pageCount)
        {
            ProductBrandSearchCondition bcon = new ProductBrandSearchCondition
            {
                Bname = condition,
                Page = page,
                PageCount = pageCount
            };
            var brandList = _productBrandService.GetProductBrandsByCondition(bcon).Select(a => new
            {
                a.Id,
                a.Bimg,
                a.Bname,
                a.SubTitle,
                a.Content,
                ProductPramater = a.ParameterEntities.Select(p => new { p.Parametername, p.Parametervaule })
            }).ToList();
            var count = _productBrandService.GetProductBrandCount(bcon);
            return PageHelper.toJson(new {List=brandList,Count=count});
        }

        /// <summary>
        /// 根据品牌id获取项目参数；
        /// </summary>
        /// <param name="ProductBrandId"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetBrandParameterByBrand(int ProductBrandId)
        {
            return PageHelper.toJson(_brandParameterService.GetBrandParametersByBrandId(ProductBrandId).ToList());
        }
        #endregion
    }
}