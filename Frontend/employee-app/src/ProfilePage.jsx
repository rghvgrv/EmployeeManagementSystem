import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

const ProfilePage = () => {
    const [profile, setProfile] = useState(null);
    const [error, setError] = useState("");
    const { userId } = useParams();
    const navigate = useNavigate();

    const token = localStorage.getItem("token");

    useEffect(() => {
        if (!token || !userId) {
            redirectToLogin("Invalid session. Redirecting to login...");
            return;
        }

        validateToken();
    }, [userId, token, navigate]);

    // ✅ Centralized session validation
    const validateToken = async () => {
        try {
            const response = await fetch(`http://localhost:5059/api/Auth/validate-session/${userId}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.ok) {
                const newToken = response.headers.get("New-Token");
                if (newToken) {
                    localStorage.setItem("token", newToken); // ✅ Update token if refreshed
                }
                fetchProfile();
            } else {
                redirectToLogin("Session expired. Redirecting to login...");
            }
        } catch (err) {
            redirectToLogin("An error occurred while validating the session.");
        }
    };

    // ✅ Fetch employee profile
    const fetchProfile = async () => {
        try {
            const response = await fetch(`http://localhost:5059/api/Employee/GetEmployeeByUserId/${userId}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("token")}`, // ✅ Always use latest token
                },
            });

            if (response.ok) {
                const data = await response.json();
                setProfile(data);
            } else {
                setError("Failed to fetch profile details.");
            }
        } catch (err) {
            setError("An error occurred while fetching profile details.");
        }
    };

    // ✅ Back button validation before navigating
    const handleBack = async () => {
        try {
            const response = await fetch(`http://localhost:5059/api/Auth/validate-session/${userId}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("token")}`, // ✅ Always use latest token
                },
            });

            if (response.ok) {
                navigate("/welcome");
            } else {
                redirectToLogin("Session expired. Redirecting to login...");
            }
        } catch (err) {
            redirectToLogin("An error occurred while validating the session.");
        }
    };

    // ✅ Helper function for logout and redirect
    const redirectToLogin = (message) => {
        setError(message);
        setTimeout(() => {
            localStorage.clear();
            navigate("/");
        }, 2000);
    };

    return (
        <div className="profile-container">
            <h1>Employee Profile</h1>
            {error && <p style={{ color: "red" }}>{error}</p>}
            {profile ? (
                <div className="profile-details">
                    <p><strong>Employee ID:</strong> {profile.empId}</p>
                    <p><strong>First Name:</strong> {profile.firstName}</p>
                    <p><strong>Last Name:</strong> {profile.lastName}</p>
                    <p><strong>Role:</strong> {profile.role}</p>
                    <p><strong>Department:</strong> {profile.department}</p>
                    <p><strong>Created Date:</strong> {new Date(profile.createdDate).toLocaleDateString()}</p>
                    <p><strong>Modified Date:</strong> {new Date(profile.modifiedDate).toLocaleDateString()}</p>
                    <p><strong>Modified By:</strong> {profile.modifiedBy}</p>
                    <p><strong>Created By:</strong> {profile.createdBy}</p>
                </div>
            ) : (
                <p>Loading profile...</p>
            )}
            <button id="backBtn" onClick={handleBack}>Back</button>
        </div>
    );
};

export default ProfilePage;
