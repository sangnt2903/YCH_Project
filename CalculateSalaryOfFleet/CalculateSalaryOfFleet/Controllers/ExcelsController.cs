using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalculateSalaryOfFleet.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace CalculateSalaryOfFleet.Controllers
{
    public class ExcelsController : CheckAuthenticateController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly FleetsTripsContext _ctx;
        public ExcelsController(IHostingEnvironment hostingEnvironment, FleetsTripsContext ctx)
        {
            _hostingEnvironment = hostingEnvironment;
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            return View(_ctx.Excels.ToList());
        }

        public bool CheckGetExtentionsFileIsSupported(IFormFile f)
        {
            string[] extentionsFile = new string[2] { 
            ".xlsx",
            ".csv"
            };

            string extentionFile = Path.GetExtension(f.FileName).ToLower();

            if(Array.IndexOf(extentionsFile,extentionFile) != -1)
            {
                return true;
            }
            return false;
        }

        [HttpPost("Excels/UpLoadExcel")]
        public IActionResult UpLoadExcel(IFormFile fExcel, CancellationToken cancellationToken)
        {
            ResetDatabase();
            Excels fileToImport = new Excels
            {
                ExcelUploadedDate = DateTime.Now
            };

            if (fExcel != null && CheckGetExtentionsFileIsSupported(fExcel))
            {
                ViewBag.Error = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excels", fExcel.FileName);
                using (var file = new FileStream(path, FileMode.Create))
                {
                    fExcel.CopyTo(file);
                }
                fileToImport.ExcelFileName = fExcel.FileName;

                _ctx.Add(fileToImport);
                _ctx.SaveChanges();  /// Save excel file history


                // import data
                using (var stream = new MemoryStream())
                {
                    fExcel.CopyTo(stream);
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets["DynamicReport"];
                        if (workSheet != null)
                        {
                            // List to ADD database
                            List<RawData> rawDatas = new List<RawData>();
                            int totalRows = workSheet.Dimension.Rows;

                            for (int i = 6; i < totalRows; i++)
                            {
                                var dataString = workSheet.Cells[i, 46].Value;
                                string dateString = dataString != null ? dataString.ToString().Split('-')[0] + "/" + dataString.ToString().Split('-')[1] + "/" + dataString.ToString().Split('-')[2] : DateTime.MaxValue.ToString("dd/MM/yyyy");
                                rawDatas.Add(new RawData
                                {
                                    OrderNo = workSheet.Cells[i, 1].Value.ToString(),
                                    JobNo = workSheet.Cells[i, 41].Value.ToString(),
                                    DeliveryCustCode = workSheet.Cells[i, 14].Value.ToString(),
                                    DeliveryAddress = workSheet.Cells[i, 17].Value != null ? workSheet.Cells[i, 17].Value.ToString() : String.Empty,
                                    ServiceLevel = workSheet.Cells[i, 24].Value != null ? workSheet.Cells[i, 24].Value.ToString() : "servicesLevel-unknown-" + i,
                                    TruckId = workSheet.Cells[i, 40].Value != null ? workSheet.Cells[i, 40].Value.ToString() : "trucks-unknown-" + i,
                                    TruckType = workSheet.Cells[i, 43].Value != null ? workSheet.Cells[i, 43].Value.ToString() : String.Empty,
                                    TransportAgent = workSheet.Cells[i, 45].Value != null ? workSheet.Cells[i, 45].Value.ToString() : String.Empty,
                                    AtdcompleteDate = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    DriverName = workSheet.Cells[i, 50].Value != null ? workSheet.Cells[i, 50].Value.ToString() : String.Empty,
                                    DriverPhone = workSheet.Cells[i, 51].Value != null ? workSheet.Cells[i, 51].Value.ToString() : String.Empty,
                                });
                            }

                            _ctx.RawData.AddRange(rawDatas);
                            _ctx.SaveChanges();
                            // Insert data for per table
                            ImportDataToPerTable();

                            ViewBag.ImportSuccess = "Import dữ liệu thành công !";
                            return View("Index", _ctx.Excels.ToList());
                        }
                        else
                        {
                            ViewBag.Error = "Không tìm thấy sheet cần thiết của hệ thống để import dữ liệu! Vui lòng kiểm tra tên của Sheet theo yêu cầu của hệ thống !";
                            return View("Index", _ctx.Excels.ToList());
                        }
                    }

                }
            }
            else
            {
                ViewBag.Error = "Vui lòng chọn file excel hoặc định dạng file của bạn không được hỗ trợ. Lưu ý những file được hỗ trợ bao gồm : .xlsx, .csv ";
                return View("Index", _ctx.Excels.ToList());
            }
        }

        public void ResetDatabase()
        {
            var orders = _ctx.Orders.ToList();
            var jobs = _ctx.Jobs.ToList();
            var drivers = _ctx.Drivers.ToList();
            var trucks = _ctx.Trucks.ToList();
            var deliveries = _ctx.DeliveryCustomers.ToList();
            var rawDatas = _ctx.RawData.ToList();

            _ctx.Orders.RemoveRange(orders);
            _ctx.SaveChanges();
            _ctx.Jobs.RemoveRange(jobs);
            _ctx.SaveChanges();
            _ctx.Drivers.RemoveRange(drivers);
            _ctx.SaveChanges();
            _ctx.Trucks.RemoveRange(trucks);
            _ctx.SaveChanges();
            _ctx.DeliveryCustomers.RemoveRange(deliveries);
            _ctx.SaveChanges();
            _ctx.RawData.RemoveRange(rawDatas);
            _ctx.SaveChanges();
        }


        //TODO: IMPORT RAWDATA
        //public IActionResult ImportRawData(int excelCode)
        //{
        //    //ResetDatabase();
        //    string rootFolder = _hostingEnvironment.WebRootPath;
        //    string fileName = _ctx.Excels.SingleOrDefault(p => p.ExcelCode == excelCode).ExcelFileName;
        //    FileInfo file = new FileInfo(Path.Combine(rootFolder, "Excels", @fileName));
        //    using (ExcelPackage package = new ExcelPackage(file))
        //    {
        //        ExcelWorksheet workSheet = package.Workbook.Worksheets["DynamicReport"];
        //        if (workSheet != null)
        //        {
        //            // List to ADD database
        //            List<RawData> rawDatas = new List<RawData>();
        //            int totalRows = workSheet.Dimension.Rows;
                     
        //            for (int i = 5; i < totalRows; i++)
        //            {
        //                var dataString = workSheet.Cells[i, 46].Value;
        //                string dateString = dataString != null? dataString.ToString().Split('-')[0] + "/" + dataString.ToString().Split('-')[1] + "/" + dataString.ToString().Split('-')[2] : DateTime.MaxValue.ToString("dd/MM/yyyy");
        //                rawDatas.Add(new RawData
        //                {
        //                    OrderNo = workSheet.Cells[i, 1].Value.ToString(),
        //                    JobNo = workSheet.Cells[i, 41].Value.ToString(),
        //                    DeliveryCustCode = workSheet.Cells[i, 14].Value.ToString(),
        //                    DeliveryAddress = workSheet.Cells[i, 17].Value != null ? workSheet.Cells[i, 17].Value.ToString(): String.Empty,
        //                    ServiceLevel = workSheet.Cells[i, 24].Value != null ? workSheet.Cells[i, 24].Value.ToString(): "servicesLevel-unknown-"+i,
        //                    TruckId = workSheet.Cells[i, 40].Value != null? workSheet.Cells[i, 40].Value.ToString():"trucks-unknown-"+i ,
        //                    TruckType = workSheet.Cells[i, 43].Value != null ? workSheet.Cells[i, 43].Value.ToString(): String.Empty,
        //                    TransportAgent = workSheet.Cells[i, 45].Value!= null ? workSheet.Cells[i, 45].Value.ToString() : String.Empty,
        //                    AtdcompleteDate = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture),
        //                    DriverName = workSheet.Cells[i, 50].Value != null ? workSheet.Cells[i, 50].Value.ToString(): String.Empty,
        //                    DriverPhone = workSheet.Cells[i, 51].Value != null ? workSheet.Cells[i, 51].Value.ToString() : String.Empty,
        //                });
        //            }

        //            _ctx.RawData.AddRange(rawDatas);
        //            _ctx.SaveChanges();
        //            // Insert data for per table
        //            ImportDataToPerTable();

        //            ViewBag.ImportSuccess = "Import dữ liệu thành công !";
        //            return View("Index", _ctx.Excels.ToList());
        //        } else
        //        {
        //            ViewBag.Error = "Không tìm thấy sheet cần thiết của hệ thống để import dữ liệu! Vui lòng kiểm tra tên của Sheet theo yêu cầu của hệ thống !";
        //            return View("Index", _ctx.Excels.ToList());
        //        }
        //    }
        //}


        // TODO: IMPORT DATA PER TABLE
        public void ImportDataToPerTable()
        {
            List<Trucks> trucks = new List<Trucks>();
            List<Drivers> drivers = new List<Drivers>();
            List<Jobs> jobs = new List<Jobs>();
            List<Orders> orders = new List<Orders>();
            List<DeliveryCustomers> deliveries = new List<DeliveryCustomers>();
            List<RawData> rawDatas = _ctx.RawData.ToList();
            for (int i = 0; i < rawDatas.Count(); i++)
            {
                if (rawDatas[i].TruckId != null && trucks.SingleOrDefault(p => p.TruckId.ToLower().ToString() == rawDatas[i].TruckId.ToLower().ToString()) == null && _ctx.Trucks.SingleOrDefault(p => p.TruckId.ToLower().ToString() == rawDatas[i].TruckId.ToLower().ToString()) == null)
                {
                    trucks.Add(new Trucks
                    {
                        TruckId = rawDatas[i].TruckId.ToString(),
                        TruckType = rawDatas[i].TruckType.ToString()
                    });
                }

                if (rawDatas[i].DriverPhone != null && drivers.SingleOrDefault(p => p.DriverIcno.ToLower().ToString() == rawDatas[i].DriverPhone.ToLower().ToString()) == null && _ctx.Drivers.SingleOrDefault(p => p.DriverIcno.ToLower() == rawDatas[i].DriverPhone.ToLower().ToString()) == null)
                {
                    drivers.Add(new Drivers
                    {
                        DriverIcno = rawDatas[i].DriverPhone.ToString(),
                        DriverName = rawDatas[i].DriverName.ToString(),
                        DriverPhone = rawDatas[i].DriverPhone.ToString()
                    });
                }

                if (rawDatas[i].JobNo != null && jobs.SingleOrDefault(p => p.JobNo.ToLower() == rawDatas[i].JobNo.ToLower().ToString()) == null && _ctx.Jobs.SingleOrDefault(p => p.JobNo.ToLower() == rawDatas[i].JobNo.ToLower().ToString()) == null)
                {
                    jobs.Add(new Jobs
                    {
                        JobNo = rawDatas[i].JobNo.ToString(),
                        DriverIcno = rawDatas[i].DriverPhone,
                        TruckId = rawDatas[i].TruckId
                    });
                }

                if (rawDatas[i].DriverPhone != null)
                {
                    orders.Add(new Orders
                    {
                        OrderNo = rawDatas[i].OrderNo.ToString(),
                        JobNo = rawDatas[i].JobNo.ToString(),
                        TranportAgent = rawDatas[i].TransportAgent.ToString(),
                        DeliveryCustCode = rawDatas[i].DeliveryCustCode.ToString(),
                        AtdcompleteDate = rawDatas[i].AtdcompleteDate.Value,
                    });
                }

                if (rawDatas[i].DeliveryCustCode != null && deliveries.SingleOrDefault(p => p.DeliveryCustCode.ToLower() == rawDatas[i].DeliveryCustCode.ToLower().ToString()) == null && _ctx.DeliveryCustomers.SingleOrDefault(p => p.DeliveryCustCode.ToLower() == rawDatas[i].DeliveryCustCode.ToLower().ToString()) == null)
                {
                    deliveries.Add(new DeliveryCustomers
                    {
                        DeliveryCustCode = rawDatas[i].DeliveryCustCode.ToString(),
                        DeliveryAddress = rawDatas[i].DeliveryAddress.ToString(),
                        ServiceLevel = rawDatas[i].ServiceLevel.ToString(),
                    });
                }
            }

            _ctx.Trucks.AddRange(trucks);
            _ctx.SaveChanges();
            _ctx.Drivers.AddRange(drivers);
            _ctx.SaveChanges();
            _ctx.Jobs.AddRange(jobs);
            _ctx.SaveChanges();
            _ctx.DeliveryCustomers.AddRange(deliveries);
            _ctx.SaveChanges();
            _ctx.Orders.AddRange(orders);
            _ctx.SaveChanges();
        }
    }
}