using System.Collections.Generic;
using System.Linq;
using Lychee.Stocks.Entities;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Lychee.Stocks.Models.News;
using Omu.ValueInjecter;

namespace Lychee.Stocks.Helpers
{
    public static class NewsHelper
    {
        public static List<NewsViewModel> GetNewsViewModels(List<News> news, List<WatchListGroup> watchList)
        {
            return news.Select(x =>
            {
                var viewModel = Mapper.Map<NewsViewModel>(x);
                viewModel.StockCode = x.StockInfoVM.StockCode;
                viewModel.IsInWatchList = watchList.Any(w => w.WatchLists.Any(wl => wl.StockCode == x.StockInfoVM.StockCode && !wl.Deleted));
                return viewModel;
            }).ToList();
        }
    }
}