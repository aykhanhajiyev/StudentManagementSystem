
// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

//Get data with AJAX();
var month = new Array();
month[0] = "Yan";
month[1] = "Fev";
month[2] = "Mart";
month[3] = "Apr";
month[4] = "May";
month[5] = "iyun";
month[6] = "iyul";
month[7] = "Avq";
month[8] = "Sen";
month[9] = "Okt";
month[10] = "Noy";
month[11] = "Dek";
$(document).ready(function () {
    var mylabeldata = new Array();
    var myfaiz = new Array();
    $.ajax({
        url: '/home/getexamresults',
        type: 'GET',
        datatype: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var date = new Date(parseFloat(data[i].Tarix.match(/\d+/g)));
                var dateString = date.getDate() + "-" + (month[date.getMonth()]) + "-" + date.getFullYear();
                mylabeldata.push(dateString);
                myfaiz.push(data[i].Faiz);
            }

            var ctx = document.getElementById("myAreaChart");
            var myLineChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: mylabeldata,
                    datasets: [{
                        label: "Faiz",
                        lineTension: 0.3,
                        backgroundColor: "rgba(2,117,216,0.2)",
                        borderColor: "rgba(2,117,216,1)",
                        pointRadius: 5,
                        pointBackgroundColor: "rgba(2,117,216,1)",
                        pointBorderColor: "rgba(255,255,255,0.8)",
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(2,117,216,1)",
                        pointHitRadius: 50,
                        pointBorderWidth: 2,
                        data: myfaiz,
                    }],
                },
                options: {
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'date'
                            },
                            gridLines: {
                                display: false
                            },
                            ticks: {
                                maxTicksLimit: 7
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: 100,
                                maxTicksLimit: 20
                            },
                            gridLines: {
                                color: "rgba(0, 0, 0, .125)",
                            }
                        }],
                    },
                    legend: {
                        display: false
                    }
                }
            });
        },
        error: function (data) {
            console.log(data);
        }
    });
});

$(document).ready(function () {
    var mylabeldata = new Array();
    var myfaiz = new Array();
    $.ajax({
        url: '/home/getexamresults1',
        type: 'GET',
        datatype: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var date = new Date(parseFloat(data[i].Tarix.match(/\d+/g)));
                var dateString = date.getDate() + "-" + (month[date.getMonth()]) + "-" + date.getFullYear();
                mylabeldata.push(dateString);
                myfaiz.push(data[i].Faiz);
            }

            var ctx = document.getElementById("myAreaChart1");
            var myLineChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: mylabeldata,
                    datasets: [{
                        label: "Faiz",
                        lineTension: 0.3,
                        backgroundColor: "rgba(2,117,216,0.2)",
                        borderColor: "rgba(2,117,216,1)",
                        pointRadius: 5,
                        pointBackgroundColor: "rgba(2,117,216,1)",
                        pointBorderColor: "rgba(255,255,255,0.8)",
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(2,117,216,1)",
                        pointHitRadius: 50,
                        pointBorderWidth: 2,
                        data: myfaiz,
                    }],
                },
                options: {
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'date'
                            },
                            gridLines: {
                                display: false
                            },
                            ticks: {
                                maxTicksLimit: 7
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: 100,
                                maxTicksLimit: 20
                            },
                            gridLines: {
                                color: "rgba(0, 0, 0, .125)",
                            }
                        }],
                    },
                    legend: {
                        display: false
                    }
                }
            });
        },
        error: function (data) {
            console.log(data);
        }
    });
});

