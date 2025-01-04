import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './LoginPage.jsx';
import WelcomePage from './Welcome.jsx';
import NewUser from './NewUser.jsx';
import ProfilePage from './ProfilePage.jsx';

const App = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<LoginPage />} />
                <Route path="/welcome" element={<WelcomePage />} />
                <Route path="/new-user" element={<NewUser />} />
                <Route path="/GetEmployeeByUserId/:userId" element={<ProfilePage />} />
            </Routes>
        </Router>
    );
};

export default App;
