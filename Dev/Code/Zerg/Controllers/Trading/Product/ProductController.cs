using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        /// <summary>
        /// 构造函数（操作函数注入）
        /// </summary>
        public ProductController(
            IProductService productService,
            IProductBrandService productBrandService,
            IProductDetailService productDetailService,
            IProductParameterService productParameterService,
            IClassifyService classifyService)
        {
            _productService = productService;
            _productBrandService = productBrandService;
            _productDetailService = productDetailService;
            _productParameterService = productParameterService;
            _classifyService = classifyService;
        }


        #region 商品添加/删除
        /// <summary>
        /// 添加商品；
        /// </summary>
        /// <param name="obj">此参数由product和productDetail组成</param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public int AddProduct([FromBody]JObject obj)
        {
            dynamic json = obj;
            JObject JProduct = json.product;
            JObject JProductDetail = json.productDetail;
            var product = JProduct.ToObject<ProductModel>();
            var productDetail = JProductDetail.ToObject<ProductDetailModel>();
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
                Adduser = productDetail.Adduser,
                Updtime = DateTime.Now,
                Upduser = productDetail.Upduser
            };

            ProductDetailEntity PDE2 = _productDetailService.Create(PDE);
            ClassifyEntity CE = _classifyService.GetClassifyById(product.ClassifyId);
            ProductBrandEntity CBE = _productBrandService.GetProductBrandById(product.ProductBrandId);
            ProductEntity PE = new ProductEntity()
            {
                Bussnessid = product.Bussnessid,
                BussnessName="yoopoon",
                Commission=product.Commission,
                Dealcommission=product.Dealcommission,
                Price=product.Price,
                Classify = CE,
                ProductBrand = CBE,
                ProductDetail = PDE2,
                Productimg = PDE.Productimg,
                Productname = PDE.Productname,
                Recommend = product.Recommend,
                Sort = product.Sort,
                Status = product.Status,
                Stockrule = product.Stockrule,
                Updtime = DateTime.Now,
                Upduser = PDE.Upduser,
                Addtime = DateTime.Now,
                Adduser = PDE.Adduser
            };
            try
            {
                return _productService.Create(PE).Id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string DelProduct(int productId)
        {
            try
            {
                ProductEntity PE = _productService.GetProductById(productId);
                if (_productService.Delete(PE)) {
                    return "删除商品成功";
                }
                else {
                    return "删除商品失败，该商品可能有关联项！";
                }
               
            }
            catch (Exception e)
            {
                return "删除商品失败";
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
        public HttpResponseMessage GetAllProduct()
        {
            ProductSearchCondition PSC = new ProductSearchCondition()
            {
                OrderBy = EnumProductSearchOrderBy.OrderById
            };
            var product = _productService.GetProductsByCondition(PSC).Select(a => new ProductDetail
            {
                Productname = a.Productname,
                Productimg = a.ProductDetail.Productimg,
                Price = a.Price,
                SubTitle = a.SubTitle,
                ProductDetailed = a.ProductDetail.Productdetail
            }).ToList();
            var totalCount = _productService.GetProductCount(PSC);
            return PageHelper.toJson(new { List = product, TotalCount = totalCount });
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
            return PageHelper.toJson(_productService.GetProductById(productId));
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
            var productList = _productService.GetProductsByProductBrand(BrandId).Select(a => new ProductDetail
            {
                Productname = a.Productname,
                Productimg = a.Productimg,
                Price = a.Price,
                SubTitle = a.SubTitle,
                Phone = a.ContactPhone,
                Productimg1 = a.ProductDetail.Productimg1,
                Productimg2 = a.ProductDetail.Productimg2,
                Productimg3 = a.ProductDetail.Productimg3,
                Productimg4 = a.ProductDetail.Productimg4,
                ProductDetailed=a.ProductDetail.Productdetail
            }).ToList();
            //return PageHelper.toJson(_productService.GetProductsByProductBrand(BrandId));
            return PageHelper.toJson(productList);
        }

        public HttpResponseMessage GetSearchProduct(ProductSearchCondition condtion)
        {
            var productList = _productService.GetProductsByCondition(condtion).Select(a => new ProductDetail
            {
                Productname = a.Productname,
                Price = a.Price,
                SubTitle = a.SubTitle,
                Productimg = a.ProductDetail.Productimg,
                Productimg1 = a.ProductDetail.Productimg1,
                Productimg2 = a.ProductDetail.Productimg2,
                Productimg3 = a.ProductDetail.Productimg3,
                Productimg4 = a.ProductDetail.Productimg4,
                ProductDetailed=a.ProductDetail.Productdetail
            }).ToList();         
            return PageHelper.toJson(productList);
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