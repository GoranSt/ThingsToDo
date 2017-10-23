debugger;

$(function () {

    getStatistics();
    appendBreadcrumb();

});

function appendBreadcrumb() {
    var html = '<li><i class="fa fa-angle-right"></i><span>' + resourceStatistics + '</span></li>';
    $('.page-breadcrumb').append(html)

}
 
function getStatistics() {

    Metronic.blockUI();
    $.get('/Statistics/GetStatistics', function (data) {
        debugger;
        if (data.success) {
            debugger;

       var bindToId = '#pieChartWeekly';
            pieChart(bindToId, data.statistics.WeeklyFinishedTasks, data.statistics.WeeklyRemovedTasks, data.statistics.WeeklyRemainingTasks);
            bindToId = '#timeSeriesWeeklyChart';
            console.log(data.statistics.TimeSeriesChartListFinishedTasks);
            timeSeriesChart(bindToId, data.statistics.TimeSeriesChartListFinishedTasks, data.statistics.TimeSeriesChartListRemovedTasks, data.statistics.TimeSeriesChartListRemainingTasks);

            bindToId = '#pieChartMonthly';
            pieChart(bindToId, data.statistics.MonthlyFinishedTasks, data.statistics.MonthlyRemovedTasks, data.statistics.MonthlyRemainingTasks);

            bindToId = '#pieChartQuarterly'; 
            pieChart(bindToId, data.statistics.QuarterFinishedTasks, data.statistics.QuarterRemovedTasks, data.statistics.QuarterRemainingTasks);

            Metronic.unblockUI();

        }
        else {
            showMessageDialog(data.message);
            Metronic.unblockUI();
        }
    }).error(function (jqXHR) {
        showMessageDialog(jqXHR.responseText);
        Metronic.unblockUI();
    });
}

function pieChart(bindToId, finished, removed, remaining) {

    var chart = c3.generate({
        bindto: bindToId,
        data: {
            // iris data from R
            columns: [
                ['Finished', finished],
                ['Removed', removed],
                 ['Remaining', remaining]
            ],
            type: 'pie',
            onclick: function (d, i) { console.log("onclick", d, i); },
            onmouseover: function (d, i) { console.log("onmouseover", d, i); },
            onmouseout: function (d, i) { console.log("onmouseout", d, i); }
        }
    });
};

function timeSeriesChart(bindToId, tasksFinished, tasksRemoved, tasksRemaining) {

    var date = new Date();
    console.log(date.getUTCDay());
    console.log(date.getUTCDay()-5);
    var counter = -6;
    var chart = c3.generate({
        bindto: bindToId,
        data: {
            x: 'x',
            //        xFormat: '%Y%m%d', // 'xFormat' can be used as custom format of 'x'
            columns: [
                //['x', d.getDate().toString(), d.getDate() - 1, d.getDate() - 2, d.getDate() - 3, d.getDate() - 4, d.getDate() -5],
    //            ['x', '20130101', '20130102', '20130103', '20130104', '20130105', '20130106'],
                ['x', moment().add(-6, 'days').format('dddd'), moment().add(-5, 'days').format('dddd'), moment().add(-4, 'days').format('dddd'), moment().add(-3, 'days').format('dddd'), moment().add(-2, 'days').format('dddd'), moment().add(-1, 'days').format('dddd'), moment().format('dddd')],
               
                 ['Finished Tasks', tasksFinished[6].TaskCount, tasksFinished[5].TaskCount, tasksFinished[4].TaskCount, tasksFinished[3].TaskCount, tasksFinished[2].TaskCount, tasksFinished[1].TaskCount, tasksFinished[0].TaskCount],
                ['Removed Tasks', tasksRemoved[6].TaskCount, tasksRemoved[5].TaskCount, tasksRemoved[4].TaskCount, tasksRemoved[3].TaskCount, tasksRemoved[2].TaskCount, tasksRemoved[1].TaskCount, tasksRemoved[0].TaskCount],
                ['Remaining Tasks', tasksRemaining[6].TaskCount, tasksRemaining[5].TaskCount, tasksRemaining[4].TaskCount, tasksRemaining[3].TaskCount, tasksRemaining[2].TaskCount, tasksRemaining[1].TaskCount, tasksRemaining[0].TaskCount]
            ]
        },
        axis: {
            x: {
                type: 'category',
                
            },
        },

      //  regions: [
      //{ axis: 'x', start: 1, end: 5, class: 'regionX' },
      //  ]
    });

//    var chart = c3.generate({
//        bindto: bindToId,
//    data: {
//        x: 'x',
//        //        xFormat: '%Y%m%d', // 'xFormat' can be used as custom format of 'x'
//        columns: [
//            ['x', '2017-8-20', '2013-01-02', '2013-01-03', '2013-01-04', '2013-01-05', '2013-01-06'],
////            ['x', '20130101', '20130102', '20130103', '20130104', '20130105', '20130106'],
//            ['Finished', 3, 4, 1, 2, 6, 7],
//            ['Removed', 1, 1, 3, 1, 9, 2],
//             ['Remaining', 1, 2, 3, 4, 5, 6]
//        ]
//    },
//    axis: {
//        x: {
//            type: 'timeseries',
//            tick: {
//                format: '%Y-%m-%d'
//            }
//        }
//    }
//});
};

// axies
var chart = c3.generate({
    bindto: '#chart',
    data: {
        columns: [
          ['Finished', 30, 200, 100, 400, 150, 250],
          ['Closed', 50, 20, 10, 40, 15, 25],
          ['Remaining', 50, 20, 10, 40, 15, 25]
        ],
        axes: {
            data2: 'y2' // ADD
        }
    },
    axis: {
        y2: {
            show: true // ADD
        }
    }
});



// pie chart




setTimeout(function () {
    chart.load({
        columns: [
            ["setosa", 0.2, 0.2, 0.2, 0.2, 0.2, 0.4, 0.3, 0.2, 0.2, 0.1, 0.2, 0.2, 0.1, 0.1, 0.2, 0.4, 0.4, 0.3, 0.3, 0.3, 0.2, 0.4, 0.2, 0.5, 0.2, 0.2, 0.4, 0.2, 0.2, 0.2, 0.2, 0.4, 0.1, 0.2, 0.2, 0.2, 0.2, 0.1, 0.2, 0.2, 0.3, 0.3, 0.2, 0.6, 0.4, 0.3, 0.2, 0.2, 0.2, 0.2],
            ["versicolor", 1.4, 1.5, 1.5, 1.3, 1.5, 1.3, 1.6, 1.0, 1.3, 1.4, 1.0, 1.5, 1.0, 1.4, 1.3, 1.4, 1.5, 1.0, 1.5, 1.1, 1.8, 1.3, 1.5, 1.2, 1.3, 1.4, 1.4, 1.7, 1.5, 1.0, 1.1, 1.0, 1.2, 1.6, 1.5, 1.6, 1.5, 1.3, 1.3, 1.3, 1.2, 1.4, 1.2, 1.0, 1.3, 1.2, 1.3, 1.3, 1.1, 1.3],
            ["virginica", 2.5, 1.9, 2.1, 1.8, 2.2, 2.1, 1.7, 1.8, 1.8, 2.5, 2.0, 1.9, 2.1, 2.0, 2.4, 2.3, 1.8, 2.2, 2.3, 1.5, 2.3, 2.0, 2.0, 1.8, 2.1, 1.8, 1.8, 1.8, 2.1, 1.6, 1.9, 2.0, 2.2, 1.5, 1.4, 2.3, 2.4, 1.8, 1.8, 2.1, 2.4, 2.3, 1.9, 2.3, 2.5, 2.3, 1.9, 2.0, 2.3, 1.8],
        ]
    });
}, 1500);

setTimeout(function () {
    chart.unload({
        ids: 'data1'
    });
    chart.unload({
        ids: 'data2'
    });
}, 2500);



// data color

var chart = c3.generate({
    bindto: '#dataColorChart',
    data: {
        columns: [
            ['data1', 30, 20, 50, 40, 60, 50],
            ['data2', 200, 130, 90, 240, 130, 220],
            ['data3', 300, 200, 160, 400, 250, 250]
        ],
        type: 'bar',
        colors: {
            data1: '#ff0000',
            data2: '#00ff00',
            data3: '#0000ff'
        },
        color: function (color, d) {
            // d will be 'id' when called for legends
            return d.id && d.id === 'data3' ? d3.rgb(color).darker(d.value / 150) : color;
        }
    }
});