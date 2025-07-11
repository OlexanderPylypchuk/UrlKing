import Auth from "./pages/Auth.jsx";
import Index from './pages/Index.jsx';
import { Routes, Route } from "react-router-dom";

function App() {
    return (
        <Routes>
            <Route path="/" element={<Index />} />
            <Route path="/auth" element={<Auth />} />
        </Routes>
    );
}

export default App;