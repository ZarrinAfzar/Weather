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
    [Authorize(Roles = "AdminiStratore,CompareReportJame")]
    public class CompareReportJameController : Controller
    {

        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _environment;

        [HttpPost]
        public DataTable Electrical_Data(IFormFile file, string startDate, string endDate)
        {
            DataTable ElDataTable = new DataTable();
            string conString = string.Empty;



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
                            ElDataTable = Utility.ConvertXSLXtoDataTable(conString,"Sheet1");

                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES'";
                            ElDataTable = Utility.ConvertXSLXtoDataTable(conString,"Sheet1");

                            break;
                    }
                }
            }
            if (ElDataTable.Rows.Count > 0)
            {
                ElDataTable.Columns.Remove("Second");
                ElDataTable.Columns.Remove("...PRS - 16ch(mb)");
                ElDataTable.Columns.Remove("...PRSN- 16ch(mb)");
                ElDataTable.Columns.Remove("...PRSX- 16ch(mb)");
                ElDataTable.Columns.Remove("...PRSA- 16ch(mb)");
                ElDataTable.Columns.Remove("...WSP - 16ch(m/s)");
                ElDataTable.Columns.Remove("...WSPN - 16ch(m/s)");
                ElDataTable.Columns.Remove("...WSPX - 16ch(m/s)");
                ElDataTable.Columns.Remove("...WSPA - 16ch(m/s)");
                ElDataTable.Columns.Remove("...HUMX - 16ch(%)");
                ElDataTable.Columns.Remove("...HUMN - 16ch(%)");
                ElDataTable.Columns.Remove("...HUMA - 16ch(%)");
                ElDataTable.Columns.Remove("...TMPN - 16ch(c)");
                ElDataTable.Columns.Remove("...TMPX - 16ch(c)");
                ElDataTable.Columns.Remove("...TMPA - 16ch(c)");
                ElDataTable.Columns.Remove("...WDR - 16ch(un)");
                ElDataTable.Columns.Remove("...WDRX - 16ch(un)");
                ElDataTable.Columns.Remove("...WDRA - 16ch(un)");
                ElDataTable.Columns.Remove("...RAD - 16ch(un)");
                ElDataTable.Columns.Remove("...RADN - 16ch(un)");
                ElDataTable.Columns.Remove("...RADX - 16ch(un)");
                ElDataTable.Columns.Remove("...RADA - 16ch(un)");
              
                ElDataTable.Columns.Remove("...BAT - 16ch(Volt)");
                ElDataTable.Columns.Remove("...BATN - 16ch(Volt)");
                ElDataTable.Columns.Remove("...BATX - 16ch(Volt)");
                ElDataTable.Columns.Remove("...BATA - 16ch(Volt)");
                ElDataTable.Columns.Remove("...RAN_TOT - 16ch(mm)");
                ElDataTable.Columns.Remove("...RAN_12 - 16ch(mm)");
                ElDataTable.Columns.Remove("EVAP TEST(MM)");
                ElDataTable.Columns.Remove("nEVAP TEST(mm)");
                ElDataTable.Columns.Remove("...EVP - 16ch(p)");
                //		...HUM - 16ch(%)	...TMP - 16ch(c)			...RAN_24 - 16ch(mm)				 
            }


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
                            MeDataTable = Utility.ConvertXSLXtoDataTable(conString,"Sheet1");

                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES'";
                            MeDataTable = Utility.ConvertXSLXtoDataTable(conString,"Sheet1");

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


            //DataTable dt = new DataTable();

            //dt.Columns.Add("تاریخ", typeof(DateTime));
            //dt.Columns.Add("دما الکتریکال 6:30", typeof(float));
            //dt.Columns.Add("دما الکتریکال 12:30", typeof(float));
            //dt.Columns.Add("دما الکتریکال 18:30", typeof(float));
            //dt.Columns.Add("رطوبت الکتریکال 6:30", typeof(float));
            //dt.Columns.Add("رطوبت الکتریکال 12:30", typeof(float));
            //dt.Columns.Add("رطوبت الکتریکال 18:30", typeof(float));
            //dt.Columns.Add("باران الکتریکال", typeof(float));
            //dt.Columns.Add("دما مکانیکال 6:30", typeof(float));
            //dt.Columns.Add("دما مکانیکال 12:30", typeof(float));
            //dt.Columns.Add("دما مکانیکال 18:30", typeof(float));
            //dt.Columns.Add("رطوبت مکانیکال 6:30", typeof(float));
            //dt.Columns.Add("رطوبت مکانیکال 12:30", typeof(float));
            //dt.Columns.Add("رطوبت مکانیکال 18:30", typeof(float));
            //dt.Columns.Add("باران مکانیکال", typeof(float));
            //string export = stationId + "_" + DateTime.Now.ToPeString();
            //DateTime days = startDate.ToGreDateTime();
            //for (int i = 1; i <= ((TimeSpan)(enDate - stDate)).Days + 1; i++)
            //{
            //    dt.Rows.Add(days);
            //    days.AddDays(1);
            //}

            //byte[] fileContents;
            //using (var package = new ExcelPackage())
            //{
            //    var worksheet = package.Workbook.Worksheets.Add(export);
            //    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
            //    fileContents = package.GetAsByteArray();
            //}
            //if (fileContents == null || fileContents.Length == 0)
            //{
            //    return NotFound();
            //}
            //return File(
            //    fileContents: fileContents,
            //    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            //    fileDownloadName: export + ".xlsx"
            //);
            //IWorkbook workbook = new HSSFWorkbook();

            //ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            ////make a header row
            //IRow row1 = sheet1.CreateRow(0);

            //for (int j = 0; j < dt.Columns.Count; j++)
            //{

            //    ICell cell = row1.CreateCell(j);
            //    String columnName = dt.Columns[j].ToString();
            //    cell.SetCellValue(columnName);
            //}

            ////loops through data
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    IRow row = sheet1.CreateRow(i + 1);
            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {

            //        ICell cell = row.CreateCell(j);
            //        String columnName = dt.Columns[j].ToString();
            //        cell.SetCellValue(dt.Rows[i][columnName].ToString());
            //    }
            //}
            //string Extension =
            //                       System.IO.Path.GetExtension("xlsx");
            //using (var exportData = new MemoryStream())
            //{
            //    Response.Clear();
            //    workbook.Write(exportData);
            //    if (Extension == "xlsx") //xlsx file format
            //    {
            //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //        Response.Headers.Add("Content-Disposition", string.Format("attachment;filename={0}", "example.xlsx"));
            //        Response.Body.WriteByte(exportData);
            //    }
            //    else if (Extension == "xls")  //xls file format
            //    {
            //        Response.ContentType = "application/vnd.ms-excel";
            //        Response.Headers.Add"Content-Disposition", string.Format("attachment;filename={0}", "example.xls"));
            //        Response.Body.WriteByte(exportData.GetBuffer());
            //    }

            //}


        }
        public static DataTable MergeTablesByIndex(DataTable t1, DataTable t2)
        {
            if (t1 == null || t2 == null) throw new ArgumentNullException("t1 or t2", "Both tables must not be null");

            DataTable t3 = t1.Clone();  // first add columns from table1
            foreach (DataColumn col in t2.Columns)
            {
                string newColumnName = col.ColumnName;
                int colNum = 1;
                while (t3.Columns.Contains(newColumnName))
                {
                    newColumnName = string.Format("{0}_{1}", col.ColumnName, ++colNum);
                }
                t3.Columns.Add(newColumnName, col.DataType);
            }
            var mergedRows = t1.AsEnumerable().Zip(t2.AsEnumerable(),
                (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());
            foreach (object[] rowFields in mergedRows)
                t3.Rows.Add(rowFields);

            return t3;
        }
        public CompareReportJameController(GenericUoW genericUoW, UserManager<User> userManager, IHostingEnvironment environment)
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


        public async Task<ActionResult> CompareReportJame_List(IFormFile ELfile, IFormFile MEfile,  string StartDate, string EndDate)
        {
            DataTable CompareTable = new DataTable();
            try
            {
                PersianCalendar pc = new PersianCalendar();

                User user = await _userManager.GetUserAsync(HttpContext.User);
                DataTable ElDt = Electrical_Data(ELfile, StartDate, EndDate);
                DataTable MeDt = Mechanical_Data(MEfile);
                MeDt.Columns.Add("datetime", typeof(DateTime));
                for (int j = 0; j < MeDt.Rows.Count; j++)
                {
                    //Year	Month	Day	Hour	Minute

                    MeDt.Rows[j]["datetime"] = pc.ToDateTime(Convert.ToInt16(Convert.ToInt16(MeDt.Rows[j]["sal1"].ToString()) <= 99 ? "13" + MeDt.Rows[j]["sal1"].ToString() : "1400"), Convert.ToInt16(MeDt.Rows[j]["mah1"].ToString()), Convert.ToInt16(MeDt.Rows[j]["rooz"].ToString()), 0, 0, 0, 0);
                    MeDt.Rows[j]["SumRain"] = Convert.ToDouble(MeDt.Rows[j]["ab_baran_1830"].ToString() == "" ? "0" : MeDt.Rows[j]["ab_baran_1830"].ToString()) + Convert.ToDouble(MeDt.Rows[j]["ab_baran_630"].ToString() == "" ? "0" : MeDt.Rows[j]["ab_baran_630"].ToString());
                }
                MeDt.Columns.Remove("ab_baran_1830");
                MeDt.Columns.Remove("ab_baran_630");
                MeDt.Columns.Remove("sal1");
                MeDt.Columns.Remove("mah1");
                MeDt.Columns.Remove("rooz");

                CompareTable.Columns.Add("Datetime", typeof(DateTime));
                CompareTable.Columns.Add(new DataColumn { ColumnName = "nam6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "nam12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "nam18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "dama6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "dama12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "dama18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "baran", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mnam6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mnam12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mnam18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mdama6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mdama12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mdama18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mbaran", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cnam6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cnam12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cnam18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cdama6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cdama12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cdama18", DataType = typeof(float), AllowDBNull = true });
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
                        mdama12 = Convert.ToDouble(MefoundRows[0][4] == DBNull.Value ? 10000 : MefoundRows[0][4]);
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

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(IFormFile ELfile, IFormFile MEfile, string StationName, string StartDate, string EndDate)
        {
            DataTable CompareTable = new DataTable();
            try
            {
                PersianCalendar pc = new PersianCalendar();

                User user = await _userManager.GetUserAsync(HttpContext.User);
                DataTable ElDt = Electrical_Data(ELfile, StartDate, EndDate);
                DataTable MeDt = Mechanical_Data(MEfile);
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
                CompareTable.Columns.Add(new DataColumn { ColumnName = "nam6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "nam12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "nam18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "dama6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "dama12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "dama18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "baran", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mnam6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mnam12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mnam18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mdama6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mdama12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mdama18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "mbaran", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cnam6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cnam12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cnam18", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cdama6", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cdama12", DataType = typeof(float), AllowDBNull = true });
                CompareTable.Columns.Add(new DataColumn { ColumnName = "cdama18", DataType = typeof(float), AllowDBNull = true });
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
                        mdama12 = Convert.ToDouble(MefoundRows[0][4] == DBNull.Value ? 10000 : MefoundRows[0][4]);
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
            byte[] fileContents;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(StationName);
                worksheet.Cells["A1"].LoadFromDataTable(CompareTable, true);
                fileContents = package.GetAsByteArray();
            }
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: StationName + ".xlsx"
            );



        }
    }
}
