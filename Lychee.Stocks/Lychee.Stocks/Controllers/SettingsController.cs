using System.Web.Mvc;
using Lychee.Domain.Interfaces;

namespace Lychee.Stocks.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingRepository _settingRepository;

        public SettingsController(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public ActionResult Index()
        {
            var settings = _settingRepository.GetAllSettings();
            return View(settings);
        }
    }
}