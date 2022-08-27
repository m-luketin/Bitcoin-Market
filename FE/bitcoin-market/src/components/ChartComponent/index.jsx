import React, { useCallback, useEffect, useState } from "react";
import AnyChart from "anychart-react";
import anychart from "anychart";
import "./chart.scss";
import axios from "axios";
import { urls } from "../configs/urls";

const ChartComponent = ({ hackyRerenderVariable }) => {
	const [chart, setChart] = useState();
	const [loaded, setLoaded] = useState(false);

	useEffect(() => {
		getData().then((data) => {
			const btcChart = anychart.candlestick();
			const series = btcChart.candlestick(data);

			series.risingStroke("#222");
			series.risingFill("green");
			series.fallingStroke("#222");
			series.fallingFill("red");

			btcChart.xScroller(true);
			btcChart.xScroller().fill("#ff990033");
			btcChart.xScroller().selectedFill("#ff9900");
			btcChart.xScroller().minHeight(25);

			btcChart.background().fill("#fefefe");
			btcChart.labels().fontColor("#222");
			btcChart.labels().fontColor("#222");
			btcChart.title().fontColor("#222");
			btcChart.yAxis().title("BTC price, $");

			btcChart.container("container");
			btcChart.draw();
			setChart(btcChart);
			setLoaded(true);
		});
	}, [hackyRerenderVariable]);

	const getData = useCallback(async () => {
		try {
			const response = await axios.get(urls.getChartData);
			const propertyValueArray = [];
			response.data.forEach((chartPoint) => {
				const chartPointDate = new Date(chartPoint.date);
				let propertyValues = [
					Date.UTC(
						chartPointDate.getFullYear(),
						chartPointDate.getMonth(),
						chartPointDate.getDate()
					),
					chartPoint.open,
					chartPoint.high,
					chartPoint.low,
					chartPoint.close,
				];
				propertyValueArray.push(propertyValues);
			});
			return anychart.data.set(propertyValueArray);
		} catch (error) {}
	});

	return (
		<div id="container" className="chart-container">
			{loaded && <AnyChart instance={chart} width="80vw" height="340px" />}
		</div>
	);
};

export default ChartComponent;
