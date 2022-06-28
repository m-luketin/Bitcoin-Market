import React from "react";
import TradeHistory from "./TradeHistory";
import Trading from "./Trading";
import "./body.scss";

const Body = () => {
	return (
		<div className="body">
			<TradeHistory />
			<Trading />
		</div>
	);
};

export default Body;
