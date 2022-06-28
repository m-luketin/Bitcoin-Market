import React from "react";
import "./header.scss";

const Header = () => {
	return (
		<header className="header">
			<div className="header-image-container">
				<img src="icons/bitcoin.svg" alt="bitcoin icon" />
				<h1 className="header-title">Bitcoin marketplace</h1>
			</div>
			<div className="header-button-container">
				<button className="button header-button login-button">Login</button>
				<button className="button header-button register-button">
					Register
				</button>
			</div>
		</header>
	);
};

export default Header;
