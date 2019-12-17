
Highcharts.chart('chartTop', {
    chart: {
        type: 'column'
    },
    title: {
        text: 'Column chart with negative values'
    },
    xAxis: {
        categories: ['Apples', 'Oranges', 'Pears', 'Grapes', 'Bananas']
    },
    credits: {
        enabled: false
    },
    series: [{
        name: 'John',
        data: [15, 13, 14, 17, 12]
    }, {
        name: 'Jane',
        data: [12, -12, -13, 12, 11]
    }, {
        name: 'Joe',
        data: [13, 14, 14, -12, 15]
    }]
});