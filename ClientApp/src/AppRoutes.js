import { Counter } from "./components/Counter";
import { WeatherForecast } from "./components/WeatherForecast";
import { Home } from "./components/Home";
import { Countries } from "./components/Countries";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/counter',
        element: <Counter />
    },
    {
        path: '/weather-forecast',
        element: <WeatherForecast />
    },
    {
        path: '/countries',
        element: < Countries />
    }

];

export default AppRoutes;
