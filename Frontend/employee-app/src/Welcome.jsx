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

  const handleLogout = () => {
    localStorage.removeItem("username");
    localStorage.removeItem("userId"); 
    localStorage.removeItem("token");
    localStorage.removeItem('scrollPosition'); // Clear scroll position on logout
    navigate("/"); // Redirect to login after logout
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
