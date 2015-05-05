using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CMS.Entity.Model;
using CMS.Service.Resource;
using YooPoon.Core.Site;
using Zerg.Common;

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
            string strTemp, strYear, strMonth, strDay, strHour, strMinute, strSecond, strMillisecond;
            DateTime dt = DateTime.Now;
            int rndNumber = rnd.Next(randomMaxValue);
            strYear = dt.Year.ToString();
            strMonth = (dt.Month > 9) ? dt.Month.ToString() : "0" + dt.Month.ToString();
            strDay = (dt.Day > 9) ? dt.Day.ToString() : "0" + dt.Day.ToString();
            strHour = (dt.Hour > 9) ? dt.Hour.ToString() : "0" + dt.Hour.ToString();
            strMinute = (dt.Minute > 9) ? dt.Minute.ToString() : "0" + dt.Minute.ToString();
            strSecond = (dt.Second > 9) ? dt.Second.ToString() : "0" + dt.Second.ToString();
            strMillisecond = dt.Millisecond.ToString();
            strTemp = strYear + strMonth + strDay + "_" + strHour + strMinute + strSecond + "_" + strMillisecond + "_" + rndNumber.ToString();
            return strTemp;
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            int pos = file.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1;
            var resource = new ResourceEntity
            {
                Guid = Guid.NewGuid(),
                Name = GetUniquelyString()+file.FileName,
                Length = file.ContentLength,
                Type=file.FileName.Substring(pos,file.FileName.Length-pos),
                Adduser=_workContent.CurrentUser.Id,
                Addtime=DateTime.Now,
                 UpdUser=_workContent.CurrentUser.Id,
                UpdTime=DateTime.Now,
            };
            if (_resourceService.Create(resource) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "文件上传成功"));
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"文件上传失败"));
            }            
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
