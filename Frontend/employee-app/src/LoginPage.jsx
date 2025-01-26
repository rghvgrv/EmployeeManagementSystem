import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const LoginPage = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();

        const credentials = { username, password };

        try {
            const response = await fetch("http://localhost:5059/api/Auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(credentials),
            });

            const data = await response.json();

            if (response.ok) {
                // Handle successful login, e.g., redirect to another page
                console.log(data);
                localStorage.setItem('username', data.username);
                localStorage.setItem("userId", data.userid); // Save userId from login response
                localStorage.setItem("token", data.token);
                navigate('/welcome'); 
            } else {
                setError(data.message || "Invalid login or password");
            }
        } catch (err) {
            setError("An error occurred while logging in");
        }
    };

    return (
        <div className="login">
            <h1>Login</h1>
            <form onSubmit={handleLogin}>
                <div>
                    <input
                        type="text"
                        placeholder="Username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit">Login</button>
            </form>
            <div className="newbtn">
            <button id="forgot-psw" onClick={() => navigate("/new-password")}>
                Forgot Password
            </button>
            <button id="new-user" onClick={() => navigate("/new-user")}>
                New User
            </button>
            </div>
            {error && <p style={{ color: "red" }}>{error}</p>}
        </div>
    );
};

export default LoginPage;
