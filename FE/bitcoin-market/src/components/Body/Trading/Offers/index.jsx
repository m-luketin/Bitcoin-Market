import React from "react";
import { useEffect, useState, useCallback } from "react";
import "./offers.scss";
import axios from "axios";
import { urls } from "../../../configs/urls";
import SingleOrder from "../../OrderHistory/SingleOrder";
import Big from "big.js";

const Offers = ({ value, hackyRerenderVariable }) => {
	const pageSize = 999;
	const [currentSalesPage, setCurrentSalesPage] = useState(0);
	const [currentBuysPage, setCurrentBuysPage] = useState(0);
	const [sales, setSales] = useState([]);
	const [buys, setBuys] = useState([]);
	const [saleSums, setSaleSums] = useState([]);
	const [buySums, setBuySums] = useState([]);
	const [totalSaleSum, setTotalSaleSum] = useState(new Big(0));
	const [totalBuySum, setTotalBuySum] = useState(new Big(0));

	const fetchLatestSales = useCallback(async () => {
		try {
			const response = await axios.get(
				urls.getLatestSales + `?page=${currentSalesPage}&pageSize=${pageSize}`
			);
			setSales(response.data);
			return response.data;
		} catch (error) {
			console.error(error);
		}
	}, [currentSalesPage]);

	const fetchLatestBuys = useCallback(async () => {
		try {
			const response = await axios.get(
				urls.getLatestBuys + `?page=${currentBuysPage}&pageSize=${pageSize}`
			);
			setBuys(response.data);
			return response.data;
		} catch (error) {
			console.error(error);
		}
	}, [currentBuysPage]);

	useEffect(() => {
		getData();
	}, [hackyRerenderVariable]);

	const getData = async () => {
		const fetchedSales = await fetchLatestSales();
		const fetchedBuys = await fetchLatestBuys();
		calculateSums(fetchedSales, fetchedBuys);
	};

	const calculateSums = (fetchedSales, fetchedBuys) => {
		let salesSum = Big(0.0);
		const allSalesSums = [];

		fetchedSales.forEach((sale) => {
			salesSum = salesSum.plus(sale.valueInBtc).minus(sale.filledValue);
			allSalesSums.push(salesSum);
		});
		setSaleSums(allSalesSums);
		setTotalSaleSum(salesSum);

		let buysSum = new Big(0.0);
		const allBuysSums = [];
		fetchedBuys.forEach((buy) => {
			buysSum = buysSum.plus(buy.valueInBtc).minus(buy.filledValue);
			allBuysSums.push(buysSum);
		});
		setBuySums(allBuysSums);
		setTotalBuySum(buysSum);
	};

	return (
		<div className="offers">
			<div className="buys">
				<div className="buys-header">
					<span className="buys-header-item">Buys</span>
					<div className="buys-header-container">
						<span className="buys-header-item">Date</span>
						<span className="buys-header-item">Time</span>
						<span className="buys-header-item">Sum</span>
						<span className="buys-header-item">Bid(USD)</span>
						<span className="buys-header-item">Ask($)</span>
					</div>
				</div>
				<div className="buys-container">
					{!!buys?.length &&
						buys.map((buy, index) => {
							return (
								<SingleOrder
									order={buy}
									isBuy={true}
									key={index}
									sum={buySums[index]}
									showBothValues={true}
									sumPercentage={buySums[index]
										.div(totalBuySum.eq(0) ? 1 : totalBuySum)
										.times(100)
										.toPrecision(8)}
								/>
							);
						})}
				</div>
			</div>
			<div className="sales">
				<div className="sales-header">
					<span className="sales-header-item">Sells</span>
					<div className="sales-header-container">
						<span className="sales-header-item">Ask(BTC)</span>
						<span className="sales-header-item">Bid($)</span>
						<span className="sales-header-item">Sum</span>
						<span className="sales-header-item">Time</span>
						<span className="sales-header-item">Date</span>
					</div>
				</div>
				<div className="sales-container">
					{!!sales?.length &&
						sales.map((sale, index) => {
							return (
								<SingleOrder
									order={sale}
									isSale={true}
									key={index}
									sum={saleSums[index]}
									sumPercentage={
										totalSaleSum.eq(0)
											? 0
											: saleSums[index]?.div(totalSaleSum)?.times(100)
									}
									showBothValues={true}
								/>
							);
						})}
				</div>
			</div>
		</div>
	);
};

export default Offers;
