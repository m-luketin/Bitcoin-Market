import React from "react";
import { useEffect, useState } from "react";
import "./orderHistory.scss";

import axios from "axios";
import { urls } from "../../configs/urls";
import SingleOrder from "./SingleOrder";

const OrderHistory = () => {
	const pageSize = 999;
	const [currentPage, setCurrentPage] = useState(0);
	const [orders, setOrders] = useState([]);

	useEffect(() => {
		fetchLatestOrders();
	}, []);

	const fetchLatestOrders = async () => {
		try {
			const response = await axios.get(
				urls.getLatestOrders + `?page=${currentPage}&pageSize=${pageSize}`
			);
			setOrders(response.data);
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<div className="order-history">
			<div className="order-history-header">
				<span className="order-history-header-item">Orders</span>
				<div className="order-history-header-container">
					<span className="order-history-header-item">BTC</span>
					<span className="order-history-header-item">USD</span>
					<span className="order-history-header-item">Time</span>
					<span className="order-history-header-item">Date</span>
				</div>
			</div>
			<div className="order-history-container">
				{orders?.length &&
					orders.map((order, index) => {
						return <SingleOrder order={order} key={index} />;
					})}
			</div>
		</div>
	);
};

export default OrderHistory;
