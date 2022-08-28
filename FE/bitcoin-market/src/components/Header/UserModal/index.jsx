import React, { useCallback, useEffect, useState } from "react";
import Modal from "react-modal";
import "./user.scss";
import axios from "axios";
import { urls } from "../../configs/urls";
import Big from "big.js";
import SingleOrder from "../../Body/OrderHistory/SingleOrder";
import SingleUser from "../../Body/OrderHistory/SingleUser";

const customStyles = {
	content: {
		top: "50%",
		left: "50%",
		right: "auto",
		bottom: "auto",
		transform: "translate(20%, 50%)",
		padding: 0,
		boxShadow: "1px 1px var(--off-black)",
	},
};

const UserModal = ({
	modalIsOpen,
	closeModal,
	username,
	setUserUsdBalance,
	setUserBtcBalance,
	balanceUsd,
	balanceBtc,
}) => {
	const pageSize = 999;
	const [validationError, setValidationError] = useState("");
	const [modalUsdBalance, setModalUsdBalance] = useState(Big(0.0));
	const [modalBtcBalance, setModalBtcBalance] = useState(Big(0.0));
	const [activeOrders, setActiveOrders] = useState([]);
	const [finishedOrders, setFinishedOrders] = useState([]);
	const [currentPage, setCurrentPage] = useState(0);
	const [isUserAdmin, setIsUserAdmin] = useState(false);
	const [isAdminPanel, setIsAdminPanel] = useState(false);
	const [allOrders, setAllOrders] = useState([]);
	const [allUsers, setAllUsers] = useState([]);
	const [tradingVolume, setTradingVolume] = useState(0);
	const [numberOfUsers, setNumberOfUsers] = useState(0);

	useEffect(() => {
		Modal.setAppElement("#App");
		setModalUsdBalance(Big(balanceUsd));
		setModalBtcBalance(Big(balanceBtc));

		fetchUserActiveOrders();
		fetchUserFinishedOrders();
		fetchIsUserAdmin();
		fetchAllOrders();
		fetchAllUsers();
		fetchStats();
	}, [balanceBtc, balanceUsd]);

	const submitUser = async () => {
		try {
			await axios.post(
				urls.setUserBalance,
				{
					balanceUsd: modalUsdBalance,
					balanceBtc: modalBtcBalance,
				},
				{
					headers: {
						Authorization: `Bearer ${localStorage.getItem("Bearer")}`,
					},
				}
			);
			setUserUsdBalance(Big(modalUsdBalance));
			setUserBtcBalance(Big(modalBtcBalance));
		} catch (error) {
			if (error?.response?.data?.length) {
				setValidationError(error?.response?.data);
				setTimeout(() => {
					setValidationError("");
				}, 3000);
			}
		}
	};

	const fetchUserActiveOrders = useCallback(async () => {
		try {
			const response = await axios.get(
				urls.getUserActiveOrders + `?page=${currentPage}&pageSize=${pageSize}`,
				{
					headers: {
						Authorization: `Bearer ${localStorage.getItem("Bearer")}`,
					},
				}
			);
			setActiveOrders(response.data);
		} catch (error) {
			console.error(error);
		}
	});

	const fetchUserFinishedOrders = useCallback(async () => {
		try {
			const response = await axios.get(
				urls.getUserFinishedOrders +
					`?page=${currentPage}&pageSize=${pageSize}`,
				{
					headers: {
						Authorization: `Bearer ${localStorage.getItem("Bearer")}`,
					},
				}
			);
			setFinishedOrders(response.data);
		} catch (error) {
			console.error(error);
		}
	});

	const fetchIsUserAdmin = useCallback(async () => {
		try {
			const response = await axios.get(urls.getIsUserAdmin, {
				headers: {
					Authorization: `Bearer ${localStorage.getItem("Bearer")}`,
				},
			});
			setIsUserAdmin(response.data);
		} catch (error) {
			console.error(error);
		}
	});

	const fetchAllOrders = useCallback(async () => {
		try {
			const response = await axios.get(urls.getAllOrders, {
				headers: {
					Authorization: `Bearer ${localStorage.getItem("Bearer")}`,
				},
			});
			setAllOrders(response.data);
		} catch (error) {
			console.error(error);
		}
	});

	const fetchAllUsers = useCallback(async () => {
		try {
			const response = await axios.get(urls.getAllUsers, {
				headers: {
					Authorization: `Bearer ${localStorage.getItem("Bearer")}`,
				},
			});
			setAllUsers(response.data);
		} catch (error) {
			console.error(error);
		}
	});

	const fetchStats = useCallback(async () => {
		try {
			const response = await axios.get(urls.getStats, {
				headers: {
					Authorization: `Bearer ${localStorage.getItem("Bearer")}`,
				},
			});
			setNumberOfUsers(response.data.numberOfUsers);
			setTradingVolume(response.data.lastDayVolume);
		} catch (error) {
			console.error(error);
		}
	});

	const cancelButtonCallback = async (order) => {
		try {
			await axios.delete(urls.deleteOrder + `?orderId=${order.id}`, {
				headers: {
					Authorization: `Bearer ${localStorage.getItem("Bearer")}`,
				},
			});
			await fetchUserFinishedOrders();
			await fetchUserActiveOrders();
			await fetchAllOrders();
			await fetchStats();
		} catch (error) {
			console.error(error);
		}
	};

	const removeUserCallback = async (user) => {
		try {
			await axios.post(
				urls.deleteUser,
				{
					userToRemoveId: user.id,
				},
				{
					headers: {
						Authorization: `Bearer ${localStorage.getItem("Bearer")}`,
					},
				}
			);
			await fetchAllUsers();
			await fetchStats();
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<Modal
			isOpen={modalIsOpen}
			onRequestClose={closeModal}
			style={customStyles}
			contentLabel="User settings"
			className={"user-modal"}
		>
			<div className="user-modal-header">
				<h2 className="user-modal-title">{username}</h2>
				<span onClick={closeModal} className="user-modal-close">
					x
				</span>
			</div>
			<>
				{!isAdminPanel && (
					<div className="user-modal-body">
						{isUserAdmin && (
							<button
								className="admin-panel-button"
								onClick={() => setIsAdminPanel(true)}
							>
								Admin panel
							</button>
						)}
						<div className="user-modal-balance">
							<div className="balance-inputs">
								<div className="balance-input-usd">
									<span className="balance-usd-label">Usd balance</span>
									<input
										type="number"
										value={modalUsdBalance}
										onChange={(e) => setModalUsdBalance(e.target.value)}
									/>
								</div>
								<div className="balance-input-btc">
									<span className="balance-btc-label">Btc balance</span>
									<input
										type="number"
										value={modalBtcBalance}
										onChange={(e) => setModalBtcBalance(e.target.value)}
									/>
									<div className="submit-button-container">
										<button className="submit-button" onClick={submitUser}>
											Confirm
										</button>
										<span className="validation-error">{validationError}</span>
									</div>
								</div>
							</div>
						</div>
						<div className="user-active-orders">
							<div className="user-active-orders-header">
								<span className="user-active-orders-header-item">
									Active buy orders
								</span>
								<div className="user-active-orders-header-container">
									<span className="user-active-orders-header-item">USD</span>
									<span className="user-active-orders-header-item">Time</span>
									<span className="user-active-orders-header-item">Date</span>
									<span className="user-active-orders-displacement-item">
										Type
									</span>
								</div>
							</div>
							<div className="user-active-orders-container">
								{activeOrders?.length &&
									activeOrders
										.filter((ao) => ao.isBuy)
										.map((order, index) => {
											return (
												<SingleOrder
													order={order}
													key={index}
													isBuy={order.isBuy}
													isSale={!order.isBuy}
													showTypeOfTransaction={false}
													cancelButtonCallback={() =>
														cancelButtonCallback(order)
													}
												/>
											);
										})}
							</div>
						</div>{" "}
						<div className="user-active-orders">
							<div className="user-active-orders-header">
								<span className="user-active-orders-header-item">
									Active sell orders
								</span>
								<div className="user-active-orders-header-container">
									<span className="user-active-orders-header-item">BTC</span>
									<span className="user-active-orders-header-item">Time</span>
									<span className="user-active-orders-header-item">Date</span>
									<span className="user-active-orders-displacement-item">
										Type
									</span>
								</div>
							</div>
							<div className="user-active-orders-container">
								{activeOrders?.length &&
									activeOrders
										.filter((ao) => !ao.isBuy)
										.map((order, index) => {
											return (
												<SingleOrder
													order={order}
													key={index}
													isBuy={order.isBuy}
													isSale={!order.isBuy}
													showTypeOfTransaction={false}
													cancelButtonCallback={() =>
														cancelButtonCallback(order)
													}
												/>
											);
										})}
							</div>
						</div>{" "}
						<div className="user-finished-orders">
							<div className="user-finished-orders-header">
								<span className="user-finished-orders-header-item">
									Finished orders
								</span>
								<div className="user-finished-orders-header-container">
									<span className="user-active-orders-header-item">USD</span>
									<span className="user-active-orders-header-item">BTC</span>
									<span className="user-active-orders-header-item">Time</span>
									<span className="user-active-orders-header-item">Date</span>
									<span className="user-active-orders-header-item">Type</span>
									<span className="user-active-orders-displacement-item">
										Type
									</span>
								</div>
							</div>
							<div className="user-finished-orders-container">
								{finishedOrders?.length &&
									finishedOrders.map((order, index) => {
										return (
											<SingleOrder
												order={order}
												key={index}
												isBuy={order.isBuy}
												isSale={!order.isBuy}
												showTypeOfTransaction={true}
												showBothValues={true}
											/>
										);
									})}
							</div>
						</div>
					</div>
				)}
				{isAdminPanel && (
					<div className="admin-panel">
						<button
							className="admin-panel-button"
							onClick={() => setIsAdminPanel(false)}
						>
							User panel
						</button>
						<div className="stats">
							<div className="stats-trading-volume">
								Last 24h trading volume:{" "}
								<span className="stats-value">{tradingVolume.toFixed(2)}$</span>
							</div>{" "}
							<div className="stats-trading-volume">
								Current users:{" "}
								<span className="stats-value">{numberOfUsers}</span>
							</div>
						</div>
						<div className="user-active-orders">
							<div className="user-active-orders-header">
								<span className="user-active-orders-header-item">
									All orders
								</span>
								<div className="user-active-orders-header-container">
									<span className="user-active-orders-item">BTC</span>
									<span className="user-active-orders-item">Time</span>
									<span className="user-active-orders-item">Date</span>
									<span className="user-active-orders-item">Type</span>
								</div>
							</div>
							<div className="all-orders-container">
								{allOrders?.length &&
									allOrders.map((order, index) => {
										return (
											<SingleOrder
												order={order}
												key={index}
												isBuy={order.isBuy}
												isSale={!order.isBuy}
												showTypeOfTransaction={true}
												cancelButtonCallback={() => cancelButtonCallback(order)}
											/>
										);
									})}
							</div>
						</div>
						<div className="user-active-orders">
							<div className="user-active-orders-header">
								<span className="user-active-orders-header-item">
									All orders
								</span>
								<div className="user-active-orders-header-container">
									<span className="user-active-orders-item">Username</span>
									<span className="user-active-orders-item">USD Balance</span>
									<span className="user-active-orders-item">BTC Balance</span>
									<span className="user-active-orders-item">Type</span>
								</div>
							</div>
							<div className="all-orders-container">
								{allUsers?.length &&
									allUsers.map((user, index) => {
										return (
											<SingleUser
												user={user}
												key={index}
												removeUserCallback={() => removeUserCallback(user)}
											/>
										);
									})}
							</div>
						</div>
					</div>
				)}
			</>
		</Modal>
	);
};

export default UserModal;
