using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Weather.Data.Base;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,CompareReportRain")]
    public class CompareReportRainController : Controller
    {

        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _environment;

        [HttpPost]
        public DataTable Electrical_Data(long StationId, string startDate, string endDate)
        {
            DataTable ElDataTable = new DataTable();
            DateTime stDate = startDate.ToGreDateTime();
            DateTime enDate = endDate.ToGreDateTime();

            try
            {

                List<double> data = new List<double>();
                List<string> RangeDate = new List<string>();
                string[,] Parametr = new string[3, 2];
                Parametr[0, 0] = "@FromDate";
                Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[1, 0] = "@ToDate";
                Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[2, 0] = "@StationId";
                Parametr[2, 1] = StationId.ToString();
                ElDataTable = _genericUoW.GetFromSp(Parametr, "Sp_CompareRainData");

            }
            catch (Exception ex)
            { }
            return ElDataTable;
        }
        [HttpPost]
        public DataTable Mechanical_Data(IFormFile file)
        {
            string conString = string.Empty;
            DataTable MeDataTable = new DataTable();


            if (file != null && file.Length > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", file.FileName);


                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }


                    conString = string.Format(conString, path);

                    switch (fileExtension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES'";
                            MeDataTable = Utility.ConvertXSLXtoDataTable(conString, "IMPORT DATAEASE.");

                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES'";
                            MeDataTable = Utility.ConvertXSLXtoDataTable(conString, "IMPORT DATAEASE.");

                            break;
                    }
                }
            }
            if (MeDataTable.Rows.Count > 0)
            {
                MeDataTable.Columns.Remove("cs");
                MeDataTable.Columns.Remove("sazman");
                MeDataTable.Columns.Remove("ost");
                MeDataTable.Columns.Remove("ostan");
                MeDataTable.Columns.Remove("river");
                MeDataTable.Columns.Remove("abi");
                MeDataTable.Columns.Remove("mah2");
                MeDataTable.Columns.Remove("code");
                MeDataTable.Columns.Remove("station");
                MeDataTable.Columns.Remove("notice");
                MeDataTable.Columns.Remove("saat_shoroe");
                MeDataTable.Columns.Remove("saat_payan");
                MeDataTable.Columns.Remove("barf18");
                MeDataTable.Columns.Remove("barf6");
                MeDataTable.Columns.Remove("ab_barf18");
                MeDataTable.Columns.Remove("baran18");
                MeDataTable.Columns.Remove("ab_barf6");
                MeDataTable.Columns.Remove("baran6");
                MeDataTable.Columns.Remove("sb");
                MeDataTable.Columns.Remove("star");
                int count = MeDataTable.Columns.Count;
                for (int i = 4; i < count; i++)
                {
                    MeDataTable.Columns.Remove(MeDataTable.Columns[4].ColumnName);
                }
            }
            return MeDataTable;



        }

        public CompareReportRainController(GenericUoW genericUoW, UserManager<User> userManager, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                return View();
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();

                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                return View();
            }
        }


        public async Task<ActionResult> CompareReportRain_List(IFormFile file, long StationId,string StationName, string StartDate, string EndDate)
        {
            DataTable CompareTable = new DataTable();
            try
            {
                PersianCalendar pc = new PersianCalendar();

                User user = await _userManager.GetUserAsync(HttpContext.User);
                DataTable ElDt = Electrical_Data(StationId, StartDate, EndDate);
                DataTable MeDt = Mechanical_Data(file);
                MeDt.Columns.Add("Datetime", typeof(DateTime));

                for (int j = 0; j < MeDt.Rows.Count; j++)
                {
                    MeDt.Rows[j]["Datetime"] = pc.ToDateTime(Convert.ToInt16(Convert.ToInt16(MeDt.Rows[j]["sal"].ToString())), Convert.ToInt16(MeDt.Rows[j]["mah"].ToString()), Convert.ToInt16(MeDt.Rows[j]["rooz"].ToString()), 0, 0, 0, 0);

                }

                MeDt.Columns.Remove("sal");
                MeDt.Columns.Remove("mah");
                MeDt.Columns.Remove("rooz");

                CompareTable.Columns.Add("Datetime", typeof(DateTime));
                CompareTable.Columns.Add("StationId", typeof(long));
                CompareTable.Columns.Add("StationName", typeof(string));
                CompareTable.Columns.Add(new DataColumn { ColumnName = "baran", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mbaran", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cbaran", DataType = typeof(float), AllowDBNull = true });



                double baran = 0, mbaran = 0;
                DateTime date;

                DateTime stDate = StartDate.ToGreDateTime();
                DateTime enDate = EndDate.ToGreDateTime();

                DateTime Eldate, Medate;

                date = stDate;
                string strdate = "";
                for (int i = 1; i < ((TimeSpan)(enDate - stDate)).Days + 1; i++)
                {
                    baran = 0; mbaran = 0;
                    DataRow[] ElfoundRows;
                    DataRow[] MefoundRows;
                    DataRow newRow = CompareTable.NewRow();

                    newRow["Datetime"] = date;
                    newRow["StationId"] = StationId;
                    newRow["StationName"] = StationName;
                    newRow["baran"] = DBNull.Value;
                    newRow["mbaran"] = DBNull.Value;
                    newRow["cbaran"] = DBNull.Value;


                    ElfoundRows = ElDt.Select("Datetime=#" + date.ToShortDateString() + "#");
                    if (ElfoundRows.Length > 0)
                    {
                        baran = Convert.ToDouble(ElfoundRows[0][1] == DBNull.Value ? 10000 : ElfoundRows[0][1]);
                        newRow["baran"] = baran;
                    }
                    MefoundRows = MeDt.Select("Datetime=#" + date.ToShortDateString() + "#");
                    if (MefoundRows.Length > 0)
                    {
                        mbaran = Convert.ToDouble(MefoundRows[0][0] == DBNull.Value ? 10000 : MefoundRows[0][0]);
                        newRow["mbaran"] = mbaran;
                    }
                    if (mbaran != 10000 && baran != 10000)
                        newRow["cbaran"] = Math.Round((mbaran - baran) / mbaran * 100, 0).ToString();
                    CompareTable.Rows.Add(newRow);
                    date = date.AddDays(1);
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView(CompareTable);

        }
        [HttpPost]
        public async Task<JsonResult> GetStation(int Projectid)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            else
            {
                List<long> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
                return Json(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x == n.Id) && n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            }
        }

     
    }
}
