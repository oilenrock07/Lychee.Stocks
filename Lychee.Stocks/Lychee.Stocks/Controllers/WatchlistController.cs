﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Entities;
using Lychee.Stocks.Models.Watchlist;

namespace Lychee.Stocks.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly IWatchListService _watchListService;

        public WatchlistController(IWatchListService watchListService)
        {
            _watchListService = watchListService;
        }

        public ActionResult Index(int id = 0)
        {
            var watchList = _watchListService.GetAllWatchList();
            if (id == 0 && watchList.Any())
                id = watchList.First().WatchListGroupId;

            var viewModel = new WatchlistViewModel
            {
                WatchListGroups = watchList,
                SelectedId = id
            };
            return View(viewModel);
        }

        public ActionResult Delete(int groupId, int watchListId)
        {
            _watchListService.DeleteWatchList(groupId, watchListId);
            return RedirectToAction("Index", new {id = groupId });
        }

        [HttpPost]
        public ActionResult AddWatchList(int groupId, string stockCode)
        {
            _watchListService.QuickAddWatchList(groupId, stockCode);
            return RedirectToAction("Index", new { id = groupId });
        }

        [HttpPost]
        public ActionResult UpdateWatchList(Watchlist model)
        {
            _watchListService.UpdateWatchList(model);
            return RedirectToAction("Index", new { id = model.WatchListGroupId});
        }
    }
}