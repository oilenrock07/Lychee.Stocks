using System.Linq;
using System.Web.Mvc;
using Lychee.Infrastructure.Interfaces;
using Lychee.Scrapper.Entities.Entities;

namespace Lychee.Stocks.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IRepository<Setting> _settingRepository;

        public SettingsController(IRepository<Setting> settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public ActionResult Index()
        {
            var settings = _settingRepository.GetAll().ToList();
            return View(settings);
        }
    }
}