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
    [Authorize(Roles = "AdminiStratore,CompareReport")]
    public class CompareReportController : Controller
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
                ElDataTable = _genericUoW.GetFromSp(Parametr, "Sp_CompareData");

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
                            MeDataTable = Utility.ConvertXSLXtoDataTable(conString, "DATABASE");

                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES'";
                            MeDataTable = Utility.ConvertXSLXtoDataTable(conString, "DATABASE");

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
                MeDataTable.Columns.Remove("code");
                MeDataTable.Columns.Remove("station");
                MeDataTable.Columns.Remove("sal");
                MeDataTable.Columns.Remove("ertfa");
                MeDataTable.Columns.Remove("mah");
                MeDataTable.Columns.Remove("note");
                MeDataTable.Columns.Remove("dam_tar_1830");
                MeDataTable.Columns.Remove("dam_tar_1230");
                MeDataTable.Columns.Remove("dam_tar_630");
                MeDataTable.Columns.Remove("dam_max_1830");
                MeDataTable.Columns.Remove("dam_min_630");
                MeDataTable.Columns.Remove("ab_barfmozab_1830");
                MeDataTable.Columns.Remove("barftazeh_1830");
                MeDataTable.Columns.Remove("ab_barfmozab_630");
                MeDataTable.Columns.Remove("barftazeh_630");
                MeDataTable.Columns.Remove("baad_rozaneh");
                MeDataTable.Columns.Remove("counter_630");
                MeDataTable.Columns.Remove("tabkhir");
                MeDataTable.Columns.Remove("hajmab_1830");
                MeDataTable.Columns.Remove("hajmab_630");
                MeDataTable.Columns.Remove("dam_5z_630");
                MeDataTable.Columns.Remove("dam_5z_1230");
                MeDataTable.Columns.Remove("dam_10z_630");
                MeDataTable.Columns.Remove("dam_10z_1230");
                MeDataTable.Columns.Remove("dam_10z_1830");
                MeDataTable.Columns.Remove("dam_20z_630");
                MeDataTable.Columns.Remove("dam_20z_1230");
                MeDataTable.Columns.Remove("dam_20z_1830");
                MeDataTable.Columns.Remove("dam_50z_630");
                MeDataTable.Columns.Remove("dam_50z_1230");
                MeDataTable.Columns.Remove("dam_50z_1830");
                MeDataTable.Columns.Remove("dam_100z_630");
                MeDataTable.Columns.Remove("dam_100z_1230");
                MeDataTable.Columns.Remove("dam_100z_1830");
                MeDataTable.Columns.Remove("jahat_bad_630");
                MeDataTable.Columns.Remove("jahat_bad_1830");
                MeDataTable.Columns.Remove("counter_1830");
                MeDataTable.Columns.Remove("ertefa_badsanj");
                MeDataTable.Columns.Remove("baran6");
                MeDataTable.Columns.Remove("baran18");
                MeDataTable.Columns.Remove("baran");
                MeDataTable.Columns.Remove("star");
                MeDataTable.Columns.Remove("baran_shoroe_630");
                MeDataTable.Columns.Remove("baran_shoroe_1830");
                MeDataTable.Columns.Remove("baran_khatame_1830");
                MeDataTable.Columns.Remove("baran_khatame_630");
                MeDataTable.Columns.Remove("eshelbarf_1830");
                MeDataTable.Columns.Remove("eshelbarf_630");
                MeDataTable.Columns.Remove("tab18");
                MeDataTable.Columns.Remove("tab6");
                MeDataTable.Columns.Remove("darje hogage_1830");
                MeDataTable.Columns.Remove("darje hogage_630");
                MeDataTable.Columns.Remove("v_tab");
                MeDataTable.Columns.Remove("damatasht_1830");
                MeDataTable.Columns.Remove("damatasht_630");
                MeDataTable.Columns.Remove("abr_630");
                MeDataTable.Columns.Remove("abr_1230");
                MeDataTable.Columns.Remove("abr_1830");
                MeDataTable.Columns.Remove("ab_andazegiri_630");
                MeDataTable.Columns.Remove("ab_andazegiri_1830");
                MeDataTable.Columns.Remove("dam_5z_1830");
                int count = MeDataTable.Columns.Count;
                for (int i = 11; i < count; i++)
                {
                    MeDataTable.Columns.Remove(MeDataTable.Columns[11].ColumnName);
                }

            }
            return MeDataTable;

        }

        public CompareReportController(GenericUoW genericUoW, UserManager<User> userManager, IHostingEnvironment environment)
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


        public async Task<ActionResult> CompareReport_List(IFormFile file, long StationId, string StationName, string StartDate, string EndDate)
        {
            DataTable CompareTable = new DataTable();
            try
            {
                PersianCalendar pc = new PersianCalendar();

                User user = await _userManager.GetUserAsync(HttpContext.User);
                DataTable ElDt = Electrical_Data(StationId, StartDate, EndDate);
                DataTable MeDt = Mechanical_Data(file);
                MeDt.Columns.Add("datetime", typeof(DateTime));
                MeDt.Columns.Add("SumRain", typeof(float));
                for (int j = 0; j < MeDt.Rows.Count; j++)
                {
                    MeDt.Rows[j]["datetime"] = pc.ToDateTime(Convert.ToInt16(Convert.ToInt16(MeDt.Rows[j]["sal1"].ToString()) <= 99 ? "13" + MeDt.Rows[j]["sal1"].ToString() : "1400"), Convert.ToInt16(MeDt.Rows[j]["mah1"].ToString()), Convert.ToInt16(MeDt.Rows[j]["rooz"].ToString()), 0, 0, 0, 0);
                    MeDt.Rows[j]["SumRain"] = Convert.ToDouble(MeDt.Rows[j]["ab_baran_1830"].ToString() == "" ? "0" : MeDt.Rows[j]["ab_baran_1830"].ToString()) + Convert.ToDouble(MeDt.Rows[j]["ab_baran_630"].ToString() == "" ? "0" : MeDt.Rows[j]["ab_baran_630"].ToString());
                }
                MeDt.Columns.Remove("ab_baran_1830");
                MeDt.Columns.Remove("ab_baran_630");
                MeDt.Columns.Remove("sal1");
                MeDt.Columns.Remove("mah1");
                MeDt.Columns.Remove("rooz");

                CompareTable.Columns.Add("Datetime", typeof(DateTime));
                CompareTable.Columns.Add("StationName", typeof(string));
                CompareTable.Columns.Add(new DataColumn { ColumnName = "dama6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "dama12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "dama18", DataType = typeof(float), AllowDBNull = true });

                CompareTable.Columns.Add(new DataColumn { ColumnName = "nam6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "nam12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "nam18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "baran", DataType = typeof(float), AllowDBNull = true });

                CompareTable.Columns.Add(new DataColumn { ColumnName = "mdama6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mdama12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mdama18", DataType = typeof(float), AllowDBNull = true });

                CompareTable.Columns.Add(new DataColumn { ColumnName = "mnam6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mnam12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mnam18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mbaran", DataType = typeof(float), AllowDBNull = true });
                
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cdama6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cdama12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cdama18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cnam6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cnam12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cnam18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cbaran", DataType = typeof(float), AllowDBNull = true });



                double nam6 = 0, nam12 = 0, nam18 = 0, dama12 = 0, dama6 = 0, dama18 = 0, baran = 0;
                double mnam6 = 0, mnam12 = 0, mnam18 = 0, mdama12 = 0, mdama6 = 0, mdama18 = 0, mbaran = 0;
                DateTime date;

                DateTime stDate = StartDate.ToGreDateTime();
                DateTime enDate = EndDate.ToGreDateTime();

                DateTime Eldate, Medate;

                date = stDate;
                string strdate = "";
                for (int i = 1; i < ((TimeSpan)(enDate - stDate)).Days + 1; i++)
                {
                    DataRow[] ElfoundRows;
                    DataRow[] MefoundRows;
                    DataRow newRow = CompareTable.NewRow();

                    newRow["Datetime"] = date;
                    newRow["StationName"] = StationName;
                    newRow["nam6"] = DBNull.Value;
                    newRow["nam12"] = DBNull.Value;
                    newRow["nam18"] = DBNull.Value;
                    newRow["dama6"] = DBNull.Value;
                    newRow["dama12"] = DBNull.Value;
                    newRow["dama18"] = DBNull.Value;
                    newRow["baran"] = DBNull.Value;
                    newRow["mnam6"] = DBNull.Value;
                    newRow["mnam12"] = DBNull.Value;
                    newRow["mnam18"] = DBNull.Value;
                    newRow["mdama6"] = DBNull.Value;
                    newRow["mdama12"] = DBNull.Value;
                    newRow["mdama18"] = DBNull.Value;
                    newRow["mbaran"] = DBNull.Value;
                    newRow["cnam6"] = DBNull.Value;
                    newRow["cnam12"] = DBNull.Value;
                    newRow["cnam18"] = DBNull.Value;
                    newRow["cdama6"] = DBNull.Value;
                    newRow["cdama12"] = DBNull.Value;
                    newRow["cdama18"] = DBNull.Value;
                    newRow["cbaran"] = DBNull.Value;


                    ElfoundRows = ElDt.Select("datetime=#" + date.ToShortDateString() + "#");
                    if (ElfoundRows.Length > 0)
                    {
                        nam6 = Convert.ToDouble(ElfoundRows[0][4] == DBNull.Value ? 10000 : ElfoundRows[0][4]);
                        nam12 = Convert.ToDouble(ElfoundRows[0][5] == DBNull.Value ? 10000 : ElfoundRows[0][5]);
                        nam18 = Convert.ToDouble(ElfoundRows[0][6] == DBNull.Value ? 10000 : ElfoundRows[0][6]);
                        dama6 = Convert.ToDouble(ElfoundRows[0][1] == DBNull.Value ? 10000 : ElfoundRows[0][1]);
                        dama12 = Convert.ToDouble(ElfoundRows[0][2] == DBNull.Value ? 10000 : ElfoundRows[0][2]);
                        dama18 = Convert.ToDouble(ElfoundRows[0][3] == DBNull.Value ? 10000 : ElfoundRows[0][3]);
                        baran = Convert.ToDouble(ElfoundRows[0][7] == DBNull.Value ? 10000 : ElfoundRows[0][7]);
                        newRow["nam6"] = nam6;
                        newRow["nam12"] = nam12;
                        newRow["nam18"] = nam18;
                        newRow["dama6"] = dama6;
                        newRow["dama12"] = dama12;
                        newRow["dama18"] = dama18;
                        newRow["baran"] = baran;
                    }
                    MefoundRows = MeDt.Select("datetime=#" + date.ToShortDateString() + "#");
                    if (MefoundRows.Length > 0)
                    {
                        mnam6 = Convert.ToDouble(MefoundRows[0][4] == DBNull.Value ? 10000 : MefoundRows[0][4]);
                        mnam12 = Convert.ToDouble(MefoundRows[0][2] == DBNull.Value ? 10000 : MefoundRows[0][2]);
                        mnam18 = Convert.ToDouble(MefoundRows[0][0] == DBNull.Value ? 10000 : MefoundRows[0][0]);
                        mdama6 = Convert.ToDouble(MefoundRows[0][5] == DBNull.Value ? 10000 : MefoundRows[0][5]);
                        mdama12 = Convert.ToDouble(MefoundRows[0][3] == DBNull.Value ? 10000 : MefoundRows[0][3]);
                        mdama18 = Convert.ToDouble(MefoundRows[0][1] == DBNull.Value ? 10000 : MefoundRows[0][1]);
                        mbaran = Convert.ToDouble(MefoundRows[0][7] == DBNull.Value ? 10000 : MefoundRows[0][7]);
                        newRow["mnam6"] = mnam6;
                        newRow["mnam12"] = mnam12;
                        newRow["mnam18"] = mnam18;
                        newRow["mdama6"] = mdama6;
                        newRow["mdama12"] = mdama12;
                        newRow["mdama18"] = mdama18;
                        newRow["mbaran"] = mbaran;
                    }
                    if (mnam6 != 10000 && nam6 != 10000)
                        newRow["cnam6"] = Math.Round((mnam6 - nam6) / mnam6 * 100, 0).ToString();
                    if (mnam12 != 10000 && nam12 != 10000)
                        newRow["cnam12"] = Math.Round((mnam12 - nam12) / mnam12 * 100, 0).ToString();
                    if (mnam18 != 10000 && nam18 != 10000)
                        newRow["cnam18"] = Math.Round((mnam18 - nam18) / mnam18 * 100, 0).ToString();
                    if (mdama6 != 10000 && dama6 != 10000)
                        newRow["cdama6"] = Math.Round((mdama6 - dama6) / mdama6 * 100, 0).ToString();
                    if (mdama12 != 10000 && dama12 != 10000)
                        newRow["cdama12"] = Math.Round((mdama12 - dama12) / mdama12 * 100, 0).ToString();
                    if (mdama18 != 10000 && dama18 != 10000)
                        newRow["cdama18"] = Math.Round((mdama18 - dama18) / mdama18 * 100, 0).ToString();
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
