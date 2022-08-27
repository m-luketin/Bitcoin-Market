import React, { useState } from "react";
import Modal from "react-modal";
import "./register.scss";
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

const RegisterModal = ({ modalIsOpen, closeModal, switchModal }) => {
	const afterOpenModal = () => {};

	const [username, setUsername] = useState();
	const [password, setPassword] = useState();
	const [validationError, setValidationError] = useState("");

	const submitRegister = async () => {
		try {
			const response = await axios.post(urls.registerUser, {
				username,
				password,
			});

			switchModal();
		} catch (error) {
			if (error?.response?.data?.length) {
				setValidationError(error?.response?.data);
				// setTimeout(() => {
				// 	setValidationError("");
				// }, 3000);
			}
		}
	};

	return (
		<Modal
			isOpen={modalIsOpen}
			onAfterOpen={afterOpenModal}
			onRequestClose={closeModal}
			style={customStyles}
			contentLabel="Example Modal"
		>
			<div className="register-modal-header">
				<h2 className="register-modal-title">Register</h2>
				<span onClick={closeModal} className="register-modal-close">
					x
				</span>
			</div>
			<div className="register-modal-body">
				<div className="register-inputs">
					<div className="register-input-username">
						<span className="username-label">Username</span>
						<input
							type="text"
							value={username}
							onChange={(e) => setUsername(e.target.value)}
						/>
					</div>
					<div className="register-input-password">
						<span className="password-label">Password</span>
						<input
							type="password"
							value={password}
							onChange={(e) => setPassword(e.target.value)}
						/>
					</div>
				</div>
				<div className="submit-button-container">
					<button className="submit-button" onClick={submitRegister}>
						Confirm
					</button>
					<span className="validation-error">{validationError}</span>
				</div>
				<div className="register-switch">
					<span className="switch-label">Don't have an account?</span>
					<span className="switch-button" onClick={switchModal}>
						Log in
					</span>
				</div>
			</div>
		</Modal>
	);
};

export default RegisterModal;
