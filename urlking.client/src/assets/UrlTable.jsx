import { useEffect, useState } from 'react';
import { useAuth } from '../contexts/AuthContext'; 

function UrlTable({ refreshTrigger }) {
    const [urls, setUrls] = useState();
    const { accessToken, userId, role } = useAuth();

    useEffect(() => {
        populateUrls();
    }, [refreshTrigger]);

    async function populateUrls() {
        const response = await fetch('/api/url');
        console.log(response)
        if (response.ok) {
            const data = await response.json();
            setUrls(data);
        }
    }

    async function handleDelete(code) {
        const confirmed = window.confirm("Are you sure you want to delete this URL?");
        if (!confirmed) return;

        try {
            const response = await fetch(`/api/url/${code}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || "Failed to delete");
            }

            // Remove deleted item from table
            setUrls(prev => prev.filter(url => url.code !== code));
        } catch (err) {
            alert("Delete failed: " + err.message);
        }
    }

    const contents = urls === undefined
        ? <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 min-w-full">
            <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        </div>
        : <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 min-w-full">
            <table className="w-1/2 table-auto border border-gray-300 rounded-lg shadow-md overflow-hidden text-sm">
                <thead className="bg-gray-100 text-gray-700 uppercase font-semibold">
                    <tr>
                        <th className="border border-gray-300 px-4 py-2 text-left">Original Url</th>
                        <th className="border border-gray-300 px-4 py-2 text-left">Short Url</th>
                        <th className="border border-gray-300 px-4 py-2 text-left">Created time</th>
                        <th className="border border-gray-300 px-4 py-2 text-left">Created time</th>
                        <th className="border border-gray-300 px-4 py-2 text-left"></th>
                    </tr>
                </thead>
                <tbody className="text-gray-800">
                    {urls.map((url, index) => (
                        <tr
                            key={url.code}
                            className={
                                index % 2 === 0
                                    ? "bg-white"
                                    : "bg-gray-50 hover:bg-gray-100 transition-colors"
                            }
                        >
                            <td className="border border-gray-300 px-4 py-2">{url.longUrl}</td>
                            <td className="border border-gray-300 px-4 py-2">{url.shortUrl}</td>
                            <td className="border border-gray-300 px-4 py-2">{url.createdDate}</td>
                            <td className="border border-gray-300 px-4 py-2">{userId}</td>
                            <th className="border border-gray-300 px-4 py-2 text-left">{(accessToken && (url.userId === userId || role === "admin")) && (
                                <button className="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded" onClick={() => handleDelete(url.code)}>
                                Delete
                            </button>)}</th>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>


    return (
        contents
    );
}

export default UrlTable;