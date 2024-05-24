function updateCharts(sentimentChartData, ratingChartData) {

    const sentimentChartElement = document.getElementById("sentimentChart");
    const sentimentCanvas = sentimentChartElement.getContext("2d");

    destroyChart("sentimentChart");

    window.sentimentChart = new Chart(sentimentCanvas, {
        type: 'line',
        data: {
            labels: sentimentChartData.labels || [], 
            datasets: [{
                label: 'Average Sentiment Score',
                data: sentimentChartData.data || [], 
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                pointBackgroundColor: 'rgba(75, 192, 192, 1)',
                pointBorderColor: 'rgba(75, 192, 192, 1)',
                pointBorderWidth: 1,
                pointRadius: 5,
                tension: 0.1
            }]
        },
        options: {
            scales: {
                x: {
                    type: 'time',
                    time: {
                        parser: 'MMM dd, yyyy',
                        unit: 'day',
                        tooltipFormat: 'll',
                        displayFormats: {
                            day: 'MMM dd, yyyy'
                        }
                    }
                },
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    const ratingChartElement = document.getElementById("ratingChart");
    const ratingCanvas = ratingChartElement.getContext("2d");

    destroyChart("ratingChart");

    window.ratingChart = new Chart(ratingCanvas, {
        type: 'bar',
        data: {
            labels: ratingChartData && ratingChartData.labels ? ratingChartData.labels : [],
            datasets: [{
                label: 'Number of Reviews',
                data: ratingChartData && ratingChartData.data ? ratingChartData.data : [],
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    stepSize: 1
                }
            }
        }
    });
}


function destroyChart(chartId) {
    let chartInstance = Chart.getChart(chartId);
    if (chartInstance) {
        chartInstance.destroy();
    }
}

const searchForm = document.getElementById("searchForm");
searchForm.addEventListener("submit", function (event) {
    event.preventDefault(); 

    const formData = new FormData(searchForm); 

    fetch("/Business/Dashboard?handler=Search", {
        method: "POST",
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log("Data:", data); 

            if (!data || !data.sentimentChartData || !data.ratingChartData) {
                console.error("Invalid JSON data received from the server.");
                return;
            }

            updateCharts(data.sentimentChartData, data.ratingChartData);
        })
        .catch(error => {
            console.error("Error:", error); 
        });
});

updateCharts({ sentimentChartData: {}, ratingChartData: {} });

