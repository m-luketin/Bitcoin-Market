import React from "react";
import "./singleTrade.scss";

const SingleTrade = ({ trade }) => {
	const parseDateTimeString = (dateTimeString) => {
		const splitDateTime = dateTimeString.split("T");
		let dateString = splitDateTime[0];
		let timeString = splitDateTime[1];

		dateString = dateString.replaceAll("-", "/");
		timeString = timeString.split(":")[0] + ":" + timeString.split(":")[1];

		return { dateString, timeString };
	};

	const transactionTime = parseDateTimeString(
		trade.transactionFinished
	).timeString;
	const transactionDate = parseDateTimeString(
		trade.transactionFinished
	).dateString;

	return (
		<>
			{trade && (
				<div className="single-trade">
					<span className="single-trade-item single-trade-btc-value">
						{trade.valueInBtc}
					</span>
					<span className="single-trade-item single-trade-usd-value">
						{trade.valueInUsd}
					</span>
					<span className="single-trade-item single-trade-time">
						{transactionTime}
					</span>
					<span className="single-trade-item single-trade-btc-value">
						{transactionDate}
					</span>
				</div>
			)}
		</>
	);
};

export default SingleTrade;
