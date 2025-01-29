import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const WelcomePage = () => {
  const [username, setUsername] = useState("");
  const [userId, setUserId] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    // Retrieve session data from localStorage
    const storedUsername = localStorage.getItem("username");
    const storedUserId = localStorage.getItem("userId");

    if (storedUsername && storedUserId) {
      setUsername(storedUsername);
      setUserId(storedUserId);
    } else {
      navigate("/"); // Redirect to login if no session data
    }

    // Sync session across tabs
    const handleStorageChange = (event) => {
      if (event.key === "username" || event.key === "userId") {
        const newUsername = localStorage.getItem("username");
        const newUserId = localStorage.getItem("userId");

        if (!newUsername || !newUserId) {
          // User logged out in another tab
          navigate("/");
        } else {
          setUsername(newUsername);
          setUserId(newUserId);
        }
      }
    };

    // Add event listener for storage changes
    window.addEventListener("storage", handleStorageChange);

    // Periodic session validation
    const validateSession = async () => {
      const token = localStorage.getItem("token");
      if (token && storedUserId) {
        try {
          const response = await fetch(`http://localhost:5059/api/Auth/validate-session/${storedUserId}`, {
            method: "GET",
            headers: {
              "Authorization": `Bearer ${token}`,
            },
          });

          if (!response.ok) {
            // Session invalid, log out the user
            handleLogout();
          }
        } catch (error) {
          console.error("Error validating session:", error);
          handleLogout(); // Log out on error
        }
      }
    };

    const intervalId = setInterval(validateSession, 10000); // Check every sec

    // WebSocket for real-time logout
    const socket = new WebSocket("ws://localhost:5059");

    socket.onmessage = (message) => {
      const data = JSON.parse(message.data);
      if (data.type === "logout" && data.userId === storedUserId) {
        alert("You have been logged out due to another login.");
        handleLogout();
      }
    };

    return () => {
      // Cleanup event listeners and intervals
      window.removeEventListener("storage", handleStorageChange);
      clearInterval(intervalId);
      socket.close();
    };
  }, [navigate]);

  const viewProfile = () => {
    navigate(`/GetEmployeeByUserId/${userId}`);
  };

  const handleLogout = async () => {
    const employeeUserId = localStorage.getItem("userId");
    const token = localStorage.getItem("token");

    if (employeeUserId && token) {
      try {
        const response = await fetch("http://localhost:5059/api/Auth/logout", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
          },
          body: JSON.stringify({ employeeUserId }),
        });

        if (response.ok) {
          localStorage.removeItem("username");
          localStorage.removeItem("userId");
          localStorage.removeItem("token");
          navigate("/"); // Redirect to login page
        } else {
          console.error("Logout failed", await response.text());
        }
      } catch (error) {
        console.error("Error logging out:", error);
      }
    } else {
      console.error("User is not logged in.");
    }
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
