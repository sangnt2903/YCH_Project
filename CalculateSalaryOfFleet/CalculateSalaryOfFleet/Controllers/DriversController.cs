using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CalculateSalaryOfFleet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.Style;

namespace CalculateSalaryOfFleet.Controllers
{
    public class DriversController : CheckAuthenticateController
    {
        private readonly FleetsTripsContext _ctx;
        public DriversController(FleetsTripsContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            return View(_ctx.Drivers.Where(p=>p.DriverIcno != String.Empty).ToList());
        }

        public double CalculateTrip(int numberOfOrders,List<Rules> rules)
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

        public IActionResult GetAllJobs(string driverICNo)
        {
            List<Rules> rules = _ctx.Rules.OrderBy(p => p.RuleNumber).ToList();
            var results = from ord in _ctx.Orders
                          join jb in _ctx.Jobs on ord.JobNo equals jb.JobNo
                          where jb.DriverIcno == driverICNo
                          group ord by new {ord.JobNo, ord.AtdcompleteDate} into ordJobNo
                          select new JobModelView
                          {
                              JobNo = ordJobNo.Key.JobNo,
                              ATD_Date = ordJobNo.Key.AtdcompleteDate,
                              NumberOfDropPoint = ordJobNo.Select(p => p.DeliveryCustCode).Distinct().Count(),
                              NumberOfTrips = CalculateTrip(ordJobNo.Select(p => p.DeliveryCustCode).Distinct().Count(), rules),
                          };

            Drivers driver = _ctx.Drivers.SingleOrDefault(p => p.DriverIcno == driverICNo);
            string driverString = driverICNo + "-" + driver.DriverName;
            ViewBag.driver = driverString;
            return View(results);
        }

        public double CalculateTripInTotalJobs(string driverIcNo)
        {
            double totalTrip = 0.0;
            List<Rules> rules = _ctx.Rules.OrderBy(p => p.RuleNumber).ToList();
            var results = from ord in _ctx.Orders
                          join jb in _ctx.Jobs on ord.JobNo equals jb.JobNo
                          where jb.DriverIcno == driverIcNo
                          group ord by ord.JobNo into ordJobNo
                          select new JobModelView
                          {
                              JobNo = ordJobNo.Key,
                              NumberOfDropPoint = ordJobNo.Select(p => p.DeliveryCustCode).Distinct().Count(),
                              NumberOfTrips = CalculateTrip(ordJobNo.Select(p => p.DeliveryCustCode).Distinct().Count(), rules),
                          };
            totalTrip = results.Select(p => p.NumberOfTrips).Sum();
            return totalTrip;
        } 
    }
}   