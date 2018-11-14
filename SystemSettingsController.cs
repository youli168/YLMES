using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

namespace YLMES.Controllers
{
    public class SystemSettingsController : Controller
    {
      

        // GET: SystemSettings
        public ActionResult i() {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        #region 权限设置

        public ActionResult Permissions()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                List<PM_FunctionList> list = ys.PM_FunctionList.ToList();
                ViewBag.ck = list;
            }
            return View();
        }
        public ActionResult ProductLine() {

            return View();
        }
        public ActionResult ProductStation()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                List<PM_ProductLine> list = ys.PM_ProductLine.ToList();
                ViewBag.ck = list;
            }
            return View();
        }
        //添加工位
        public string ADDProductStation()
        {
            string CreatedBy = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "ADD");
                parms[1] = new SqlParameter("@CreatedBy", CreatedBy.ToString());
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductStation @Type,'','','','','','','','',@CreatedBy", parms);
            }
            return "true";
        }
        //删除工位类型
        public string DlProductStation(string StationID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "delete");
                parms[1] = new SqlParameter("@StationID", Convert.ToInt32(StationID));
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductStation @Type,@StationID", parms);

            }
            return "true";
        }
        //修改工位类型
        public string UpProductStation(string Line, string StationID,string Station,string StationType)
        {
            string CreatedBy = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
              
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@Type", "Update");
                parms[1] = new SqlParameter("@StationID", Convert.ToInt32(StationID));
                parms[2] = new SqlParameter("@Station", Station);
                parms[3] = new SqlParameter("@Line", Line);
                parms[4] = new SqlParameter("@StationType", StationType);
                parms[5] = new SqlParameter("@CreatedBy", CreatedBy.ToString());
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductStation @Type,@StationID,@Station,'',@Line,'','','',@StationType,@CreatedBy", parms);


            }
            return "true";
        }
        //搜索工位类型
        public ActionResult SeProductStation(string Name, int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "check");
                parms[1] = new SqlParameter("@Station", Name);
                var list = ys.Database.SqlQuery<SP_PM_ProductStation_Result>("exec SP_PM_ProductStation @Type,'',@Station", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_ProductStation_Result> pageList = new PageList<SP_PM_ProductStation_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        //添加生产线
        public string ADDProduct() {
            string CreatedBy = Session["name"].ToString();
         
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
            
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type","ADD");
                parms[1] = new SqlParameter("@CreatedBy",CreatedBy.ToString());
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductLine @Type,'','',@CreatedBy", parms);
            }
            return "true";
        }
        //删除生产线
        public string DlProduct(string lineID) {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "delete");
                parms[1] = new SqlParameter("@lineID",Convert.ToInt32( lineID));
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductLine @Type,@lineID", parms);

            }
            return "true";
        }
        //修改生产线信息
        public string  UpProduct(string line,string lineID) {
            string CreatedBy = Session["name"].ToString();
           
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type","Update");
                parms[1] = new SqlParameter("@line", line);
                parms[2] = new SqlParameter("@lineID",Int32.Parse(lineID));
                parms[3] = new SqlParameter("@CreatedBy",CreatedBy.ToString());
                int i =ys.Database.ExecuteSqlCommand("exec SP_PM_ProductLine @Type,@lineID,@line,@CreatedBy",parms);
           
                
            }
            return "true";
        }
        //搜索生产线
        public ActionResult SeProductLine(string Name,int page,int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "check");
                parms[1] = new SqlParameter("@line", Name);
                var list = ys.Database.SqlQuery<SP_PM_ProductLine_Result> ("exec SP_PM_ProductLine @Type,'',@line", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_ProductLine_Result> pageList = new PageList<SP_PM_ProductLine_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
               
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        //查询已有权限
        public ActionResult ByExistsRoot(string UserName, string FunctionName,int page,int limit) {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@type","Type");
                parms[1] = new SqlParameter("@UserName",UserName);
                parms[2] = new SqlParameter("@FunctionName",FunctionName);
                parms[3] = new SqlParameter("@typeid",int.Parse("1"));
                var list = ys.Database.SqlQuery<PM_EditAccessSetup_Result>("exec PM_EditAccessSetup @type,@UserName,@FunctionName,'',@typeid", parms).ToList();
                 hasmap = new Dictionary<string, Object>();
                    PageList<PM_EditAccessSetup_Result> pageList = new PageList<PM_EditAccessSetup_Result>(list, page, limit);
                    int count = list.Count();
                    hasmap.Add("code", 0);
                    hasmap.Add("msg", "");
                    hasmap.Add("count", count);
                    hasmap.Add("data", pageList);
                   
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        //查询未有权限
        public ActionResult ByNoExistsRoot(string UserName, string FunctionName, int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@type", "Type");
                parms[1] = new SqlParameter("@UserName", UserName);
                parms[2] = new SqlParameter("@FunctionName", FunctionName);
                parms[3] = new SqlParameter("@typeid", int.Parse("0"));
                var list = ys.Database.SqlQuery<PM_EditAccessSetup_Result>("exec PM_EditAccessSetup @type,@UserName,@FunctionName,'',@typeid", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<PM_EditAccessSetup_Result> pageList = new PageList<PM_EditAccessSetup_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);


        }
        //权限修改
        public string UpRole(string FunctionName,int Status ,string UserName) {

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@type", "EDIT");
                parms[1] = new SqlParameter("@UserName", UserName);
                parms[2] = new SqlParameter("@FunctionName", FunctionName);
                parms[3] = new SqlParameter("@Status", Status);
               int i = ys.Database.ExecuteSqlCommand("exec PM_EditAccessSetup @type,@UserName,@FunctionName,@Status,''", parms);
              
               
  
            }
            return "true";
        }
        #endregion
    }
}