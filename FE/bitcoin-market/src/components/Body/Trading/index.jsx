import React, { useState } from "react";
import "./trading.scss";
import axios from "axios";
import { urls } from "../../configs/urls";
import Offers from "./Offers";
import Big from "big.js";

const Trading = ({ isLoggedIn, rerenderOffers, hackyRerenderVariable }) => {
	const [isBuy, setIsBuy] = useState(true);
	const [isMarketOrder, setIsMarketOrder] = useState(true);
	const [orderValue, setOrderValue] = useState(new Big(0));
	const [limitValue, setLimitValue] = useState(new Big(0));
	const [validationError, setValidationError] = useState("");

	const confirmOrder = async () => {
		try {
			const bearer = localStorage.getItem("Bearer");
			await axios.post(
				urls.addOrder,
				isMarketOrder
					? { type: 0, isBuy, orderValue }
					: { type: 1, isBuy, orderValue, limitValue },
				{ headers: { Authorization: `Bearer ${bearer}` } }
			);

			rerenderOffers((hackyRerenderVariable += 1));
			// TODO make the fucking child components show the new transaction
		} catch (error) {
			if (error?.response?.data?.length) {
				setValidationError(error?.response?.data);
				setTimeout(() => {
					setValidationError("");
				}, 3000);
			}
		}
	};

	return (
		<div className="trading">
			{!isLoggedIn ? (
				<div className="trading-login-message">Log in to order</div>
			) : (
				<div className="trading-body">
					<div className="trading-header">
						<div className="trading-mode">
							<button
								onClick={() => setIsBuy(true)}
								className={
									isBuy
										? "trading-mode-button trading-mode-button-buy active"
										: "trading-mode-button trading-mode-button-buy"
								}
							>
								Buy
							</button>
							<button
								onClick={() => setIsBuy(false)}
								className={
									!isBuy
										? "trading-mode-button trading-mode-button-sell active"
										: "trading-mode-button trading-mode-button-sell"
								}
							>
								Sell
							</button>
						</div>
						<div className="trading-type">
							<button
								onClick={() => setIsMarketOrder(true)}
								className={
									isMarketOrder
										? "trading-type-button trading-type-button-market active"
										: "trading-type-button trading-type-button-market"
								}
							>
								Market
							</button>
							<button
								onClick={() => setIsMarketOrder(false)}
								className={
									!isMarketOrder
										? "trading-type-button  trading-type-button-limit active"
										: "trading-type-button trading-type-button-limit"
								}
							>
								Limit
							</button>
						</div>
					</div>
					<div className="trading-inputs">
						<div className="trading-inputs-order">
							{isBuy ? (
								isMarketOrder ? (
									<span>Amount USD to spend</span>
								) : (
									<span>Amount BTC to buy</span>
								)
							) : (
								<span>Amount BTC to sell</span>
							)}
							<input
								type="number"
								value={orderValue}
								onChange={(e) => {
									try {
										const bigNumber = new Big(e.target.value);
										console.log(e.target.value);
										console.log(bigNumber);
										setOrderValue(bigNumber);
									} catch (error) {
										setOrderValue(e.target.value);
									}
								}}
							/>
						</div>
						{!isMarketOrder && (
							<div className="trading-inputs-limit">
								<span>USD price limit</span>
								<input
									type="number"
									value={limitValue}
									onChange={(e) => setLimitValue(new Big(e.target.value))}
								/>
							</div>
						)}
					</div>
					<div className="trading-confirm-container">
						<span className="validation-error">{validationError}</span>
						<button
							onClick={confirmOrder}
							className={
								isBuy
									? "trading-confirm trading-confirm--buy"
									: "trading-confirm trading-confirm--sell"
							}
						>
							Place {isBuy ? "Buy" : "Sell"}{" "}
							{isMarketOrder ? "Market" : "Limit"} order
						</button>
					</div>
				</div>
			)}
		</div>
	);
};

export default Trading;
