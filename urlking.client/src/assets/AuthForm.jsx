import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import Cookies from "js-cookie";

export default function AuthForm() {
    const [isRegistering, setIsRegistering] = useState(false);
    const [username, setUsername] = useState("");
    const [name, setName] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const { login } = useAuth();

    const navigate = useNavigate();

    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setError("");
        setSuccessMessage("");

        const url = isRegistering ? "/api/auth/register" : "/api/auth/login";
        const body = isRegistering
            ? { username, name, password }
            : { username, password };

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(body),
            });

            if (!response.ok) {
                const text = await response.text();
                throw new Error(text || "Failed to authenticate");
            }
            else {
                const data = await response.json();
                setSuccessMessage("Success");
                const token = data.accessToken;
                if (token) {
                    login(token);
                    navigate("/");      
                }
            }

        } catch (err) {
            setError(err.message || "Unexpected error");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <div className="w-full max-w-md p-8 bg-white rounded shadow-md">
                <h2 className="text-2xl font-bold mb-6 text-center">
                    {isRegistering ? "Register" : "Login"}
                </h2>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <input
                        type="text"
                        placeholder="Username"
                        required
                        className="w-full px-4 py-2 border rounded focus:outline-none focus:ring focus:border-blue-500"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />

                    {isRegistering && (
                        <input
                            type="text"
                            placeholder="Name"
                            required
                            className="w-full px-4 py-2 border rounded focus:outline-none focus:ring focus:border-blue-500"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        />
                    )}

                    <input
                        type="password"
                        placeholder="Password"
                        required
                        className="w-full px-4 py-2 border rounded focus:outline-none focus:ring focus:border-blue-500"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 rounded transition duration-200 disabled:opacity-50"
                    >
                        {isRegistering ? "Sign Up" : "Login"}
                    </button>
                </form>

                {isLoading && (
                    <p className="text-sm text-blue-600 text-center mt-3">Loading...</p>
                )}
                {successMessage && (
                    <p className="text-sm text-green-600 text-center mt-3">
                        {successMessage}
                    </p>
                )}
                {error && (
                    <p className="text-sm text-red-600 text-center mt-3">{error}</p>
                )}

                <p
                    className="text-sm text-center text-blue-600 mt-6 cursor-pointer hover:underline"
                    onClick={() => {
                        setIsRegistering(!isRegistering);
                        setError("");
                        setSuccessMessage("");
                    }}
                >
                    {isRegistering
                        ? "Already have an account? Login"
                        : "Don't have an account? Register"}
                </p>
            </div>
        </div>
    );
}