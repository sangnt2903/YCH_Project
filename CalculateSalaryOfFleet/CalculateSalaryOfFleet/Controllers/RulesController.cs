using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculateSalaryOfFleet.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculateSalaryOfFleet.Controllers
{
    public class RulesController : CheckAuthenticateController
    {
        private readonly FleetsTripsContext _ctx;
        public RulesController(FleetsTripsContext ctx)
        {
            _ctx = ctx;
        }
        public IActionResult Index()
        {
            return View(_ctx.Rules.ToList());
        }

        public IActionResult CreateHST()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateHST(Rules r)
        {
            _ctx.Rules.Add(r);
            _ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_ctx.Rules.Find(id));
        }

        [HttpPost]
        public IActionResult Edit(Rules rules)
        {
            Rules ruleUpdate = _ctx.Rules.Find(rules.RuleId);
            ruleUpdate.RuleFrom = rules.RuleFrom;
            ruleUpdate.RuleTo = rules.RuleTo;
            ruleUpdate.RuleNumber = rules.RuleNumber;

            _ctx.Rules.Update(ruleUpdate);
            _ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult MoneyByPerformance()
        {
            return View(_ctx.MoneyJob.ToList());
        }

        public IActionResult EditMoneyByPerformance(int id)
        {
            return View(_ctx.MoneyJob.Find(id));
        }

        [HttpPost]
        public IActionResult EditMoneyByPerformance(MoneyJob moneyJob)
        {
            MoneyJob ruleUpdate = _ctx.MoneyJob.Find(moneyJob.Id);
            ruleUpdate.JobOrder = moneyJob.JobOrder;
            ruleUpdate.PerformenceMoney = moneyJob.PerformenceMoney;


            _ctx.MoneyJob.Update(ruleUpdate);
            _ctx.SaveChanges();
            return RedirectToAction("MoneyByPerformance");
        }
        public IActionResult ShipZone()
        {
            return View(_ctx.TypeJob.ToList());
        }

        public IActionResult EditShipZone(int id)
        {
            return View(_ctx.TypeJob.Find(id));
        }

        [HttpPost]
        public IActionResult EditShipZone(TypeJob typeJob)
        {
            TypeJob ruleUpdate = _ctx.TypeJob.Find(typeJob.TypeJobNo);
            ruleUpdate.TypeJob1 = typeJob.TypeJob1;
            ruleUpdate.TypeJobDescription = typeJob.TypeJobDescription;


            _ctx.TypeJob.Update(ruleUpdate);
            _ctx.SaveChanges();
            return RedirectToAction("ShipZone");
        }

        public IActionResult MoneyByBigCar()
        {
            return View(_ctx.MoneyByBigCar.ToList());
        }

        public IActionResult EditMoneyByBigCar(int id)
        {
            return View(_ctx.MoneyByBigCar.Find(id));
        }

        [HttpPost]
        public IActionResult EditMoneyByBigCar(MoneyByBigCar moneyByBigCar)
        {
            MoneyByBigCar ruleUpdate = _ctx.MoneyByBigCar.Find(moneyByBigCar.TripNo);
            ruleUpdate.MoneyOfServiceLevelHcmc = moneyByBigCar.MoneyOfServiceLevelHcmc;
            ruleUpdate.MoneyOfServiceLevelHcmp = moneyByBigCar.MoneyOfServiceLevelHcmp;


            _ctx.MoneyByBigCar.Update(ruleUpdate);
            _ctx.SaveChanges();
            return RedirectToAction("MoneyByBigCar");
        }
        public IActionResult DesLocateException()
        {
            return View(_ctx.DesLocateException.ToList());
        }

        public IActionResult EditDesLocateException(int id)
        {
            return View(_ctx.DesLocateException.Find(id));
        }

        [HttpPost]
        public IActionResult EditDesLocateException(DesLocateException desLocateException)
        {
            DesLocateException ruleUpdate = _ctx.DesLocateException.Find(desLocateException.DesLocationNo);
            ruleUpdate.DeslocationName = desLocateException.DeslocationName;
            ruleUpdate.DeslocationFullName = desLocateException.DeslocationFullName;


            _ctx.DesLocateException.Update(ruleUpdate);
            _ctx.SaveChanges();
            return RedirectToAction("DesLocateException");
        }
        public IActionResult TruckWeightType()
        {
            var listTruckSize = from ts in _ctx.TruckSize
                                join tst in _ctx.TruckSizeType on ts.TruckSizeId equals tst.TruckSizeId
                                select new TruckSizeTypeModelView
                                {
                                    TruckType = ts.TruckType,
                                    TruckSizeName = tst.TruckSizeName
                                };
            return View(listTruckSize.ToList());
        }

        public IActionResult EditTruckWeightType(string truckType)
        {
            ViewData["TruckSizeType"] = _ctx.TruckSizeType.ToList();
            return View(_ctx.TruckSize.Find(truckType));
        }

        [HttpPost]
        public IActionResult EditTruckWeightType(TruckSize truckSize)
        {
            TruckSize ruleUpdate = _ctx.TruckSize.Find(truckSize.TruckType);
            ruleUpdate.TruckSizeId = truckSize.TruckSizeId;
            _ctx.TruckSize.Update(ruleUpdate);
            _ctx.SaveChanges();
            return RedirectToAction("TruckWeightType");
        }

        public IActionResult CreateTruckSize()
        {
            ViewData["TruckSizeType"] = _ctx.TruckSizeType.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateTruckSize(TruckSize truckSize)
        {
            TruckSize truckSizeInsert = new TruckSize()
            {
                TruckType = truckSize.TruckType.ToUpper(),
                TruckSizeId = truckSize.TruckSizeId
            };
            _ctx.TruckSize.Add(truckSizeInsert);
            _ctx.SaveChanges();
            return RedirectToAction("TruckWeightType");
        }
    }
}