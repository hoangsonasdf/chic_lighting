const dataBarHorizontal = {
    type: 'bar',
    data: {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [
            {
                label: 'Traffic',
                data: [30, 15, 62, 65, 61, 65, 40],
            },
        ],
    },
};

const optionsBarHorizontal = {
    options: {
        indexAxis: 'y',
        scales: {
            x: {
                stacked: true,
                grid: {
                    display: true,
                    borderDash: [2],
                    zeroLineColor: 'rgba(0,0,0,0)',
                    zeroLineBorderDash: [2],
                    zeroLineBorderDashOffset: [2],
                },
                ticks: {
                    color: 'rgba(0,0,0, 0.5)',
                },
            },
            y: {
                stacked: true,
                grid: {
                    display: false,
                },
                ticks: {
                    color: 'rgba(0,0,0, 0.5)',
                },
            },
        },
    },
};

new mdb.Chart(document.getElementById('bar-chart-horizontal'), dataBarHorizontal, optionsBarHorizontal)