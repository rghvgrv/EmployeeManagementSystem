import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom"; 

const WelcomePage = () => {
  const [username, setUsername] = useState("");
  const [userId, setUserId] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    // Retrieve session data from localStorage (will persist across tabs)
    const storedUsername = localStorage.getItem("username");
    const storedUserId = localStorage.getItem("userId");

    if (storedUsername && storedUserId) {
      setUsername(storedUsername);
      setUserId(storedUserId);
    } else {
      navigate("/");  // Redirect to login if no session data
    }

    // Sync session across tabs
    const handleStorageChange = (event) => {
      if (event.key === "username" || event.key === "userId") {
        setUsername(localStorage.getItem("username"));
        setUserId(localStorage.getItem("userId"));
      }
    };

    // Add event listener for storage changes in other tabs
    window.addEventListener("storage", handleStorageChange);

    return () => {
      // Cleanup the event listener when the component unmounts
      window.removeEventListener("storage", handleStorageChange);
    };
  }, [navigate]);

  const viewProfile = () => {
    navigate(`/GetEmployeeByUserId/${userId}`);
  };

  const handleLogout = async () => {
    const employeeUserId = localStorage.getItem("userId");  // Get the userId from localStorage
    const token = localStorage.getItem("token");    // Optionally get the token
    // Make a POST request to the backend to log the user out and update IsActive to false
    if (employeeUserId && token) {
        try {
            const response = await fetch("http://localhost:5059/api/Auth/logout", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",// Optional: pass token if needed for authentication
                },
                body: JSON.stringify({ employeeUserId }), // Send userId to backend
            });

            if (response.ok) {
                localStorage.removeItem("username");
                localStorage.removeItem("userId"); 
                localStorage.removeItem("token"); // Clear localStorage
                navigate("/"); // Redirect to login page
            } else {
                console.error("Logout failed", await response.text());
            }
        } catch (error) {
            console.error("Error logging out:", error);
        }
    } else {
        console.error("User is not logged in.");
    } // Redirect to login after logout
  };

  return (
    <div className="welcome">
      <button id="logout" onClick={handleLogout}>
        Logout
      </button>
      <h1>Welcome {username}!</h1>
      <button id="viewProfile" onClick={viewProfile}>
        View Profile
      </button>
    </div>
  );
};

export default WelcomePage;
