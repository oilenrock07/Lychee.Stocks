
function renderChart(placeholder, label, data) {
    var ctx = document.getElementById(placeholder).getContext('2d');
    ctx.canvas.width = 1000;
    ctx.canvas.height = 500;


    var chart = new Chart(ctx, {
        type: 'candlestick',
        data: {
            datasets: [{
                label: label,
                data: getData(data)
            }]
        }
    });
}




function getData(data) {

    var ctr = 14;
    var arr = [];
    while (ctr >= 0) {
        arr.push({
            t: new Date(data.D[ctr]).valueOf(),
            o: data.O[ctr].toFixed(2),
            h: data.H[ctr].toFixed(2),
            l: data.L[ctr].toFixed(2),
            c: data.C[ctr].toFixed(2)
        });
        ctr--;
    }

    return arr;
}

