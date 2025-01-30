// App.js

import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './Components/Navbar';
import Home from './Components/Home';
import Games from './Components/Games';
import Bets from './Components/Bets';
import BetForm from './Components/BetForm';
import Stats from './Components/Stats';
import Export from './Components/Export';




const App = () => {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home/>} />
        <Route path="/games" element={<Games/>} />
        <Route path="/bets" element={<Bets/>} />
        <Route path="/betting/:id" element={<BetForm/>}/>
        <Route path='/stats' element={<Stats/>}/>
        <Route path='/export' element = {<Export/>}/>
      </Routes>
    </Router>
  );
};

export default App;
