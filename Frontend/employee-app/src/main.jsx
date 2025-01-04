import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import './css/LoginPage.css'
import App from './App.jsx'
import './css/Welcome.css'
import './css/NewUser.css'
import './css/ProfilePage.css'

createRoot(document.getElementById('root')).render(
    <App />
)
