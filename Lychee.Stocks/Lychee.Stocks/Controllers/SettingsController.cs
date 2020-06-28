using System.Net;
using System.Web.Mvc;
using Lychee.Domain.Interfaces;

namespace Lychee.Stocks.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public ActionResult Index()
        {
            var settings = _settingService.GetAllSettings();
            return View(settings);
        }

        [HttpPost]
        public HttpStatusCodeResult UpdateSetting(int settingId, string value)
        {
            _settingService.UpdateValue(settingId, value);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}