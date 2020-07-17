using System.Net;
using System.Web.Mvc;
using Lychee.Domain.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Services;

namespace Lychee.Stocks.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IStockService _stockService;
        public SettingsController(ISettingService settingService, IStockService stockService)
        {
            _settingService = settingService;
            _stockService = stockService;
        }

        public ActionResult Index()
        {
            var settings = _settingService.GetAllSettings();
            return View(settings);
        }

        [HttpPost]
        public HttpStatusCodeResult UpdateSetting(string name, int settingId, string value)
        {
            _settingService.UpdateValue(settingId, value);

            CustomUpdate(name, value);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void CustomUpdate(string name, string value)
        {
            switch (name)
            {
                case SettingNames.InvestagramsCookieName:
                    _stockService.UpdateInvestagramsCookie(value);
                    break;
            }
        }
    }
}