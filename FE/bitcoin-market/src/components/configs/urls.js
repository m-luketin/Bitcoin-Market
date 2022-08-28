const baseUrl = "https://localhost:44322/api";

const controllerUrls = {
	order: baseUrl + "/Order",
	user: baseUrl + "/User",
};

export const urls = {
	addOrder: controllerUrls.order + "/add",
	getLatestOrders: controllerUrls.order + "/latest",
	getLatestSales: controllerUrls.order + "/sales",
	getLatestBuys: controllerUrls.order + "/buys",
	getOrdersByUser: controllerUrls.order + "/user-orders",
	getSalesByUser: controllerUrls.order + "/user-sales",
	getPurchasesByUser: controllerUrls.order + "/user-purchases",
	getChartData: controllerUrls.order + "/chart",
	getUserActiveOrders: controllerUrls.order + "/active-orders",
	getUserFinishedOrders: controllerUrls.order + "/finished-orders",
	deleteOrder: controllerUrls.order,
	getAllOrders: controllerUrls.order + "/get",
	getStats: controllerUrls.order + "/stats",
	getUserById: controllerUrls.user + "/get",
	getUserByUsername: controllerUrls.user + "/username",
	registerUser: controllerUrls.user + "/register",
	getUsersByOrder: controllerUrls.user + "/order",
	logInUser: controllerUrls.user + "/login",
	setUserBalance: controllerUrls.user + "/balance",
	getIsUserAdmin: controllerUrls.user + "/admin",
	getAllUsers: controllerUrls.user + "/get",
	deleteUser: controllerUrls.user + "/admin-remove-user",
};
