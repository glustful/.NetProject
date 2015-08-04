using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using Trading.Entity.Model;
using Trading.Service.BrandParameter;
using Trading.Service.Classify;
using Trading.Service.Product;
using Trading.Service.ProductBrand;
using Trading.Service.ProductDetail;
using Trading.Service.ProductParameter;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.Trading.Product;

namespace Zerg.Controllers.Trading.Product
{
    [AllowAnonymous]
    [Description("品牌管理类")]
    public class BrandController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IProductBrandService _productBrandService;
        private readonly IProductDetailService _productDetailService;
        private readonly IProductParameterService _productParameterService;
        private readonly IClassifyService _classifyService;
        private readonly IBrandParameterService _brandParameterService;
        private readonly IWorkContext _workContext;
        /// <summary>
        /// 构造函数（操作函数注入）
        /// </summary>

        public BrandController(
            IProductService productService,
            IProductBrandService productBrandService,
            IProductDetailService productDetailService,
            IProductParameterService productParameterService,
            IClassifyService classifyService,
            IBrandParameterService brandParameterService,
            IWorkContext workContext)
        {
            _productService = productService;
            _productBrandService = productBrandService;
            _productDetailService = productDetailService;
            _productParameterService = productParameterService;
            _classifyService = classifyService;
            _brandParameterService = brandParameterService;
            _workContext = workContext;
        }
        #region 商品项目管理
        /// <summary>
        /// 添加品牌项目
        /// </summary>
        /// <param name="productBrandModel">品牌项目和数据模型</param>
        /// <returns>添加结果</returns>
        [Description("添加商品品牌")]
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage AddProductBrand([FromBody]ProductBrandModel productBrandModel)
        {
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(productBrandModel.Bname);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
            }
            else
            {
                ProductBrandEntity pbe = new ProductBrandEntity()
                {
                    Addtime = DateTime.Now,
                    Adduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture),
                    Bimg = productBrandModel.Bimg,
                    Bname = productBrandModel.Bname,
                    Updtime = DateTime.Now,
                    Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture),
                    SubTitle = productBrandModel.SubTitle,
                    Content = productBrandModel.Content,
                    AdTitle = productBrandModel.AdTitle,
                    ClassId = productBrandModel.ClassId

                };

                try
                {
                    _productBrandService.Create(pbe);
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功！"));
                }
                catch (Exception)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "不能添加自身！"));
                }
            }
        }
        /// <summary>
        /// 修改品牌项目
        /// </summary>
        /// <param name="productBrandModel">品牌项目和数据模型</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage UpProductBrand(ProductBrandModel productBrandModel)
        {
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(productBrandModel.Bname);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
            }
            else
            {
                var pb = _productBrandService.GetProductBrandById(productBrandModel.Id);
                pb.Updtime = DateTime.Now;
                pb.Bname = productBrandModel.Bname;
                pb.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                pb.SubTitle = productBrandModel.SubTitle;
                pb.Content = productBrandModel.Content;
                pb.Bimg = productBrandModel.Bimg;
                pb.AdTitle = productBrandModel.AdTitle;
                pb.ClassId = productBrandModel.ClassId;
                if (_productBrandService.Update(pb) != null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据修改成功！"));
                }

                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                }
            }
        }
        /// <summary>
        /// 删除品牌；
        /// </summary>
        /// <param name="brandId">品牌ID</param>
        /// <returns>删除结果</returns>
        [Description("删除商品品牌")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage DelBrandById(int brandId)
        {
            try
            {
                if (_productBrandService.Delete(_productBrandService.GetProductBrandById(brandId)))
                {

                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "删除品牌失败，该品牌可能有商品已被添加"));
                }
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
            }

        }

        /// <summary>
        /// 添加品牌项目参数
        /// </summary>
        /// <returns>添加结果</returns>
        [Description("添加品牌项目参数")]
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage AddProductBrandParameter([FromBody]ProductBrandParameterModel productBrandParameterModel)
        {

            ProductBrandEntity pbe = _productBrandService.GetProductBrandById(productBrandParameterModel.ProductBrandId);

            var productPramater = pbe.ParameterEntities.Select(p => new { p.Parametername }).Select(pp => new
            {
                pp.Parametername
            }).ToList();
            foreach (var i in productPramater)
            {
                if (i.Parametername == productBrandParameterModel.Parametername)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据已经存在，请重新编辑！"));
                }
            }

            BrandParameterEntity bpe = new BrandParameterEntity()
             {

                 Addtime = DateTime.Now,
                 Adduser = productBrandParameterModel.Adduser,
                 Updtime = DateTime.Now,
                 Upduser = productBrandParameterModel.Upduser,
                 Parametername = productBrandParameterModel.Parametername,
                 Parametervaule = productBrandParameterModel.Parametervaule,
                 ProductBrand = pbe
             };
            try
            {
                _brandParameterService.Create(bpe);
                return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功！"));
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败！"));
            }
        }

        /// <summary>
        /// 删除商品参数值
        /// </summary>
        /// <param name="brandParameterId">商品参数ID</param>
        /// <returns>删除结果</returns>
        [Description("删除商品参数值")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage DelBrandParameter(int brandParameterId)
        {
            try
            {
                if (_brandParameterService.Delete(_brandParameterService.GetBrandParameterById(brandParameterId)))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功！"));

                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "无法删除，可能该项目下有商品"));
                }
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
            }
        }

        /// <summary>
        /// 获取所有品牌，返回品牌列表
        /// </summary>
        /// <param name="className">分类 （不传值查询所有）</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>品牌列表</returns>
        [Description("获取所有品牌，返回品牌列表")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetAllBrand(int page = 1, int pageSize = 10,string className=null)
        {
            var con=new ClassifySearchCondition()
            {
                Name = className
            };
            var classname=_classifyService.GetClassifysByCondition(con).FirstOrDefault();

            //参数className 不传值则默认查询所有品牌
            if (string.IsNullOrEmpty(className))
            {
                classname = null;
            }


            var sech = new ProductBrandSearchCondition
            {
                //=========================yangyue 2015/7/7 start=====================================================
                IsDescending = true,
                OrderBy = EnumProductBrandSearchOrderBy.OrderByAddtime,
                Classify=classname,
                //========================  end   ====================================================================
                Page = page,
                PageCount = pageSize

            };
            // var list = _productBrandService.GetProductBrandsByCondition(sech).Select(a => new
            //{
            //    Page = page,
            //    PageCount = pageSize,
            //});
            //取出所有品牌
            var list = _productBrandService.GetProductBrandsByCondition(sech).Select(a => new

           {
               a.Id,
               a.Bimg,
               a.Bname,
               a.SubTitle,
               a.Content,
               a.Addtime,
               a.AdTitle,
               ProductParamater = a.ParameterEntities.Select(p => new { p.Parametername, p.Parametervaule })
           }).ToList().Select(b => new
            {
                b.Id,
                b.Bimg,
                b.Bname,
                b.SubTitle,
                b.Content,
                b.AdTitle,
                ProductParamater = b.ProductParamater.ToDictionary(k => k.Parametername, v => v.Parametervaule),
                b.Addtime

            });

            var totalCount1 = _productBrandService.GetProductBrandCount(sech);


            return PageHelper.toJson(new { List = list, Condition = sech, totalCount = totalCount1 });

            //var totalCount = _productBrandService.GetProductBrandCount(Brandcondition);

            //var BrandList = _productBrandService.GetProductBrandsByCondition(Brandcondition).Select(a => new
            //{
            //    a.Id,
            //    a.Bimg,
            //    a.Bname,
            //    a.SubTitle,
            //    a.Content,
            //    ProductParamater = a.ParameterEntities.Select(p => new { p.Parametername, p.Parametervaule })
            //}).ToList();
            //return PageHelper.toJson(new { brandList = BrandList.Select(b => new { 
            //    b.Id,
            //    b.Bimg,
            //    b.Bname,
            //    b.SubTitle,
            //    b.Content,
            //    ProductParamater=b.ProductParamater.ToDictionary(k=>k.Parametername,v=>v.Parametervaule)
            //}), totalCount = totalCount });
            ////return PageHelper.toJson(_productBrandService.GetProductBrandsByCondition(PBSC).ToList());
        }

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <returns>品牌列表</returns>
        [Description("获取所有品牌，返回品牌列表")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetBrandList()
        {
            ProductBrandSearchCondition brandcondition = new ProductBrandSearchCondition
            {
                IsDescending = true,
                OrderBy = EnumProductBrandSearchOrderBy.OrderById
            };
            var brandList = _productBrandService.GetProductBrandsByCondition(brandcondition).Select(a => new
            {
                a.Id,
                a.Bname,
            }).ToList();
            return PageHelper.toJson(brandList);
        }
        #region 彭贵飞 获取品牌详细信息
        /// <summary>
        /// 获取品牌详细信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetBrandDetail(int brandId)
        {
            var brand = _productBrandService.GetProductBrandById(brandId);
            if(brand==null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "品牌不存在"));
            }
            var model = new ProductBrandModel
            {   
                Id = brand.Id,               
                Bname = brand.Bname,
                SubTitle = brand.SubTitle,                
                Content = brand.Content,
                Parameters = brand.ParameterEntities.ToDictionary(k => k.Parametername, v => v.Parametervaule)
            };
            return PageHelper.toJson(model);
        }
        #endregion
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetByBrandId(int brandId)
        {
            var brand = _productBrandService.GetProductBrandById(brandId);
            var model = new ProductBrandModel
            {
                Addtime = brand.Addtime,
                Adduser = brand.Adduser,
                Bimg = brand.Bimg,
                Bname = brand.Bname,
                SubTitle = brand.SubTitle,
                Id = brand.Id,
                Content = brand.Content,
                AdTitle = brand.AdTitle,
               ClassId = brand.ClassId.HasValue?brand.ClassId.Value:0,

                //Parameters = Brand.ParameterEntities.Select(p => new ProductBrandParameterModel
                //{
                //    Parametername = p.Parametername,
                //    Parametervaule = p.Parametervaule
                //}).ToList()
                Parameters = brand.ParameterEntities.ToDictionary(k => k.Parametername, v => v.Parametervaule)
            };
            var products = _productService.GetProductsByProductBrand(brand.Id);
            model.Products = products.Select(p => new ProductModel
            {
                Productname = p.Productname,
                Id = p.Id,
                Productimg = p.Productimg
            }).ToList();
            return PageHelper.toJson(model);
        }

        /// <summary>
        /// 根据条件查询品牌
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="page">页面</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>品牌列表</returns>
        [Description("传入查询参数，返回品牌列表")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage SearchBrand(string condition, int page, int pageCount,string className)
        {
            var con = new ClassifySearchCondition()
            {
                Name = className
            };
            var  classify= _classifyService.GetClassifysByCondition(con).FirstOrDefault();
            ProductBrandSearchCondition bcon = new ProductBrandSearchCondition
            {
                Bname = condition,
                Page = page,
                PageCount = pageCount,
                OrderBy = EnumProductBrandSearchOrderBy.OrderByAddtime,
                IsDescending = true,
                Classify =classify 
            };
            var brandList = _productBrandService.GetProductBrandsByCondition(bcon).Select(a => new
            {
                a.Id,
                a.Bimg,
                a.Bname,
                a.SubTitle,
                a.Content,
                a.AdTitle,
                ProductPramater = a.ParameterEntities.Select(p => new { p.Parametername, p.Parametervaule })
            }).ToList();
            var count = _productBrandService.GetProductBrandCount(bcon);
            return PageHelper.toJson(new
            {
                List = brandList.Select(c => new
                {
                    c.Id,
                    c.Bimg,
                    c.Bname,
                    c.SubTitle,
                    c.Content,
                    c.AdTitle,
                    ProductParamater = c.ProductPramater.ToDictionary(k => k.Parametername, v => v.Parametervaule)
                }),
                Count = count
            });
            //return PageHelper.toJson(new {List=brandList,Count=count});
        }

        /// <summary>
        /// 传入品牌ID，返回品牌参数
        /// </summary>
        /// <param name="productBrandId">品牌ID</param>
        /// <returns>品牌参数</returns>
        [Description("传入品牌ID，返回品牌参数")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetBrandParameterByBrand(int productBrandId)
        {
            var brandParameter = _brandParameterService.GetBrandParametersByBrandId(productBrandId).Select(p => new
            {
                p.Id,
                p.Parametername,
                p.Parametervaule
            }).ToList();
            return PageHelper.toJson(brandParameter);
            // return PageHelper.toJson(_brandParameterService.GetBrandParametersByBrandId(ProductBrandId).ToList());
        }


        /// <summary>
        /// 获取品牌中价格最低的品牌
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页码数量</param>
        /// <returns>价格最低的品牌</returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetOneBrand(int page = 1, int pageSize = 10)
        {
            var sech = new ProductBrandSearchCondition
            {
                Page = page,
                PageCount = pageSize,
            };
            //取出所有品牌
            var brandList = _productBrandService.GetProductBrandsByCondition(sech).Select(a => new
            {
                a.Id,
                a.Bimg,
                a.Bname

            }).ToList();

            //通过品牌取出该品牌下的价格最低的一个商品
            var product = new ProductEntity();
            List<RecProdcut> listRecProdcut = new List<RecProdcut>();
            foreach (var i in brandList)
            {
                product = GetProductByBrand(i.Id);

                if (product != null)
                {
                    listRecProdcut.Add(new RecProdcut
                    {
                        Bimg = i.Bimg,
                        BrandId = i.Id.ToString(),
                        BrandName = i.Bname,
                        Commition = product.Dealcommission.ToString(),
                        HouseType = product.ProductParameter.FirstOrDefault(o => o.Parameter.Name == "户型") == null ? "" : product.ProductParameter.FirstOrDefault(o => o.Parameter.Name == "户型").ParameterValue.Parametervalue.ToString(),
                        Price = product.Price.ToString(),
                        ProductId = product.Id,
                        SubTitle = product.SubTitle,
                        Productname = product.Productname
                    });
                }
            }
            var totalCount1 = _productBrandService.GetProductBrandCount(sech);
            //  return PageHelper.toJson(new { List = BrandList, Product = product, Condition = sech, totalCount = totalCount1 });
            return PageHelper.toJson(new { List = listRecProdcut, Condition = sech, totalCount = totalCount1 });
        }

        /// <summary>
        /// 通过BrandID获取该品牌下的最小价格商品
        /// </summary>
        /// <param name="id">品牌ID</param>
        /// <returns>最小价格品牌</returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public ProductEntity GetProductByBrand(int BrandId)
        {
            var sech = new ProductSearchCondition
            {
                OrderBy = EnumProductSearchOrderBy.Price,
                ProductBrand = BrandId
            };
            var model = _productService.GetProductsByCondition(sech).FirstOrDefault();
            if (model == null)
            {
                return null;
            }
            return model;
        }





        #endregion
    }




    /// <summary>
    /// 推荐商品 （经纪人专区 推荐楼房）
    /// </summary>
    [Description("推荐商品管理类")]
    public class RecProdcut
    {
        /// <summary>
        /// 品牌ID
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 品牌图
        /// </summary>
        public string Bimg { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Productname { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public string HouseType { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 最高佣金
        /// </summary>
        public string Commition { get; set; }
        /// <summary>
        /// 广告标题
        /// </summary>
        public string SubTitle { get; set; }

    }

}