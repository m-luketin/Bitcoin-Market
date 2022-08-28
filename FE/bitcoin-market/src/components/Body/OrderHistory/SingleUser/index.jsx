import Big from "big.js";
import React from "react";
import "./singleUser.scss";

const SingleUser = ({ user, removeUserCallback }) => {
	return (
		user && (
			<div className="single-user">
				<span className="single-user-item single-user-btc-value">
					{user?.username}
				</span>
				<span className="single-user-item single-user-usd-value">
					{Big(user?.usdBalance).toFixed(2)}
				</span>
				<span className="single-user-item single-user-time">
					{Big(user?.btcBalance).toFixed(8)}
				</span>
				<span className="single-user-item single-user-btc-value">
					{user?.isAdmin ? "Admin" : "User"}
				</span>
				<>
					<span
						className="single-user-item-cancel"
						onClick={removeUserCallback}
					>
						x
					</span>
				</>
			</div>
		)
	);
};

export default SingleUser;
