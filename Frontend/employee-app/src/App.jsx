import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './LoginPage.jsx';
import WelcomePage from './Welcome.jsx';

const App = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<LoginPage />} />
                <Route path="/welcome" element={<WelcomePage />} />
            </Routes>
        </Router>
    );
};

export default App;
