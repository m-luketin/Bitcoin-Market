import Big from "big.js";
import React from "react";
import "./singleOrder.scss";

const SingleOrder = ({
	order,
	isSale,
	isBuy,
	sum,
	sumPercentage,
	cancelButtonCallback,
	showTypeOfTransaction,
	showBothValues,
}) => {
	const parseDateTimeString = (dateTimeString, onlyTime = false) => {
		const splitDateTime = dateTimeString.split("T");
		let dateString = splitDateTime[0];
		let timeString = "";
		if (dateTimeString.split("T").length > 1) timeString = splitDateTime[1];

		dateString = dateString.replaceAll("-", "/");
		if (timeString.split(":").length > 1)
			timeString = timeString.split(":")[0] + ":" + timeString.split(":")[1];

		return { dateString, timeString };
	};

	const transactionTime = parseDateTimeString(
		isSale || isBuy || !order.transactionFinished
			? order.transactionStarted
			: order.transactionFinished
	).timeString;
	const transactionDate = parseDateTimeString(
		isSale || isBuy || !order.transactionFinished
			? order.transactionStarted
			: order.transactionFinished
	).dateString;

	let classList = order.isCancelled
		? "single-order single-order--cancelled"
		: "single-order";
	return (
		<>
			{order && (
				<div className={classList}>
					{!isSale && !isBuy ? (
						<>
							<span className="single-order-item single-order-btc-value">
								{Big(order?.valueInBtc).toFixed(8)}
							</span>
							<span className="single-order-item single-order-usd-value">
								{Big(order?.valueInUsd).toFixed(2)}
							</span>
							<span className="single-order-item single-order-time">
								{transactionTime}
							</span>
							<span className="single-order-item single-order-btc-value">
								{transactionDate}
							</span>
						</>
					) : (
						<>
							{(showBothValues || isBuy) && (
								<>
									<span className="single-order-item single-order-usd-value">
										{isBuy
											? Big(order?.valueInBtc)
													.minus(order?.filledValue)
													.div(order?.valueInBtc ? order.valueInBtc : 1)
													.mul(order?.valueInUsd)
													.toFixed(2)
											: Big(order.valueInUsd)
													.minus(order?.filledValue)
													.toFixed(2)}
									</span>{" "}
								</>
							)}
							{(showBothValues || isSale) && (
								<>
									<span className="single-order-item single-order-btc-value">
										{isSale
											? Big(order?.valueInBtc)
													.minus(order?.filledValue)
													.toFixed(8)
											: Big(order?.valueInUsd)
													.minus(order?.filledValue)
													.div(order?.valueInUsd ? order.valueInUsd : 1)
													.mul(order?.valueInBtc)
													.toFixed(2)}
									</span>{" "}
								</>
							)}
							{sum && (
								<>
									<span className="single-order-item single-order-sum">
										{sum?.toFixed(8)}
									</span>
								</>
							)}

							<span className="single-order-item single-order-time-value">
								{transactionTime}
							</span>
							<span className="single-order-item single-order-date-value">
								{transactionDate}
							</span>
							{showTypeOfTransaction && (
								<span className="single-order-item single-order-type-value">
									{isSale ? "Sale" : isBuy ? "Buy" : ""}
									{order.isCancelled && (
										<span className="single-cancelled-order">Cancelled</span>
									)}
								</span>
							)}
							{cancelButtonCallback && (
								<>
									<span
										className="single-order-item-cancel"
										onClick={cancelButtonCallback}
									>
										x
									</span>
								</>
							)}
							<div
								className={
									isBuy
										? "single-order-item-overlay--buy"
										: "single-order-item-overlay"
								}
								style={{ width: `${sumPercentage}%` }}
							></div>
						</>
					)}
				</div>
			)}
		</>
	);
};

export default SingleOrder;
