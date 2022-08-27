import React from "react";
import OrderHistory from "./OrderHistory";
import Trading from "./Trading";
import "./body.scss";
import ChartComponent from "../ChartComponent";

const Body = ({ isLoggedIn, rerenderOffers, hackyRerenderVariable }) => {
	return (
		<div className="body">
			<ChartComponent hackyRerenderVariable={hackyRerenderVariable} />
			<Trading
				isLoggedIn={isLoggedIn}
				rerenderOffers={rerenderOffers}
				hackyRerenderVariable={hackyRerenderVariable}
			/>
		</div>
	);
};

export default Body;
