using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CMS.Entity.Model;
using CMS.Service.Resource;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Common.Oss;

namespace Zerg.Controllers.CMS
{
    public class ResourceController : ApiController
    {
        
        private readonly IResourceService _resourceService;
        private readonly IWorkContext _workContent;
        public ResourceController(IResourceService resourceService,IWorkContext workContent) 
        {
            _resourceService = resourceService;
            _workContent = workContent;
        }
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
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
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
                    var fileLength = ms.Length;
                    var info = new FileInfo(item.Headers.ContentDisposition.FileName.Replace("\"", ""));


                    var fileNewName = GetUniquelyString();
                    var key = OssHelper.PutObject(ms, fileNewName + info.Extension);
                   
                    var resource = new ResourceEntity
                    {
                        Guid = Guid.NewGuid(),
                        Name = fileNewName,
                        Length = fileLength,
                        Type = info.Extension.Substring(1).ToLower(),
                        Adduser = _workContent.CurrentUser.Id,
                        Addtime = DateTime.Now,
                        UpdUser = _workContent.CurrentUser.Id,
                        UpdTime = DateTime.Now,
                    };
                    if(_resourceService.Create(resource).Id >0)
                        fileNames.Add(key);
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(true, string.Join("|",fileNames)));
        }
        public HttpResponseMessage Search()
        {
            return null;
        }
        public HttpResponseMessage Deltte(int id)
        {
            return null;
        }
    }
}
