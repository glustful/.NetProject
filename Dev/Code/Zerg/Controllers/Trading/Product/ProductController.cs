using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;
using Trading.Entity.Model;
using Trading.Service.Classify;
using Trading.Service.ParameterValue;
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
    [EnableCors("*", "*", "*", SupportsCredentials = true)] 
    public class ProductController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IProductBrandService _productBrandService;
        private readonly IProductDetailService _productDetailService;
        private readonly IProductParameterService _productParameterService;
        private readonly IClassifyService _classifyService;
        private readonly IWorkContext _workContent;
        /// <summary>
        /// 构造函数（操作函数注入）
        /// </summary>
        public ProductController(
            IProductService productService,
            IProductBrandService productBrandService,
            IProductDetailService productDetailService,
            IProductParameterService productParameterService,
            IClassifyService classifyService, IWorkContext workContent)
        {
            _productService = productService;
            _productBrandService = productBrandService;
            _productDetailService = productDetailService;
            _productParameterService = productParameterService;
            _classifyService = classifyService;
            _workContent = workContent;
        }


        #region 商品添加/删除
        /// <summary>
        /// 添加商品；
        /// </summary>
        /// <param name="obj">此参数由product和productDetail组成</param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage AddProduct([FromBody]JObject obj)
        {
            dynamic json = obj;
            JObject JProduct = json.product;
            JObject JProductDetail = json.productDetail;
            var product = JProduct.ToObject<ProductModel>();
            var productDetail = JProductDetail.ToObject<ProductDetailModel>();
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(productDetail.Productname);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
            }
            else
            {
                //先创建productDetail，跟据部分productDetail部分重叠信息创建product；
                ProductDetailEntity PDE = new ProductDetailEntity()
                {
                    Id = 0,
                    Productdetail = productDetail.Productdetail,
                    Productimg = productDetail.Productimg,
                    Productimg1 = productDetail.Productimg1,
                    Productimg2 = productDetail.Productimg2,
                    Productimg3 = productDetail.Productimg3,
                    Productimg4 = productDetail.Productimg4,
                    Productname = productDetail.Productname,
                    Sericeinstruction = productDetail.Sericeinstruction,
                    Addtime = DateTime.Now,
                    //Adduser = productDetail.Adduser,
                    Adduser = _workContent.CurrentUser.Id.ToString(),
                    Updtime = DateTime.Now,
                    //Upduser = productDetail.Upduser
                    Upduser = _workContent.CurrentUser.Id.ToString()
                };

                ProductDetailEntity PDE2 = _productDetailService.Create(PDE);
                ClassifyEntity CE = _classifyService.GetClassifyById(product.ClassifyId);
                ProductBrandEntity CBE = _productBrandService.GetProductBrandById(product.ProductBrandId);
                ProductEntity PE = new ProductEntity()
                {
                    Bussnessid = product.Bussnessid,
                    BussnessName = "yoopoon",
                    Commission = product.Commission,
                    RecCommission = product.RecCommission,
                    Dealcommission = product.Dealcommission,
                    Price = product.Price,
                    Classify = CE,
                    ProductBrand = CBE,
                    ProductDetail = PDE2,
                    Productimg = PDE.Productimg,
                    Productname = PDE.Productname,
                    Recommend = product.Recommend,
                    Sort = product.Sort,
                    Status = product.Status,
                    Stockrule = product.Stockrule,
                    SubTitle = product.SubTitle,
                    ContactPhone = product.ContactPhone,
                    Updtime = DateTime.Now,
                    //Upduser = PDE.Upduser,
                    Upduser = _workContent.CurrentUser.Id.ToString(),
                    Addtime = DateTime.Now,
                    //Adduser = PDE.Adduser
                    Adduser = _workContent.CurrentUser.Id.ToString()
                };
                var Product = _productService.Create(PE);
                if (Product != null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！", Product.Id));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                }
            }
            //try
            //{
            //    return _productService.Create(PE).Id;
            //}
            //catch (Exception e)
            //{
            //    return -1;
            //}
        }
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage DelProduct(int productId)
        {
            try
            {
                ProductEntity PE = _productService.GetProductById(productId);
                if (_productService.Delete(PE)) {
                    return PageHelper.toJson(PageHelper.ReturnValue(true,"数据删除成功"));
                }
                else {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "删除商品失败，该商品可能有关联项！"));                  
                }
               
            }
            catch (Exception e)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除商品失败"));             
            }
        }
        #endregion

        #region 商品查询
        /// <summary>
        /// 查询所有的商品列表；
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetAllProduct(int page=1,int pageSize=10)
        {
            ProductSearchCondition PSC = new ProductSearchCondition()
            {
                Page = page,
                PageCount = pageSize,
                OrderBy = EnumProductSearchOrderBy.OrderById
            };
            var productList = _productService.GetProductsByCondition(PSC).Select(a => new ProductDetail
            {                
                Id=a.Id,
                Productname = a.Productname,
                Productimg = a.Productimg,
                Price = a.Price,

                RecCommission=a.RecCommission,
                Commission=a.Commission,
                Dealcommission=a.Dealcommission,
                ClassifyName=a.Classify.Name,
                Addtime = a.Addtime,

                SubTitle = a.SubTitle,
                ProductDetailed = a.ProductDetail.Productdetail,
                StockRule=a.Stockrule,
                Advertisement = a.ProductDetail.Ad1,
                Acreage = a.ProductParameter.FirstOrDefault(pp=>pp.Parameter.Name=="面积").ParameterValue.Parametervalue.ToString(),
                Type = a.ProductParameter.FirstOrDefault(p => p.Parameter.Name == "户型").ParameterValue.Parametervalue.ToString()
            }).ToList().Select(b=>new
            {
                b.Id,
                b.Productname,
                b.Productimg,
                b.Price,

                b.RecCommission,
                b.Commission,
                b.Dealcommission,
                b.ClassifyName,
                b.Addtime,

                b.SubTitle,
                b.ProductDetailed,
                b.StockRule,
                b.Acreage,
                b.Type,
                b.Advertisement
            });
            var totalCount = _productService.GetProductCount(PSC);
            return PageHelper.toJson(new { List =productList,Condition=PSC, TotalCount = totalCount });
            //return PageHelper.toJson(_productService.GetProductsByCondition(PSC).ToList());
        }
        /// <summary>
        /// 根据Id查询商品
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetProductById(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不存在"));
            }
            var productDetail = new ProductDetail
            {
                Id = product.Id,
                Productname = product.Productname,
                Productimg = product.ProductDetail.Productimg,
                BrandImg = product.ProductBrand.Bimg,
                Price = product.Price,
                SubTitle = product.SubTitle,
                Phone = product.ContactPhone,              
                Status = product.Status==true?0:1,
                Recommend = product.Recommend==true?0:1,
                Stockrule = product.Stockrule,
                RecCommission = product.RecCommission,
                Dealcommission = product.Dealcommission,
                Commission = product.Commission,
                Sericeinstruction=product.ProductDetail.Sericeinstruction,  
               // Bname = product.ProductBrand.Bname,
                BrandId = product.ProductBrand.Id,
                ClassId = product.Classify.Id,
                // ReSharper disable once PossibleNullReferenceException
                Type =  product.ProductParameter.FirstOrDefault(p=>p.Parameter.Name=="户型")== null? "":product.ProductParameter.FirstOrDefault(p=>p.Parameter.Name=="户型").ParameterValue.Parametervalue,
                Advertisement=product.ProductDetail.Ad2,
                Productimg1 = product.ProductDetail.Productimg1,
                Productimg2 = product.ProductDetail.Productimg2,
                Productimg3 = product.ProductDetail.Productimg3,
                Productimg4 = product.ProductDetail.Productimg4,
                ProductDetailed = product.ProductDetail.Productdetail,
            };
            return PageHelper.toJson(productDetail);
        }

        /// <summary>
        /// 根据品牌项目获取产品列表
        /// </summary>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetProductsByBrand(int BrandId)
        {
            var product = _productService.GetProductsByProductBrand(BrandId).ToList();
            if(product.Count==0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"数据不存在"));
            }
            var productList=product.Select(a => new ProductDetail
                {
                    Productname = a.Productname,
                    Productimg = a.Productimg,
                    Price = a.Price,
                    SubTitle = a.SubTitle,
                    Phone = a.ContactPhone,
                    Id = a.Id,


                    //Productimg1 = a.ProductDetail.Productimg1,
                    //Productimg2 = a.ProductDetail.Productimg2,
                    //Productimg3 = a.ProductDetail.Productimg3,
                    //Productimg4 = a.ProductDetail.Productimg4,
                    //ProductDetailed = a.ProductDetail.Productdetail
                }).ToList();
            //var productList = _productService.GetProductsByProductBrand(BrandId).Select(a => new ProductDetail
            //    {
            //        Productname = a.Productname,
            //        Productimg = a.Productimg,
            //        Price = a.Price,
            //        SubTitle = a.SubTitle,
            //        Phone = a.ContactPhone,


            //        //Productimg1 = a.ProductDetail.Productimg1,
            //        //Productimg2 = a.ProductDetail.Productimg2,
            //        //Productimg3 = a.ProductDetail.Productimg3,
            //        //Productimg4 = a.ProductDetail.Productimg4,
            //        //ProductDetailed = a.ProductDetail.Productdetail
            //    }).ToList();
            var Content = product.Select(p => new
            {
                p.ProductBrand.Content
            }).First(); 
           
            //var Content = _productService.GetProductsByProductBrand(BrandId).Select(p => new
            //{
            //    p.ProductBrand.Content
            //}).First();

            //return PageHelper.toJson(_productService.GetProductsByProductBrand(BrandId));
            return PageHelper.toJson(new { productList = productList, content = Content });
        }
      
         [HttpGet]
        public HttpResponseMessage GetSearchProduct([FromUri]ProductSearchCondition condtion)
        {
            var productList = _productService.GetProductsByCondition(condtion).Select(a => new ProductDetail
            {
                Id =a.Id, 
                Productname = a.Productname,
                Productimg = a.Productimg,
                Price = a.Price,
                SubTitle = a.SubTitle,
                Advertisement = a.ProductDetail.Ad1,
                ProductDetailed = a.ProductDetail.Productdetail,
                StockRule=a.Stockrule,
                Acreage = a.ProductParameter.FirstOrDefault(pp=>pp.Parameter.Name=="面积").ParameterValue.Parametervalue.ToString(),
                Type = a.ProductParameter.FirstOrDefault(p => p.Parameter.Name == "户型").ParameterValue.Parametervalue.ToString()
            }).ToList();
              // return PageHelper.toJson(productList);
            var totalCount = _productService.GetProductCount(condtion);
            return PageHelper.toJson(new { List = productList, TotalCount = totalCount });
        }

        /// <summary>
        /// 根据分类获取产品列表
        /// </summary>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetProductsByClassify(int ClassifyId)
        {
            List<ProductEntity> PEList = new List<ProductEntity>();
            List<ClassifyEntity> CEL = GetTreeAllEndPoints(ClassifyId);
            foreach (var ce in CEL)//遍历添加每个末端分类下的产品列表；
            {
                List<ProductEntity> PELBuffer = _productService.GetProductsByClassify(ce.Id).ToList<ProductEntity>();
                foreach (var pe in PELBuffer)
                {
                    PEList.Add(pe);
                }
            }
            return PageHelper.toJson(PEList);
        }
        /// <summary>
        /// 获取该商品所有的分类参数值；
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="pageindex">分页Id</param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetAllParameterValueByProduct(int productId)
        {
            return PageHelper.toJson(_productParameterService.GetProductParametersByProduct(productId));
        }
        #endregion

        #region 商品编辑更新

        /// <summary>
        /// 上下架商品；
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string EditProductStatus(int productId, bool status)
        {
            try
            {
                ProductEntity pe = _productService.GetProductById(productId);
                pe.Status = status;
                _productService.Update(pe);
                return "修改商品状态成功";
            }
            catch (Exception e)
            {
                return "修改商品状态失败";
            }

        }
        /// <summary>
        /// 更新商品库存数量
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string EditProductStockrule(int productId, int stock)
        {
            try
            {
                ProductEntity pe = _productService.GetProductById(productId);
                pe.Stockrule = stock;
                _productService.Update(pe);
                return "修改商品状态成功";
            }
            catch (Exception e)
            {
                return "修改商品状态失败";
            }
        }
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditProduct(JObject obj)
        {
            dynamic json = obj;
            JObject JProduct = json.product;
            JObject JProductDetail = json.productDetail;
            var newProduct = JProduct.ToObject<ProductModel>();
            var newProductDetail = JProductDetail.ToObject<ProductDetailModel>();
            var oldProduct = _productService.GetProductById(newProduct.Id);
            var oldProductDetail = _productDetailService.GetProductDetailById(oldProduct.ProductDetail.Id);
            ClassifyEntity CE = _classifyService.GetClassifyById(newProduct.ClassifyId);
            ProductBrandEntity CBE = _productBrandService.GetProductBrandById(newProduct.ProductBrandId);
            //商品
            oldProduct.Price = newProduct.Price;
            oldProduct.ProductBrand = CBE;
            oldProduct.Classify = CE;
            oldProduct.Productname = newProduct.Productname;
            oldProduct.Commission = newProduct.Commission;
            oldProduct.ContactPhone = newProduct.ContactPhone;
            oldProduct.Dealcommission = newProduct.Dealcommission;
            oldProduct.Productimg = newProduct.Productimg;
            oldProduct.Status = newProduct.Status;
            oldProduct.Recommend = newProduct.Recommend;
            oldProduct.Stockrule = newProduct.Stockrule;
            oldProduct.SubTitle = newProduct.SubTitle;
            oldProduct.Upduser = _workContent.CurrentUser.Id.ToString();
            oldProduct.Updtime=DateTime.Now;
            //商品详细
            oldProductDetail.Productname = newProduct.Productname;
            oldProductDetail.Productdetail = newProductDetail.Productdetail;
            oldProductDetail.Productimg = newProduct.Productimg;
            oldProductDetail.Sericeinstruction = newProductDetail.Sericeinstruction;
            oldProductDetail.Productimg1 = newProductDetail.Productimg1;
            oldProductDetail.Productimg2 = newProductDetail.Productimg2;
            oldProductDetail.Productimg3 = newProductDetail.Productimg3;
            oldProductDetail.Productimg4 = newProductDetail.Productimg4;
            oldProductDetail.Updtime=DateTime.Now;
            oldProductDetail.Upduser = _workContent.CurrentUser.Id.ToString();
            if(_productService.Update(oldProduct)!=null&&_productDetailService.Update(oldProductDetail)!=null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
            }
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
    }
   
}
