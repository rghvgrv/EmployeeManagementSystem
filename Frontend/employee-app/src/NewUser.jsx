import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'; // Import useNavigate for redirection

const NewUserForm = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [empId, setEmpId] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const navigate = useNavigate(); // Initialize navigate function

    const handleSave = async (e) => {
        e.preventDefault();

        const newUser = {
            username: username,
            password: password,
            empId: empId,
        };

        try {
            const response = await fetch('http://localhost:5059/AddUser', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(newUser),
            });

            const data = await response.json();

            if (response.ok) {
                setSuccess('User added successfully!');
                setError('');
                setUsername('');
                setPassword('');
                setEmpId('');
            } else {
                setError(data.message || 'Failed to add user');
                setSuccess('');
            }
        } catch (err) {
            setError('An error occurred while adding the user');
            setSuccess('');
        }
    };

    const handleCancel = () => {
        // Navigate to the home page when cancel is clicked
        navigate('/');
    };

    return (
        <div className="form-container">
            <h1>Create New User</h1>
            <form onSubmit={handleSave} className="user-form">
                <div>
                    <label>Username:</label>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                        autoComplete="new-username" // Disable auto-fill
                    />
                </div>
                <div>
                    <label>Password:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                        autoComplete="new-password" // Disable auto-fill
                    />
                </div>
                <div>
                    <label>Employee ID:</label>
                    <input
                        type="text"
                        value={empId}
                        onChange={(e) => setEmpId(e.target.value)}
                        required
                        autoComplete="off" // Disable auto-fill
                    />
                </div>
                <button type="submit">Save</button>
                <button type="button" onClick={handleCancel} style={{ backgroundColor: '#f44336', color: '#fff' }}>
                    Cancel
                </button>
            </form>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {success && <p style={{ color: 'green' }}>{success}</p>}
        </div>
    );
};

export default NewUserForm;
