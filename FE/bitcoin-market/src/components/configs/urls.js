const baseUrl = "https://localhost:44322/api";

const controllerUrls = {
	trade: baseUrl + "/Trade",
	user: baseUrl + "/User",
};

export const urls = {
	addTrade: controllerUrls.trade + "/add",
	getLatestTrades: controllerUrls.trade + "/latest",
	getLatestOffers: controllerUrls.trade + "/get",
	getTradesByUser: controllerUrls.trade + "/user-trades",
	getSalesByUser: controllerUrls.trade + "/user-sales",
	getPurchasesByUser: controllerUrls.trade + "/user-purchases",
	getUserById: controllerUrls.user + "/get",
	getUserByUsername: controllerUrls.user + "/username",
	registerUser: controllerUrls.user + "/register",
	getUsersByTrade: controllerUrls.user + "/trade",
	logInUser: controllerUrls.user + "/login",
};
