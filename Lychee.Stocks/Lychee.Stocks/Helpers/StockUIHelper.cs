﻿namespace Lychee.Stocks.Helpers
{
    public static class StockUIClassHelper
    {
        private static string _even = "color-even";
        private static string _bull = "color-uptrend";
        private static string _bear = "color-downtrend";

        public static string StockColourClass(decimal value)
        {
            if (value < 0)
                return _bear;
            if (value > 0)
                return _bull;

            return _even;
        }

        public static string StockColourClass(decimal open, decimal last)
        {
            if (last < open)
                return _bear;
            if (last > open)
                return _bull;

            return _even;
        }
    }
}