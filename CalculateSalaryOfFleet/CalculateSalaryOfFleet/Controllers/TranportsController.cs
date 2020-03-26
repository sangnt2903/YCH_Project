using System;
using System.Collections.Generic;
using System.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CalculateSalaryOfFleet.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CalculateSalaryOfFleet.Controllers
{
    public class TranportsController : Controller
    {
        private readonly FleetsTripsContext _ctx;
        public TranportsController(FleetsTripsContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}