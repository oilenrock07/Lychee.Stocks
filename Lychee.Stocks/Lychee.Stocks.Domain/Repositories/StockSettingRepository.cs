using System.Collections.Generic;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Repositories;

namespace Lychee.Stocks.Domain.Repositories
{
    public class StockSettingRepository : SettingRepository
    {
        public override ICollection<Setting> GetAllSettings()
        {
            _settings = new List<Setting>
            {
                new Setting { Key = "SmartScrapper.Chromium.DownloadPath", Value = @"C:\Cawi\DEV\Lychee\Lychee.Scrapper\CustomChromium"}
            };

            return _settings;
        }
    }
}
