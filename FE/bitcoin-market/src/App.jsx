import "./App.scss";
import Body from "./components/Body";
import Header from "./components/Header";
import ChartComponent from "./components/ChartComponent";
import { useState } from "react";
import Offers from "./components/Body/Trading/Offers";
import OrderHistory from "./components/Body/OrderHistory";

function App() {
	const [isLoggedIn, setIsLoggedIn] = useState(false);
	const [hackyRerenderOffersVariable, setHackyRerenderOffersVariable] =
		useState(0);

	return (
		<div className="App" id="App">
			<Header isLoggedIn={isLoggedIn} setIsLoggedIn={setIsLoggedIn} />
			<Body
				isLoggedIn={isLoggedIn}
				rerenderOffers={setHackyRerenderOffersVariable}
				hackyRerenderVariable={hackyRerenderOffersVariable}
			/>
			<div className="app-order-data-container">
				<OrderHistory />
				<Offers hackyRerenderVariable={hackyRerenderOffersVariable} />
			</div>
		</div>
	);
}

export default App;
