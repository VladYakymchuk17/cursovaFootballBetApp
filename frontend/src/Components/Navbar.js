// Navbar.js

// Navbar.js

import React from 'react';
import { Link } from 'react-router-dom';
import 'C:\\Users\\symbi\\Desktop\\kursova\\src\\Styles\\Navbar.css'; // Import CSS file for styling

function Navbar() {
  return (
    <nav className="navbar">
      <div className="navbar-logo">
       
        <img src="https://media.api-sports.io/football/leagues/78.png" alt="Bundesliga" />
      </div>
      <ul className="navbar-links">
        <li>
          <Link to="/">Home</Link>
        </li>
        <li>
          <Link to="/games">Games</Link>
        </li>
        <li>
          <Link to="/bets">Bets</Link>
        </li>
        <li>
          <Link to="/stats">Stats</Link>
        </li>
        <li>
          <Link to="/export">Export</Link>
        </li>
      </ul>
    </nav>
  );
}

export default Navbar;

