import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom"; // Import useParams

const ProfilePage = () => {
    const [profile, setProfile] = useState(null);
    const [error, setError] = useState("");
    const { userId } = useParams(); // Get userId from URL
    const navigate = useNavigate();

    const token = localStorage.getItem("token");

    useEffect(() => {
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
                    // Token is valid, fetch the profile details
                    fetchProfile();
                } else {
                    // Token is invalid, redirect to login
                    setError("Session expired. Redirecting to login...");
                    setTimeout(() => {
                        localStorage.clear(); // Clear local storage
                        navigate("/"); // Redirect to login page
                    }, 2000);
                }
            } catch (err) {
                setError("An error occurred while validating the session.");
                setTimeout(() => {
                    localStorage.clear(); // Clear local storage
                    navigate("/"); // Redirect to login page
                }, 2000);
            }
        };

        const fetchProfile = async () => {
            try {
                const response = await fetch(`http://localhost:5059/api/Employee/GetEmployeeByUserId/${userId}`, {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (response.ok) {
                    const data = await response.json();
                    console.log(data);
                    setProfile(data);
                } else {
                    setError("Failed to fetch profile details.");
                }
            } catch (err) {
                setError("An error occurred while fetching profile details.");
            }
        };

        if (token && userId) {
            validateToken(); // Validate token before fetching the profile
        } else {
            setError("Invalid session. Redirecting to login...");
            setTimeout(() => {
                navigate("/"); // Redirect to login page
            }, 2000);
        }
    }, [userId, token, navigate]);

    const handleBack = () => {
        navigate("/welcome");
    };

    return (
        <div className="profile-container">
            <h1>Employee Profile</h1>
            {error && <p style={{ color: "red" }}>{error}</p>}
            {profile ? (
                <div className="profile-details">
                    <p>
                        <strong>Employee ID:</strong> <span>{profile.empId}</span>
                    </p>
                    <p>
                        <strong>First Name:</strong> <span>{profile.firstName}</span>
                    </p>
                    <p>
                        <strong>Last Name:</strong> <span>{profile.lastName}</span>
                    </p>
                    <p>
                        <strong>Role:</strong> <span>{profile.role}</span>
                    </p>
                    <p>
                        <strong>Department:</strong> <span>{profile.department}</span>
                    </p>
                    <p>
                        <strong>Created Date:</strong> <span>{new Date(profile.createdDate).toLocaleDateString()}</span>
                    </p>
                    <p>
                        <strong>Modified Date:</strong> <span>{new Date(profile.modifiedDate).toLocaleDateString()}</span>
                    </p>
                    <p>
                        <strong>Modified By:</strong> <span>{profile.modifiedBy}</span>
                    </p>
                    <p>
                        <strong>Created By:</strong> <span>{profile.createdBy}</span>
                    </p>
                </div>
            ) : (
                <p>Loading profile...</p>
            )}
            <button id="backBtn" onClick={handleBack}>
                Back
            </button>
        </div>
    );
};

export default ProfilePage;
