using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Demo_CRUD.Models;

namespace MVC_Demo_CRUD.Controllers
{
    public class NewsController : Controller
    {
        CRUDEntities css = new CRUDEntities();
        // GET: News
        public ActionResult Index()
        {
            ViewBag.msg = (from x in css.Newsinfo
                        join y in css.NewType
                        on x.newsType equals y.typeid
                        select new NewInfos()
                        {
                            newsid = x.newsid,
                            newsName = x.newsName,
                            typeName = y.typeName,
                            typeid=y.typeid,
                            newImage=x.newImage
                        }).ToList();
            return View();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int newsid)
        {
            var nesid = css.Newsinfo.FirstOrDefault(x => x.newsid == newsid);
            if (nesid!=null)
            {
                css.Newsinfo.Remove(nesid);
                css.SaveChanges();
            }
            return Content("<script>alert('删除完成');window.location.href='http://localhost:9291/News/Index'</script>");
        }
        /// <summary>
        /// 添加视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            var list = css.NewType.ToList();
            return View(list);
        }
        /// <summary>
        /// 添加功能
        /// </summary>
        /// <param name="newsinfos"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Addinfo(NewInfos newsinfos)
        {
            var image = AddImage();
            newsinfos.newImage = image;
            css.Newsinfo.Add(new Newsinfo
            {
                newsName = newsinfos.newsName,
                newsType = newsinfos.typeid,
                newImage = image
            });
            css.SaveChanges();
            return Content("<script>alert('增加完成');window.location.href='http://localhost:9291/News/Add'</script>");
        }
       /// <summary>
       /// 保存图片
       /// </summary>
       /// <returns></returns>
        private string AddImage()
        {
            var guid = "";
            var list = Request.Files.GetMultiple("newImage");
            if (list[0].FileName != "")
            {
                foreach (var item in list)
                {
                    var filename = item.FileName;

                    FileInfo file = new FileInfo(filename);
                    string ext = file.Extension;

                    guid = Guid.NewGuid().ToString() + ext;
                    guid = guid.Replace("-", "");

                    var path = Server.MapPath("~/upload/");
                    item.SaveAs(path + guid);
                }
            }
            return guid;
        }
        /// <summary>
        /// 修改视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int newsid)
        {
            //1、当前要修改的类别
            var newsOne = css.Newsinfo.FirstOrDefault(x => x.newsid == newsid);
            //2、类别当前修改的类别
            ViewBag.list2 = css.NewType.FirstOrDefault(x=>x.typeid==newsOne.newsType).typeName;
            //3、所有类别
            ViewBag.list = css.NewType.ToList();
            //4、传递当前编辑
            return View(newsOne);
        }
        /// <summary>
        /// 修改功能
        /// </summary>
        /// <param name="newInfos"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Editinfo(NewInfos newInfos)
        {
            //1、调用保存图片
            var image = AddImage();
            if (newInfos != null)
            {

                //2、保存
                var userinfo = css.Newsinfo.FirstOrDefault(x => x.newsid == newInfos.newsid);
                //1.1查询当前图片为空显示原有

                userinfo.newsName = newInfos.newsName;
                userinfo.newsType = newInfos.typeid;
                //3、如果修改的图片为空则赋值原来的图片
                if (image!="")
                {
                    userinfo.newImage = image;
                }
                else
                {
                    userinfo.newImage=userinfo.newImage;
                }
                css.SaveChanges();
                return Content("<script>alert('修改成功！');window.location.href='http://localhost:9291/News/Index'</script>");
            }
            else
            {
                return Content("<script>alert('修改失败！');window.location.href='http://localhost:9291/News/Edit'</script>");
            }
        }
    }
}