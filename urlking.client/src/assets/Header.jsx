import { useAuth } from "../contexts/AuthContext";

function Header() {
    const { accessToken, logout } = useAuth();

    const handleLogout = () => {
        logout();
    };

    return (
        <header className="bg-white shadow-md px-6 py-4 flex justify-between items-center">
            <div className="flex items-center space-x-6">
                <a href="/" className="text-xl font-bold text-blue-600">
                    URL Shortener
                </a>
                <nav className="flex space-x-4 text-sm text-gray-700">
                    <a href="/url" className="hover:text-blue-500">URL</a>
                    <a href="/about" className="hover:text-blue-500">About</a>
                </nav>
            </div>

            <div className="flex items-center space-x-4 text-sm text-gray-700">
                {accessToken ? (
                    <button onClick={handleLogout} className="hover:text-red-500">
                        Logout
                    </button>
                ) : (
                    <a href="/auth" className="hover:text-blue-500">Login</a>
                )}
            </div>
        </header>
    );
}

export default Header;