import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom'; // Import useNavigate

const WelcomePage = () => {
    const [username, setUsername] = useState('');
    const navigate = useNavigate(); // Initialize navigate function

    useEffect(() => {
        // Retrieve the username from local storage
        const storedUsername = localStorage.getItem('username');
        if (storedUsername) {
            setUsername(storedUsername);
        } else {
            // Redirect to login page if no user is logged in
            navigate('/'); // Use navigate to go to the login page
        }
    }, [navigate]);

    const handleLogout = () => {
        // Clear username from localStorage and navigate to login page
        localStorage.removeItem('username');
        navigate('/'); // Redirect to login page after logout
    };

    return (
        <div className="welcome">
            <button id='logout' onClick={handleLogout}>
                Logout
            </button>
            <h1>Welcome {username}!</h1>
        </div>
    );
};

export default WelcomePage;
