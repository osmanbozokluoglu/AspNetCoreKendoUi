using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreKendoUi.Models;
using Newtonsoft.Json;
using AspNetCoreKendoUi.Utils;
using KendoUI.Core;

namespace AspNetCoreKendoUi.Controllers
{
    public class HomeController : Controller
    {
        private static List<City> cityList;

        public IActionResult Index()
        {
            cityList = new List<City>() {
                new City(){ Name = "Afyon" , PlateNo = 3 },
                new City(){ Name = "Ankara" , PlateNo = 6 }
            };

            ViewBag.CityList = JsonConvert.SerializeObject(cityList);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult CitySearch(int cityNo)
        {
            return Json(cityList.Where(x => x.PlateNo == cityNo));
        }

        public JsonResult CityList()
        {
            return Json(cityList);
        }

        public DataSourceResult GridCityList()
        {
            var dataString = this.HttpContext.GetJsonDataFromQueryString();
            var request = JsonConvert.DeserializeObject<DataSourceRequest>(dataString);
            return cityList.AsQueryable().ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        public DataSourceResult GridCityList2([KendoRequest]DataSourceRequest dataSourceRequest)
        {
            return cityList.AsQueryable().ToDataSourceResult(HttpContext.GetKendoDataSourceResult());
        }
    }
}
