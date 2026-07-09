function initBarChart(chartId, url, label, legendText, spinnerNumber) {
    let chart = new Chart($(chartId), {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: label,
                data: [],
                backgroundColor: "rgba(76, 175, 80, 0.5)",
                borderColor: "#6da252",
                borderWidth: 1,
            }]
        },
        options: {
            animation: {
                duration: 2000,
                easing: 'easeOutQuart',
            },
            plugins: {
                legend: {
                    display: false,
                    position: 'top',
                },
                title: {
                    display: true,
                    text: legendText,
                    position: 'left',
                },
            },
        }
    });

    chart.ajax = {
        reload: function () {
            showLoader(spinnerNumber);
            chart.data.labels = [];
            chart.data.datasets[0].data = [];
            chart.update();

            $.ajax({
                url: url,
                type: 'GET',
                success: function (response) {
                    chart.data.labels = response.labels;
                    chart.data.datasets[0].data = response.datasets;

                    chart.update();
                },
                error: function (xhr) {
                    hideLoader(spinnerNumber);
                    handleErrorApiResponse(xhr);
                },
                complete: function () {
                    hideLoader(spinnerNumber);
                }
            });
        }
    };

    return chart;
}

function initLineChart(chartId, url, label, legendText, spinnerNumber) {
    let chart = new Chart($(chartId), {
        type: 'line',
        data: {
            labels: [],
            datasets: [{
                label: label,
                data: [],
                backgroundColor: "rgba(48, 164, 255, 0.2)",
                borderColor: "rgba(48, 164, 255, 0.8)",
                fill: true,
                borderWidth: 1
            }]
        },
        options: {
            animation: {
                duration: 2000,
                easing: 'easeOutQuart',
            },
            plugins: {
                legend: {
                    display: false,
                    position: 'right',
                },
                title: {
                    display: true,
                    text: legendText,
                    position: 'left',
                },
            },
        }
    });

    chart.ajax = {
        reload: function () {
            showLoader(spinnerNumber);
            chart.data.labels = [];
            chart.data.datasets[0].data = [];
            chart.update();

            $.ajax({
                url: url,
                type: 'GET',
                success: function (response) {
                    chart.data.labels = response.labels;
                    chart.data.datasets[0].data = response.datasets;

                    chart.update();
                },
                error: function (xhr) {
                    hideLoader(spinnerNumber);
                    handleErrorApiResponse(xhr);
                },
                complete: function () {
                    hideLoader(spinnerNumber);
                }
            });
        }
    };

    return chart;
}
