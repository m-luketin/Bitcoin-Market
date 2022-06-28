import "./App.scss";
import Body from "./components/Body";
import Header from "./components/Header";
import ChartComponent from "./components/ChartComponent";

function App() {
	return (
		<div className="App">
			<Header />
			<Body />
			<ChartComponent />
		</div>
	);
}

export default App;
