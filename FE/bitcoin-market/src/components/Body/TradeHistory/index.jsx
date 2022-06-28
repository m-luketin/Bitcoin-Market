import React from "react";
import { useEffect, useState } from "react";
import "./tradeHistory.scss";

import axios from "axios";
import { urls } from "../../configs/urls";
import SingleTrade from "./SingleTrade";

const TradeHistory = () => {
	const pageSize = 999;
	const [currentPage, setCurrentPage] = useState(0);
	const [trades, setTrades] = useState([]);

	useEffect(() => {
		fetchLatestTrades();
	});

	const fetchLatestTrades = async () => {
		try {
			const response = await axios.get(
				urls.getLatestTrades + `?page=${currentPage}&pageSize=${pageSize}`
			);
			setTrades(response.data);
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<>
			<div className="trade-history">
				<div className="trade-history-header">
					<span className="trade-history-header-item">BTC</span>
					<span className="trade-history-header-item">USD</span>
					<span className="trade-history-header-item">Time</span>
					<span className="trade-history-header-item">Date</span>
				</div>
				<div className="trade-history-container">
					{trades?.length &&
						trades.map((trade, index) => {
							return <SingleTrade trade={trade} key={index} />;
						})}
				</div>
			</div>
		</>
	);
};

export default TradeHistory;
