using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculateSalaryOfFleet.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculateSalaryOfFleet.Controllers
{
    public class RulesController : Controller
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
            return View(_ctx.TruckWeightType.ToList());
        }

        public IActionResult EditTruckWeightType(int id)
        {
            return View(_ctx.TruckWeightType.Find(id));
        }

        [HttpPost]
        public IActionResult EditTruckWeightType(TruckWeightType truckWeightType)
        {
            TruckWeightType ruleUpdate = _ctx.TruckWeightType.Find(truckWeightType.TruckWeightTypeId);
            ruleUpdate.WeightFrom = truckWeightType.WeightTo;
            _ctx.TruckWeightType.Update(ruleUpdate);
            _ctx.SaveChanges();
            return RedirectToAction("TruckWeightType");
        }
    }
}