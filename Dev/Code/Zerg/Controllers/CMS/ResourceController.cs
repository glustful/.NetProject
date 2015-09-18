using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using CMS.Entity.Model;
using CMS.Service.Resource;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;
using Zerg.Common;
using Zerg.Common.Oss;
using YooPoon.Common.Encryption;
using System.ComponentModel;
namespace Zerg.Controllers.CMS
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("资源管理类")]
    public class ResourceController : ApiController
    {

        private readonly IResourceService _resourceService;
        private readonly IWorkContext _workContent;
        /// <summary>
        /// 资源管理初始化
        /// </summary>
        /// <param name="resourceService">resourceService</param>
        /// <param name="workContent">workContent</param>
        [Description("资源管理构造函数")]
        public ResourceController(IResourceService resourceService, IWorkContext workContent)
        {
            _resourceService = resourceService;
            _workContent = workContent;
        }
        /// <summary>
        /// 获取一个不重复的资源文件名称,返回文件名
        /// </summary>
        /// <returns>资源文件名称</returns>
        private string GetUniquelyString() //获取一个不重复的文件名
        {
            Random rnd = new Random(); //获取一个随机数
            const int randomMaxValue = 1000;
            DateTime dt = DateTime.Now;
            int rndNumber = rnd.Next(randomMaxValue);
            var strYear = dt.Year.ToString();
            var strMonth = (dt.Month > 9) ? dt.Month.ToString() : "0" + dt.Month.ToString();
            var strDay = (dt.Day > 9) ? dt.Day.ToString() : "0" + dt.Day.ToString();
            var strHour = (dt.Hour > 9) ? dt.Hour.ToString() : "0" + dt.Hour.ToString();
            var strMinute = (dt.Minute > 9) ? dt.Minute.ToString() : "0" + dt.Minute.ToString();
            var strSecond = (dt.Second > 9) ? dt.Second.ToString() : "0" + dt.Second.ToString();
            var strMillisecond = dt.Millisecond.ToString();
            var strTemp = strYear + strMonth + strDay + "_" + strHour + strMinute + strSecond + "_" + strMillisecond + "_" + rndNumber.ToString();
            return strTemp;
        }
        /// <summary>
        /// 无传入参数,资源文件上传,返回上传后的资源名称(文件名称以"|"分隔开)
        /// </summary>
        /// <returns>资源文件名称</returns>
        [HttpPost]
        [Description("资源文件上传")]
        public async Task<HttpResponseMessage> Upload()
        {
            var streamProvider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(streamProvider);
            var fileNames = new List<string>();
            foreach (var item in streamProvider.Contents)
            {
                if (item.Headers.ContentDisposition.FileName != null)
                {
                    var ms = item.ReadAsStreamAsync().Result;

                    //double size = ms.Length/1024;
                    //int maxSize = 50;
                    //int maxWidth = 640;
                    //int maxHeight = 200;
                    //if (size > maxSize)
                    //{
                    //Image img = Image.FromStream(ms);
                    //    int imgWidth = img.Width;
                    //    int imgHeight = img.Height;
                    //    if (imgWidth > imgHeight)
                    //    {
                    //        if (imgWidth > maxWidth)
                    //        {
                    //            float toImgWidth = maxWidth;
                    //            //float toImgHeight = imgHeight / (imgWidth / toImgWidth);
                    //            // float toImgHeight = toImgWidth*imgHeight/imgWidth;
                    //            float toImgHeight = imgHeight * (toImgWidth / imgWidth);
                    //            // Bitmap newImg=new Bitmap(img,int.Parse(toImgWidth.ToString()),int.Parse(toImgHeight.ToString()));
                    //            Bitmap newImg = new Bitmap(img, Convert.ToInt16(toImgWidth), Convert.ToInt16(toImgHeight));

                    //            MemoryStream stream = new MemoryStream();
                    //            //newImg.Save(stream, newImg.RawFormat);                               
                    //            newImg.Save(stream, ImageFormat.Png);
                    //            ms = stream;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (imgHeight > maxHeight)
                    //        {
                    //            float toImgHeight = maxHeight;
                    //            float toImgWidth = imgWidth / (imgHeight / toImgHeight);
                    //            var newImg = new Bitmap(img, int.Parse(toImgWidth.ToString()), int.Parse(toImgHeight.ToString()));
                    //            MemoryStream stream = new MemoryStream();
                    //            newImg.Save(stream, newImg.RawFormat);
                    //            ms = stream;
                    //        }
                    //    }

                    //}
                    var fileLength = ms.Length;
                    var info = new FileInfo(item.Headers.ContentDisposition.FileName.Replace("\"", ""));
                    //var allowFomat = new[] {".png",".jpg",".jepg",".gif"};
                    //var isImage = allowFomat.Contains(info.Extension.ToLower());
                    var fileNewName = GetUniquelyString();

                    //保存至OSS
                    var key = OssHelper.PutObject(ms, fileNewName + info.Extension);
                    //if (isImage)
                    //{
                    //    OssHelper.PutThumbnaiObject(ms, fileNewName + info.Extension);
                    //}

                    var resource = new ResourceEntity
                    {
                        Guid = Guid.NewGuid(),
                        Name = fileNewName,
                        Length = fileLength,
                        Type = info.Extension.Substring(1).ToLower(),
                      //  Adduser = ((UserBase)_workContent.CurrentUser).Id,
                        Addtime = DateTime.Now,
                      //  UpdUser = _workContent.CurrentUser.Id,
                        UpdTime = DateTime.Now,
                    };
                    if (_resourceService.Create(resource).Id > 0)
                        fileNames.Add(key);
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(true, string.Join("|", fileNames)));
        }
        /// <summary>
        /// 资源文件检索,暂时未实现
        /// </summary>
        /// <returns>null(暂时未实现)</returns>
        [Description("资源文件检索,暂时未实现")]
        public HttpResponseMessage Search()
        {
            return null;
        }
        /// <summary>
        /// 资源文件删除,暂时未实现
        /// </summary>
        /// <param name="id">资源文件Id</param>
        /// <returns>null,暂时未实现</returns>
        [Description("资源文件删除,暂时未实现")]
        public HttpResponseMessage Deltte(int id)
        {
            return null;
        }
    }
}
