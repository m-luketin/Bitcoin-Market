import React, { useState } from "react";
import "./header.scss";
import LoginModal from "./LoginModal";
import RegisterModal from "./RegisterModal";
import UserModal from "./UserModal";
import Big from "big.js";

const Header = ({ isLoggedIn, setIsLoggedIn }) => {
	const [isLoginModalOpen, setIsLoginModalOpen] = useState(false);
	const [isRegisterModalOpen, setIsRegisterModalOpen] = useState(false);
	const [isUserModalOpen, setIsUserModalOpen] = useState(false);
	const [balanceUsd, setBalanceUsd] = useState(Big(0.0));
	const [balanceBtc, setBalanceBtc] = useState(Big(0.0));
	const [username, setUsername] = useState("");

	return (
		<header className="header">
			<div className="header-image-container">
				<img src="icons/bitcoin.svg" alt="bitcoin icon" />
				<h1 className="header-title">Bitcoin marketplace</h1>
			</div>
			{!isLoggedIn && (
				<div className="header-button-container">
					<button
						className="button header-button login-button"
						onClick={() => setIsLoginModalOpen(true)}
					>
						Login
					</button>
					<button
						className="button header-button register-button"
						onClick={() => setIsRegisterModalOpen(true)}
					>
						Register
					</button>
				</div>
			)}
			{isLoggedIn && (
				<div className="header-right" onClick={() => setIsUserModalOpen(true)}>
					<div className="header-username">{username}</div>
					<div className="header-balance-container">
						<div className="header-balance header-balance-usd">
							<span>USD: </span>
							<span>{balanceUsd.toFixed(2)}</span>
						</div>
						<div className="header-balance header-balance-btc">
							<span>BTC: </span>
							<span>{balanceBtc.toFixed(8)}</span>
						</div>
					</div>
				</div>
			)}
			<LoginModal
				modalIsOpen={isLoginModalOpen}
				closeModal={() => setIsLoginModalOpen(false)}
				switchModal={() => {
					setIsLoginModalOpen(false);
					setIsRegisterModalOpen(true);
				}}
				setIsLoggedIn={setIsLoggedIn}
				setBalanceUsd={setBalanceUsd}
				setBalanceBtc={setBalanceBtc}
				setHeaderUsername={setUsername}
			/>
			<RegisterModal
				modalIsOpen={isRegisterModalOpen}
				closeModal={() => setIsRegisterModalOpen(false)}
				switchModal={() => {
					setIsRegisterModalOpen(false);
					setIsLoginModalOpen(true);
				}}
			/>
			<UserModal
				modalIsOpen={isUserModalOpen}
				closeModal={() => setIsUserModalOpen(false)}
				username={username}
				setUserUsdBalance={setBalanceUsd}
				setUserBtcBalance={setBalanceBtc}
				balanceUsd={balanceUsd}
				balanceBtc={balanceBtc}
			/>
		</header>
	);
};

export default Header;
