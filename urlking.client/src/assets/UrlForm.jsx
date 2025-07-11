import { useState } from "react";
import { useAuth } from "../contexts/AuthContext";


function UrlForm({ onSuccess }) {
    const [url, setUrl] = useState("");
    const [shortUrl, setShortUrl] = useState("");
    const [error, setError] = useState("");
    const { accessToken } = useAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch("/api/url", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
                body: JSON.stringify( url ),
            });

            if (!response.ok) {
                const message = await response.text();
                throw new Error(message || "Failed to shorten URL");
            }
            const data = await response.json();
            setShortUrl(data.shortUrl);

            onSuccess?.();
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="flex flex-col items-center justify-center bg-gray-100 min-w-full">
            <div className="min-w-3/4 mx-auto mt-2 p-6 bg-white">
                <h2 className="text-2xl font-bold mb-4 text-center">Shorten Your URL</h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <input
                        type="url"
                        placeholder="Enter a long URL..."
                        className="w-full px-4 py-2 border rounded"
                        value={url}
                        onChange={(e) => setUrl(e.target.value)}
                        required
                    />
                    <button
                        type="submit"
                        className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 rounded"
                    >
                        Shorten
                    </button>
                </form>
                <a href={shortUrl} className="underline text-blue-600" target="_blank" rel="noopener noreferrer">
                    {shortUrl}
                </a>
                {error && <p className="mt-4 text-red-600 text-center">{error}</p>}
            </div>
        </div>
    );
}

export default UrlForm;