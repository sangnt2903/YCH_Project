using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using ASPCore_Final.Models;
using CalculateSalaryOfFleet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.Style;

namespace CalculateSalaryOfFleet.Controllers
{
    public class StatisticalsController : CheckAuthenticateController
    {
        private readonly FleetsTripsContext _ctx;
        public StatisticalsController(FleetsTripsContext ctx)
        {
            _ctx = ctx;
        }

        public List<DataResult> listReport
        {
            get{
                List<DataResult> dataResults = HttpContext.Session.GetComplexData<List<DataResult>>("listData");
                if (dataResults == default(List<DataResult>))
                {
                    dataResults = new List<DataResult>();
                }
                return dataResults;
            } 
        }
        public IActionResult Index()
        {
            List<string> transportAgent = new List<string>();
            transportAgent.Add("All");
            transportAgent.AddRange(_ctx.Orders.Select(p => p.TranportAgent).Distinct().ToList());
            ViewData["transportAgent"] = transportAgent;
            return View(listReport);
        }

        public double CalculateTrip(int numberOfOrders, List<Rules> rules)
        {
            if (numberOfOrders != 0)
            {
                foreach (var r in rules)
                {
                    if (r.RuleTo != null)
                    {
                        if (numberOfOrders >= r.RuleFrom && numberOfOrders <= r.RuleTo)
                        {
                            return r.RuleNumber;
                        }
                    }
                    else
                    {
                        if (numberOfOrders > r.RuleFrom) return r.RuleNumber;
                    }
                }
            }
            return 0;
        }
        public long CalculateMoney(double totalTrip, List<MoneyJob> money, List<Rules> rulesMoney, List<MoneyByBigCar> moneyByBigCar, List<TypeJob> typeJobs, List<TruckWeightType> truckWeightTypes, List<TruckSize> truckSizes, int jobOrder, bool isFar, string truckType, string serviceLevel) //jobOrder : thứ tự job
        {
            long result = 0;
            if (!String.IsNullOrEmpty(truckType))
            {
                if(truckSizes.SingleOrDefault(p=> p.TruckType == truckType).TruckSizeId == 1) // 1: small truck 2: big truck 3: container
                {
                    if (isFar)
                    {
                        result = Convert.ToInt32(Convert.ToDouble(money.LastOrDefault().PerformenceMoney) * totalTrip);
                    }
                    else
                    {
                        if (jobOrder == 1)
                        {
                            result = rulesMoney.SingleOrDefault(p => p.RuleNumber == totalTrip).MoneyFirstJob;
                        }
                        else
                        {
                            result = Convert.ToInt32(Convert.ToDouble(money.SingleOrDefault(p => p.JobOrder == 2).PerformenceMoney) * totalTrip);
                        }
                    }
                }
                else if(truckSizes.SingleOrDefault(p => p.TruckType == truckType).TruckSizeId == 2) // Xe lớn
                {
                    if (jobOrder == 1)
                    {
                        if(serviceLevel == typeJobs.FirstOrDefault().TypeJob1)
                        {
                            result = Convert.ToInt32(moneyByBigCar.SingleOrDefault(p => p.TripNo == jobOrder).MoneyOfServiceLevelHcmc);
                        }
                        else if (serviceLevel == typeJobs.LastOrDefault().TypeJob1)
                        {
                            result = Convert.ToInt32(moneyByBigCar.SingleOrDefault(p => p.TripNo == jobOrder).MoneyOfServiceLevelHcmp);
                        }
                    }
                    else
                    {
                        if (serviceLevel == typeJobs.FirstOrDefault().TypeJob1)
                        {
                            result = Convert.ToInt32(moneyByBigCar.SingleOrDefault(p => p.TripNo == 2).MoneyOfServiceLevelHcmc);
                        }
                        else if (serviceLevel == typeJobs.LastOrDefault().TypeJob1)
                        {
                            result = Convert.ToInt32(moneyByBigCar.SingleOrDefault(p => p.TripNo == 2).MoneyOfServiceLevelHcmp);
                        }
                    }
                } else // container
                {
                    // improve sau
                }
            } 
            return result;
        }
        public List<DescriptionPerJob> GetDescriptionPerJobs(string transportAgent, string driverIcNo, DateTime dateTime)
        {
            List<Rules> rules = _ctx.Rules.OrderBy(p => p.RuleNumber).ToList();
            var res = from dr in _ctx.Drivers
                      join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                      join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                      where ord.TranportAgent == transportAgent && dr.DriverIcno == driverIcNo & ord.AtdcompleteDate == dateTime
                      group ord by ord.JobNo into ordGroup
                      select new DescriptionPerJob
                      {
                          JobNo = ordGroup.Key,
                          DropPointPerJob = ordGroup.Select(p => p.DeliveryCustCode).Distinct().Count(),
                          HeSoTai = CalculateTrip(ordGroup.Select(p => p.DeliveryCustCode).Distinct().Count(), rules)
                      };
            return res.ToList();
        }
        public IActionResult GetDataReportByFilter(string startDate, string endDate, string transportAgent, string driverIcNo)
        {
            List<DataResult> results = new List<DataResult>();
            List<MoneyJob> money = _ctx.MoneyJob.OrderBy(p => p.PerformenceMoney).ToList();
            List<Rules> rules = _ctx.Rules.OrderBy(p => p.RuleNumber).ToList();
            string filter = String.Empty;
            string start = startDate != null ? DateTime.Parse(startDate).ToString("dd/MM/yyyy") : null;
            string end = endDate != null ?  DateTime.Parse(endDate).ToString("dd/MM/yyyy") : null;

            var DR_JB_ORD = from dr in _ctx.Drivers
                            join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                            join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                            select new DR_JB_ORD
                            {
                                DriverICNo = dr.DriverIcno,
                                DriverName = dr.DriverName,
                                ATD_CompleteDate = ord.AtdcompleteDate,
                                JobNo = ord.JobNo,
                                DeliveryCustCode = ord.DeliveryCustCode,
                                TransportAgent = ord.TranportAgent
                            };

            if (!String.IsNullOrEmpty(driverIcNo)) // có dữ liệu tài xế
            {
                if (!String.IsNullOrEmpty(transportAgent))
                {
                    if (String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where dr.DriverIcno == driverIcNo.ToString() && ord.TranportAgent == transportAgent
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };

                        string driverName = DR_JB_ORD.ToList().Count() > 0 ? _ctx.Drivers.Find(driverIcNo).DriverName.ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(driverName))
                        {
                            filter = "Tài xế: " + driverName + " Đơn vị vận chuyển: " + transportAgent;
                        }
                        else filter =  "Đơn vị vận chuyển: " + transportAgent;
                    }
                    else if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where dr.DriverIcno == driverIcNo.ToString() && ord.TranportAgent == transportAgent && ord.AtdcompleteDate >= DateTime.Parse(startDate) && ord.AtdcompleteDate <= DateTime.Parse(endDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                        string driverName = DR_JB_ORD.ToList().Count() > 0 ? _ctx.Drivers.Find(driverIcNo).DriverName.ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(driverName))
                        {
                            filter = "Tài xế: " + driverName + " Đơn vị vận chuyển: " + transportAgent + " Từ ngày: " + start + " đến ngày " + end;
                        }
                        else filter ="Thông tin tài xế "+ driverIcNo + " Đơn vị vận chuyển: " + transportAgent + " Từ ngày: " + start + " đến ngày " + end + " không thể tìm thấy";
                    }
                    else if (!String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where dr.DriverIcno == driverIcNo && ord.TranportAgent == transportAgent  && ord.AtdcompleteDate >= DateTime.Parse(startDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };

                        string driverName = DR_JB_ORD.ToList().Count() > 0 ? _ctx.Drivers.Find(driverIcNo).DriverName.ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(driverName))
                        {
                            filter = "Tài xế: " + driverName + " Đơn vị vận chuyển: " + transportAgent + " Từ ngày: " + start;
                        }
                        else filter = "Thông tin tài xế "+ driverIcNo + " Đơn vị vận chuyển: " + transportAgent + " Từ ngày: " + start;
                    }
                    else
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where dr.DriverIcno == driverIcNo && ord.TranportAgent == transportAgent && ord.AtdcompleteDate <= DateTime.Parse(endDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                        string driverName = DR_JB_ORD.ToList().Count() > 0 ? _ctx.Drivers.Find(driverIcNo).DriverName.ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(driverName))
                        {
                            filter = "Tài xế: " + driverName + " Đơn vị vận chuyển: " + transportAgent + " đến ngày " + end;
                        }
                        else filter = "Thông tin tài xế "+ driverIcNo + " Đơn vị vận chuyển: " + transportAgent + " đến ngày " + end + " không thể tìm thấy";
                    }
                } // có dữ liệu đơn vị vận chuyển
                else
                {
                    if (String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where dr.DriverIcno == driverIcNo
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };

                        string driverName = DR_JB_ORD.ToList().Count() > 0 ? _ctx.Drivers.Find(driverIcNo).DriverName.ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(driverName))
                        {
                            filter = "Tài xế: " + driverName;
                        }
                        else filter = "Thông tin tài xế " + driverIcNo + " không thể tìm thấy";
                    }
                    else if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where dr.DriverIcno == driverIcNo && ord.AtdcompleteDate >= DateTime.Parse(startDate) && ord.AtdcompleteDate <= DateTime.Parse(endDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };

                        string driverName = DR_JB_ORD.ToList().Count() > 0 ? _ctx.Drivers.Find(driverIcNo).DriverName.ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(driverName))
                        {
                            filter = "Tài xế: " + driverName + "Từ ngày: " + start + " đến ngày " + end;
                        }
                        else filter ="Thông tin tài xế "+ driverIcNo+ " Từ ngày: " + start + " đến ngày " + end + " không thể tìm thấy kết quả";
                    }
                    else if (!String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where dr.DriverIcno == driverIcNo && ord.AtdcompleteDate >= DateTime.Parse(startDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                        string driverName = DR_JB_ORD.ToList().Count() > 0 ? _ctx.Drivers.Find(driverIcNo).DriverName.ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(driverName))
                        {
                            filter = "Tài xế: " + driverName + " Từ ngày: " + start;
                        }
                        else filter = "Thông tin tài xế "+ driverIcNo +" Từ ngày: " + start + " không thể tìm thấy";
                    }
                    else
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where dr.DriverIcno == driverIcNo && ord.AtdcompleteDate <= DateTime.Parse(endDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };

                        string driverName = DR_JB_ORD.ToList().Count() > 0 ? _ctx.Drivers.Find(driverIcNo).DriverName.ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(driverName))
                        {
                            filter = "Tài xế: " + driverName + " đến ngày " + end;
                        }
                        else filter = "Thông tin tài xế " + driverIcNo + " đến ngày " + end + "không thể tìm thấy";
                    }
                } // không có đơn vị vận chuyển
            } else // không có dữ liệu tài xế
            {
                if (!String.IsNullOrEmpty(transportAgent))
                {
                    if (String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where ord.TranportAgent == transportAgent 
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                        filter = "Đơn vị vận chuyển " + transportAgent;
                    }
                    else if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where ord.TranportAgent == transportAgent  && ord.AtdcompleteDate >= DateTime.Parse(startDate) && ord.AtdcompleteDate <= DateTime.Parse(endDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                        filter = "Đơn vị vận chuyển: " + transportAgent + " Từ ngày: " + start + " đến ngày " + end;
                    }
                    else if (!String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where ord.TranportAgent == transportAgent  && ord.AtdcompleteDate >= DateTime.Parse(startDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };

                        filter = "Đơn vị vận chuyển: " + transportAgent + " Từ ngày: " + start;
                    }
                    else
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where ord.TranportAgent == transportAgent &&  ord.AtdcompleteDate <= DateTime.Parse(endDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                        filter = "Đơn vị vận chuyển: " + transportAgent + " Đến ngày " + end;
                    }
                } //  có dữ liệu đơn vị vận chuyển
                else
                {
                    if (String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                    }
                    else if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where ord.AtdcompleteDate >= DateTime.Parse(startDate) && ord.AtdcompleteDate <= DateTime.Parse(endDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                        filter = "Từ ngày: " + start + " đến ngày " + end;
                    }
                    else if (!String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where ord.AtdcompleteDate >= DateTime.Parse(startDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                        filter = "Từ ngày: " + start;
                    }
                    else
                    {
                        DR_JB_ORD = from dr in _ctx.Drivers
                                    join jb in _ctx.Jobs on dr.DriverIcno equals jb.DriverIcno
                                    join ord in _ctx.Orders on jb.JobNo equals ord.JobNo
                                    where ord.AtdcompleteDate <= DateTime.Parse(endDate)
                                    select new DR_JB_ORD
                                    {
                                        DriverICNo = dr.DriverIcno,
                                        DriverName = dr.DriverName,
                                        ATD_CompleteDate = ord.AtdcompleteDate,
                                        JobNo = ord.JobNo,
                                        DeliveryCustCode = ord.DeliveryCustCode,
                                        TransportAgent = ord.TranportAgent
                                    };
                        filter = "Đến ngày " + end;
                    }
                } // không có đơn vị vận chuyển
            }
            

            var DR_JB_ORD_GROUP = from res in DR_JB_ORD
                                  group res by new { res.DriverICNo, res.DriverName, res.TransportAgent, res.ATD_CompleteDate, res.JobNo } into resGroup
                                  select new DR_JB_ORD_GROUP
                                  {
                                      DriverICNo = resGroup.Key.DriverICNo,
                                      DriverName = resGroup.Key.DriverName,
                                      TransportAgent = resGroup.Key.TransportAgent,
                                      ATD_CompleteDate = resGroup.Key.ATD_CompleteDate,
                                      JobNo = resGroup.Key.JobNo,
                                      TotalDropPointInJob = resGroup.Select(p => p.DeliveryCustCode).Distinct().Count()
                                  };

            var data = from i in DR_JB_ORD_GROUP
                       orderby i.DriverName
                       group i by new { i.DriverICNo, i.DriverName, i.TransportAgent, i.ATD_CompleteDate } into dGroup
                       select new DataResult
                       {
                           DriverIcNo = dGroup.Key.DriverICNo,
                           DriverName = dGroup.Key.DriverName,
                           TransportAgent = dGroup.Key.TransportAgent,
                           ATD_Date = dGroup.Key.ATD_CompleteDate,
                           TotalJobs = dGroup.Select(p => p.JobNo).Count(),
                           TotalDropPoint = dGroup.Select(p => p.TotalDropPointInJob).Sum(),
                           descriptionPerJobs = null,
                       };

            
            foreach (var i in data.ToList())
            {
                long totalMoney = 0;
                foreach (var j in ListToCalculateMoney(i.DriverIcNo, i.ATD_Date))
                {
                    totalMoney += j.Money;
                }

                i.TotalMoney = totalMoney;
                i.descriptionPerJobs = GetDescriptionPerJobs(i.TransportAgent, i.DriverIcNo, i.ATD_Date);
                results.Add(i);
            } 

            HttpContext.Session.SetString("filter", filter);
            HttpContext.Session.SetComplexData("listData", results);

            return RedirectToAction("Index");
        }
        public IActionResult DetailJobsOfDay(string driverIcNo, DateTime atdCompleteDate)
        {
            List<Rules> rules = _ctx.Rules.OrderBy(p => p.RuleNumber).ToList();
            List<MoneyJob> money = _ctx.MoneyJob.OrderBy(p=> p.PerformenceMoney).ToList();
            List<MoneyByBigCar> moneyByBigCars = _ctx.MoneyByBigCar.OrderBy(p => p.TripNo).ToList();
            List<TypeJob> typeJobs = _ctx.TypeJob.ToList();
            List<TruckWeightType> truckWeightTypes = _ctx.TruckWeightType.ToList();
            List<TruckSize> truckSizes = _ctx.TruckSize.ToList();

            var groupList = from ord in _ctx.Orders
                            join jb in _ctx.Jobs on ord.JobNo equals jb.JobNo
                            join dr in _ctx.Drivers on jb.DriverIcno equals dr.DriverIcno
                            join tr in _ctx.Trucks on jb.TruckId equals tr.TruckId
                            join dc in _ctx.DeliveryCustomers on ord.DeliveryCustCode equals dc.DeliveryCustCode
                            where jb.DriverIcno == driverIcNo && ord.AtdcompleteDate == atdCompleteDate
                            select new JobViewModelGroup
                            {
                                DriverIcNo = dr.DriverIcno,
                                JobNo = jb.JobNo,
                                ATD_Date = ord.AtdcompleteDate,
                                TruckId = jb.TruckId,
                                TruckType = tr.TruckType,
                                DeliveryCustCode = ord.DeliveryCustCode,
                                ServiceLevel = dc.ServiceLevel
                          };

            var res = from r in groupList
                      group r by new { r.DriverIcNo, r.JobNo, r.TruckId, r.TruckType, r.ATD_Date, r.ServiceLevel } into rGroup
                      select new JobModelView
                      {
                          JobNo = rGroup.Key.JobNo,
                          TruckId = rGroup.Key.TruckId,
                          TruckType = rGroup.Key.TruckType,
                          ATD_Date = rGroup.Key.ATD_Date,
                          ServiceLevel = rGroup.Key.ServiceLevel,
                          NumberOfDropPoint = rGroup.Select(p => p.DeliveryCustCode).Distinct().Count(),
                          NumberOfTrips = CalculateTrip(rGroup.Select(p => p.DeliveryCustCode).Distinct().Count(), rules),
                          Money = 0
                      };

            List<JobModelView> results = res.ToList();
            List<DesLocateException> desLocateExceptions = _ctx.DesLocateException.ToList();
            for (int i = 0; i < results.Count(); i++)
            {
                bool isFar = checkJobAddressFar(groupList.Distinct().ToList(), desLocateExceptions, String.Empty, i);
                results[i].Money = CalculateMoney(res.ToList()[i].NumberOfTrips, money, rules, moneyByBigCars, typeJobs, truckWeightTypes, truckSizes, i + 1, isFar, results[i].TruckType, results[i].ServiceLevel); // 1: job đầu, 2: job sau
            }

            Drivers driver = _ctx.Drivers.SingleOrDefault(p => p.DriverIcno == driverIcNo);
            string driverString = driverIcNo + "-" + driver.DriverName;
            ViewBag.driver = driverString;
            return View(results);
        }

        public List<JobModelView> ListToCalculateMoney(string driverIcNo, DateTime atdCompleteDate)
        {
            List<Rules> rules = _ctx.Rules.OrderBy(p => p.RuleNumber).ToList();
            List<MoneyJob> money = _ctx.MoneyJob.OrderBy(p => p.PerformenceMoney).ToList();
            List<MoneyByBigCar> moneyByBigCars = _ctx.MoneyByBigCar.OrderBy(p => p.TripNo).ToList();
            List<TypeJob> typeJobs = _ctx.TypeJob.ToList();
            List<TruckWeightType> truckWeightTypes = _ctx.TruckWeightType.ToList();
            List<TruckSize> truckSizes = _ctx.TruckSize.ToList();

            var groupList = from ord in _ctx.Orders
                            join jb in _ctx.Jobs on ord.JobNo equals jb.JobNo
                            join dr in _ctx.Drivers on jb.DriverIcno equals dr.DriverIcno
                            join tr in _ctx.Trucks on jb.TruckId equals tr.TruckId
                            join dc in _ctx.DeliveryCustomers on ord.DeliveryCustCode equals dc.DeliveryCustCode
                            where jb.DriverIcno == driverIcNo && ord.AtdcompleteDate == atdCompleteDate
                            select new JobViewModelGroup
                            {
                                DriverIcNo = dr.DriverIcno,
                                JobNo = jb.JobNo,
                                ATD_Date = ord.AtdcompleteDate,
                                TruckId = jb.TruckId,
                                TruckType = tr.TruckType,
                                DeliveryCustCode = ord.DeliveryCustCode,
                                ServiceLevel = dc.ServiceLevel
                            };

            var res = from r in groupList
                      group r by new { r.DriverIcNo, r.JobNo, r.TruckId, r.TruckType, r.ATD_Date, r.ServiceLevel } into rGroup
                      select new JobModelView
                      {
                          JobNo = rGroup.Key.JobNo,
                          TruckId = rGroup.Key.TruckId,
                          TruckType = rGroup.Key.TruckType,
                          ATD_Date = rGroup.Key.ATD_Date,
                          ServiceLevel = rGroup.Key.ServiceLevel,
                          NumberOfDropPoint = rGroup.Select(p => p.DeliveryCustCode).Distinct().Count(),
                          NumberOfTrips = CalculateTrip(rGroup.Select(p => p.DeliveryCustCode).Distinct().Count(), rules),
                          Money = 0
                      };

            List<JobModelView> results = res.ToList();
            List<DesLocateException> desLocateExceptions = _ctx.DesLocateException.ToList();
            for (int i = 0; i < results.Count(); i++)
            {
                bool isFar = checkJobAddressFar(groupList.Distinct().ToList(), desLocateExceptions, String.Empty, i);
                results[i].Money = CalculateMoney(res.ToList()[i].NumberOfTrips, money, rules, moneyByBigCars, typeJobs, truckWeightTypes, truckSizes, i + 1, isFar, results[i].TruckType, results[i].ServiceLevel); // 1: job đầu, 2: job sau
            }

            return results;
        }

        public bool checkJobAddressFar(List<JobViewModelGroup> groupList, List<DesLocateException> desLocateExceptions, string jobNo, int index)
        {
            string JNo = String.IsNullOrEmpty(jobNo) ? groupList[index].JobNo : jobNo;

            var jobAddress = from ord in _ctx.Orders
                             join dc in _ctx.DeliveryCustomers on ord.DeliveryCustCode equals dc.DeliveryCustCode 
                             where ord.JobNo == JNo
                             group ord by new { ord.JobNo, dc.DeliveryCustCode, dc.DeliveryAddress } into jbGroup
                             select new JobDeliveryCustomer
                             {
                                 JobNo = jbGroup.Key.JobNo,
                                 DeliveryCustCode = jbGroup.Key.DeliveryCustCode,
                                 DeliveryAddress = jbGroup.Key.DeliveryAddress
                             };

            foreach (var jb in jobAddress)
            {
                foreach (var des in desLocateExceptions)
                {
                    if (jb.DeliveryAddress.Contains(des.DeslocationName) || jb.DeliveryAddress.Contains(des.DeslocationFullName))
                        return true;
                }
            }

            return false;
        }

        public IActionResult ExportReportToExcel()
        {
            List<DataResult> dataReport = listReport;
            DateTime ReportFrom = listReport.FirstOrDefault().ATD_Date;
            DateTime ReportTo = listReport.LastOrDefault().ATD_Date;
            //xuất ra excel dùng eplus
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("DynamicReport");
                worksheet.Cells[3, 1].Value = "Báo cáo từ ngày " + ReportFrom.ToString("dd/MM/yyyy") + " đến ngày " + ReportTo.ToString("dd/MM/yyyy");

                //custome size
                worksheet.Row(4).Height = 20;
                worksheet.Column(1).Width = 10;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 20;
                for (int i = 6; i <= 24; i++)
                {
                    worksheet.Column(i).Width = 30;
                    worksheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                  
                }

                //custom text
                for (int i = 1; i < 24; i++)
                {
                    if(i!=2 || i!=3 || i!=5)
                    worksheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                

                //custom color
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#108f14");
                for (int i = 1; i <= 24; i++)
                {
                    worksheet.Cells[4, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[4, i].Style.Fill.BackgroundColor.SetColor(colFromHex);
                }

                //custom format
                worksheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Row(4).Style.Font.Bold = true;

                worksheet.Cells[4, 1].Value = "STT";
                worksheet.Cells[4, 2].Value = "Số điện thoại";
                worksheet.Cells[4, 3].Value = "Tên tài xế";
                worksheet.Cells[4, 4].Value = "Đơn vị vận chuyển";
                worksheet.Cells[4, 5].Value = "Ngày";
                worksheet.Cells[4, 6].Value = "Tổng số chuyến";
                worksheet.Cells[4, 7].Value = "Tổng số điểm giao";
                for (int i = 0; i < 15; i++)
                {
                    worksheet.Cells[4, i + 8].Value = "Số điểm giao Job #"+(i+1).ToString();
                }
                worksheet.Cells[4, 23].Value = "Hệ số tài";
                worksheet.Cells[4, 24].Value = "Tổng tiền";

                //body of table  
                //  
                int recordindex = 5;
                int idx = 1;
                foreach (var data in dataReport)
                {
                    worksheet.Cells[recordindex, 1].Value = idx;
                    worksheet.Cells[recordindex, 2].Value = data.DriverIcNo;
                    worksheet.Cells[recordindex, 3].Value = data.DriverName;
                    worksheet.Cells[recordindex, 4].Value = data.TransportAgent;
                    worksheet.Cells[recordindex, 5].Value = data.ATD_Date.ToString("dd/MM/yyyy");
                    worksheet.Cells[recordindex, 6].Value = data.TotalJobs;
                    worksheet.Cells[recordindex, 7].Value = data.TotalDropPoint;
                    for (int i = 0; i < data.descriptionPerJobs.Count(); i++)
                    {
                        worksheet.Cells[recordindex, i + 8].Value = data.descriptionPerJobs[i].DropPointPerJob;
                    }
                    worksheet.Cells[recordindex, 23].Value = data.descriptionPerJobs.Select(p=>p.HeSoTai).Sum();
                    worksheet.Cells[recordindex, 24].Value = data.TotalMoney.ToString("##,#");
                    recordindex++;
                    idx++;
                }

                package.Save();
            }
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DriversReport.xlsx");
        }
    }
}