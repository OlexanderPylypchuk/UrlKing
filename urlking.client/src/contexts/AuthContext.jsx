import { createContext, useContext, useEffect, useState } from "react";
import Cookies from "js-cookie";
import { jwtDecode } from "jwt-decode";

const AuthContext = createContext();

export function AuthProvider({ children }) {
    const [accessToken, setAccessToken] = useState(null);
    const [userId, setUserId] = useState(null);
    const [role, setRole] = useState(null);

    // Load token from cookie on initial mount
    useEffect(() => {
        const token = Cookies.get("accessToken");
        setAccessToken(token || null);
        if (token) {
            try {
                const decoded = jwtDecode(token);
                setUserId(decoded["unique_name"]);
                setRole(decoded["role"]);
            } catch (e) {
                console.error("Invalid token");
                setUserId(null);
                setRole(null);
            }
        } else {
            setUserId(null);
            setRole(null);
        }
    }, []);

    // Login = store token in cookie and state
    const login = (token) => {
        Cookies.set("accessToken", token, { expires: 1, secure: true, sameSite: "Strict" });
        setAccessToken(token);
        try {
            const decoded = jwtDecode(token);
            setUserId(decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]);
            setRole(decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]);
        } catch (e) {
            setUserId(null);
            setRole(null);
        }
    };

    // Logout = remove token
    const logout = () => {
        Cookies.remove("accessToken");
        setAccessToken(null);
        setUserId(null);
        setRole(null);
    };

    return (
        <AuthContext.Provider value={{ accessToken, login, logout, userId, role }}>
            {children}
        </AuthContext.Provider>
    );
}

// Hook to use auth easily in any component
export function useAuth() {
    return useContext(AuthContext);
}