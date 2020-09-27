calculateNetProfit = function (buyPrice, shares, sellPrice) {
    try {

        var commissionRate = 0.25 / 100;
        var buyValue = buyPrice * shares;
        var buyTotalFees = computeBuyFees(buyPrice, shares, commissionRate);
        var buyTotalAmount = buyValue + buyTotalFees;

        var sellValue = sellPrice * shares;
        var sellTotalFees = computeSellFees(sellPrice, shares, commissionRate);
        var sellTotalAmount = sellValue - sellTotalFees;
        var netProfit = 0;
        var netProfitPercentage = 0;

        if (sellPrice > 0) {
            if (buyTotalAmount != 0) {
                netProfit = sellTotalAmount - buyTotalAmount;
                netProfitPercentage = computeChangePercentage(sellTotalAmount, buyTotalAmount);
            }
        }

        return {
            netProfit : netProfit.toFixed(2) ,
            netProfitPercentage: netProfitPercentage.toFixed(2),
            buyTotalAmount: buyTotalAmount.toFixed(2),
            sellTotalAmount: sellTotalAmount.toFixed(2)
        };

    } catch (ex) {
        return {};
    }
}


getPriceDecimalPlace = function (price) {
    var round2 = +price.toFixed(2);
    if (price != round2) {
        return 4;
    } else {
        return 2;
    }
}

computeBuyFees = function (price, shares, brokerCommissionRate) {
    if (price) {
        var stockWorth = computeStockWorth(price, shares);
        return computeBasicFees(stockWorth, brokerCommissionRate);
    } else {
        return 0;
    }
}

computeSellFees = function (price, shares, brokerCommissionRate, year) {
    var stockWorth = computeStockWorth(price, shares);
    var salesTax = computeSalesTax(stockWorth, null, year);
    return computeBasicFees(stockWorth, brokerCommissionRate) + salesTax;
}

computeSalesTax = function (stockWorth, salesTax, year) {
    if (!salesTax) salesTax = 0.0060;
    if (year) {
        var orderDate = new Date(year, 1, 1);
        var limitDate = new Date(2018, 1, 1);
        if (orderDate < limitDate)
            salesTax = 0.0050;
    }
    return stockWorth * salesTax;
}

computeStockWorth = function (price, shares) {
    return price * shares;
}

computeBasicFees = function (stockWorth, brokerCommissionRate) {
    var brokerCommission = computeBrokerCommission(stockWorth, brokerCommissionRate);
    var vat = computeVAT(brokerCommission);
    var fees = computePSEFee(stockWorth);
    return brokerCommission + vat + fees;
}

computeBrokerCommission = function (stockWorth, brokerCommission) {
    if (!brokerCommission && brokerCommission != 0) brokerCommission = 0.0025;
    var result = stockWorth * brokerCommission;
    if (result < 20 && brokerCommission != 0) {
        result = 20;
    }
    return result;
}

computeVAT = function (brokerCommission, vatAmount) {
    if (!vatAmount) vatAmount = 0.12;
    return brokerCommission * vatAmount;
}

computePSEFee = function (stockWorth, pseFee) {
    if (!pseFee) pseFee = 0.00015;
    return stockWorth * pseFee;
}

computePercentageSellPrice = function (breakEvenPrice, percentage) {
    var percentageMultiplier = 1.00 + percentage;
    return breakEvenPrice * percentageMultiplier;
}

computeChangePercentage = function (currentPrice, closePrice) {
    if (closePrice == 0) {
        return 0;
    }
    var change = computeChange(currentPrice, closePrice);
    return (change / closePrice) * 100;
}

computeBreakEvenSellPrice = function (shares, totalCost, decimalPlaces, brokerCommissionRate) {
    if (totalCost) {
        var exactBreakEvenSellPrice = computeExactBreakEvenSellPrice(shares, totalCost, brokerCommissionRate);
        var multiplier = 10;
        var balancer = 0;
        if (!decimalPlaces) decimalPlaces = 2;
        for (var i = 1; i < decimalPlaces; i++) {
            multiplier *= 10;
        }
        if (totalCost < 8000) {
            if (decimalPlaces == 2) {
                balancer = 0.02;
            } else {
                balancer = 0.0002;
            }
        }
        return (Math.ceil(exactBreakEvenSellPrice * multiplier) / multiplier) + balancer;
    } else {
        return 0;
    }
}

computeExactBreakEvenSellPrice = function (shares, totalCost, brokerCommissionRate) {
    if (!brokerCommissionRate) brokerCommissionRate = 0.0025;
    if (!shares || shares <= 0) {
        return 0;
    }
    return totalCost / (0.99485 - (1.12 * brokerCommissionRate)) / shares;
}

computeChange = function(currentPrice, closePrice) {
    return currentPrice - closePrice;
}