using System.Collections.Generic;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Repositories;

namespace Lychee.Stocks.Domain.Repositories
{
    public class StocksScrappedSettingRepository : ScrappedSettingRepository
    {
        public override ICollection<ScrapeSetting> GetAllSettings()
        {
            if (Settings != null) return Settings;
            
            Settings = new List<ScrapeSetting>
            {
                new ScrapeSetting
                {
                    Category = "TableDataSetting"
                }
            };


            return Settings;
        }
    }
}
