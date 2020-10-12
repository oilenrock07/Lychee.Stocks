
var color = Chart.helpers.color;
var red = color(window.chartColors.red).alpha(0.5).rgbString();
var green = color(window.chartColors.green).alpha(0.5).rgbString();
var grey = color(window.chartColors.grey).alpha(0.5).rgbString();

var redBorder = window.chartColors.red;
var greenBorder = window.chartColors.green;
var greyBorder = window.chartColors.grey;

function renderTop10TradesChart(result) {

    var horizontalBarChartData = {
        labels: result.map(x => x.StockCode),
        datasets: [{
            backgroundColor: result.map(x => getBarColor(x.Last, x.Open)),
            borderColor: result.map(x => getBarBorderColor(x.Last, x.Open)),
            borderWidth: 1,
            data: result.map(x => x.Trades)
        }]
    };


    var ctx = document.getElementById('Top10TradesCanvas').getContext('2d');
    
    renderHorizontalChart(ctx, horizontalBarChartData);
}

function renderTop10VolumesChart(result) {
    var ctx  = document.getElementById('Top10VolumesCanvas').getContext('2d');

    var horizontalBarChartData = {
        labels: result.map(x => x.StockCode),
        datasets: [{
            backgroundColor: result.map(x => getBarColor(x.Last, x.Open)),
            borderColor: result.map(x => getBarBorderColor(x.Last, x.Open)),
            borderWidth: 1,
            data: result.map(x => x.Volume)
        }]
    };

    renderHorizontalChart(ctx, horizontalBarChartData);
}

function renderHorizontalChart(ctx, horizontalBarChartData) {
    var chart = new Chart(ctx, {
        type: 'horizontalBar',
        data: horizontalBarChartData,
        options: {
            elements: {
                rectangle: {
                    borderWidth: 2,
                }
            },
            responsive: true,
            legend: {
                display: false,
            },
            title: {
                display: false,
            }
        }
    });
}

function getBarColor(last, open) {

    last = parseFloat(last);
    open = parseFloat(open);
    if (last > open) {
        return green;
    }
    else if (last < open) {
        return red;
    } else {
        return grey;
    }
}

function getBarBorderColor(last, open) {

    last = parseFloat(last);
    open = parseFloat(open);
    if (last > open) {
        return greenBorder;
    }
    else if (last < open) {
        return redBorder;
    } else {
        return greyBorder;
    }
}

$.post('Home/Top10Trades').done((result) => {
    renderTop10TradesChart(result);
});


$.post('Home/Top10Volumes').done((result) => {
    renderTop10VolumesChart(result);
});