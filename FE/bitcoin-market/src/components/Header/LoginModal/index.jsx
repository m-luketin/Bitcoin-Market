import React, { useEffect, useState } from "react";
import Modal from "react-modal";
import "./login.scss";
import axios from "axios";
import { urls } from "../../configs/urls";

const customStyles = {
	content: {
		top: "50%",
		left: "50%",
		right: "auto",
		bottom: "auto",
		marginRight: "-50%",
		transform: "translate(-50%, -50%)",
		padding: 0,
		boxShadow: "1px 1px var(--off-black)",
	},
};

const LoginModal = ({
	modalIsOpen,
	closeModal,
	switchModal,
	setIsLoggedIn,
	setBalanceUsd,
	setBalanceBtc,
	setHeaderUsername,
}) => {
	const [username, setUsername] = useState();
	const [password, setPassword] = useState();
	const [validationError, setValidationError] = useState("");

	useEffect(() => {
		Modal.setAppElement("#App");
	}, []);

	const submitLogin = async () => {
		try {
			const response = await axios.post(urls.logInUser, { username, password });

			setIsLoggedIn(true);
			closeModal();
			setBalanceUsd(response.data?.usdBalance);
			setBalanceBtc(response.data?.btcBalance);
			localStorage.setItem("Bearer", response.data.token);
			setHeaderUsername(username);
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
		<Modal
			isOpen={modalIsOpen}
			onRequestClose={closeModal}
			style={customStyles}
			contentLabel="Example Modal"
		>
			<div className="login-modal-header">
				<h2 className="login-modal-title">Log in</h2>
				<span onClick={closeModal} className="login-modal-close">
					x
				</span>
			</div>
			<div className="login-modal-body">
				<div className="login-inputs">
					<div className="login-input-username">
						<span className="username-label">Username</span>
						<input
							type="text"
							value={username}
							onChange={(e) => setUsername(e.target.value)}
						/>
					</div>
					<div className="login-input-password">
						<span className="password-label">Password</span>
						<input
							type="password"
							value={password}
							onChange={(e) => setPassword(e.target.value)}
						/>
					</div>
				</div>
				<div className="submit-button-container">
					<button className="submit-button" onClick={submitLogin}>
						Confirm
					</button>
					<span className="validation-error">{validationError}</span>
				</div>
				<div className="login-switch">
					<span className="switch-label">Don't have an account?</span>
					<span className="switch-button" onClick={switchModal}>
						Register
					</span>
				</div>
			</div>
		</Modal>
	);
};

export default LoginModal;
