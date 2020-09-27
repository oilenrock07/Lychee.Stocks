$(function () {

    function handleSharesChange(e) {
        computeNetProfit(this, 'Cutloss');
        computeNetProfit(this, 'Target');
    }

    function computeNetProfit(element, sellInputName) {
        var tr = $(element).closest('tr');

        var shares = getTextValueFromRow(tr, 'Shares');
        var entry = getTextValueFromRow(tr, 'Entry');
        var sell = getTextValueFromRow(tr, sellInputName);

        if (shares === 0 || entry === 0 || sell === 0)
            return;

        var profit = calculateNetProfit(entry, shares, sell);
        var netProfit = profit.netProfit + ' (' + numberWithCommas(profit.netProfitPercentage) + '%)';
        getSpanFromRow(tr, '.js-totalAmount').html(numberWithCommas(profit.buyTotalAmount));

        var totalAmountSelector = '.js-t-totalAmount';
        var netProfitSelector = '.js-t-netprofit';

        if (sellInputName === 'Cutloss') {
            totalAmountSelector = '.js-cl-totalAmount';
            netProfitSelector = '.js-cl-netprofit';
        }

        getSpanFromRow(tr, totalAmountSelector).html(numberWithCommas(profit.sellTotalAmount));
        getSpanFromRow(tr, netProfitSelector).html(numberWithCommas(netProfit));
    }

    function handleCutlossChange() {
        computeNetProfit(this, 'Cutloss');
    }

    function handleTargetChage() {
        computeNetProfit(this, 'Target');
    }

    function getTextValueFromRow(tr, name) {
        var input = getInputFromRow(tr, name);
        return parseFloat($(input).val());
    }

    function getInputFromRow(tr, name) {
        return tr.find("input[name='" + name + "']");
    }

    function getSpanFromRow(tr, selector) {
        return tr.find(selector);
    }

    function handleChkCalculatorClick() {
        if ($(this).is(':checked')) {
            $('.calculator-mode').css('display', 'block');
        } else {
            $('.calculator-mode').css('display', 'none');
        }
    }

    function init() {
        $('body').on('change', "[name='Shares']", handleSharesChange);
        $('body').on('change', "[name='Entry']", handleSharesChange);

        $('body').on('change', "[name='Cutloss']", handleCutlossChange);
        $('body').on('change', "[name='Target']", handleTargetChage);
        $('body').on('click', "#chkCalculator", handleChkCalculatorClick);

        $.each($('tbody > tr'), (index, element) => {
            computeNetProfit($(element), 'Target');
            computeNetProfit($(element), 'Cutloss');
        });
    }

    init();
});