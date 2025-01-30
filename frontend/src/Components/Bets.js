import React, { useState, useEffect } from 'react';
import 'C:\\Users\\symbi\\Desktop\\kursova\\src\\Styles\\Bets.css'


const teamNames = [
  'VfL Wolfsburg', 'FC Heidenheim', 'Bayer Leverkusen', 'RB Leipzig', 'Borussia Dortmund',
  '1.FC Köln', '1899 Hoffenheim', 'Werder Bremen', 'Bayern Munich', 'Union Berlin',
  'FSV Mainz 05', 'Eintracht Frankfurt', 'SV Darmstadt 98', 'SC Freiburg', 'FC Augsburg',
  'Borussia Monchengladbach', 'VfB Stuttgart', 'Vfl Bochum'
];

function Bets() {
  //const [bets, setBets] = useState([]);
  const [filteredBets, setFilteredBets] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [selectedTeam, setSelectedTeam] = useState('');
  const [selectedMonth, setSelectedMonth] = useState('0');
  const [selectedCoefficient, setSelectedCoefficient] = useState('');

  useEffect(() => {
    const fetchBets = async () => {
      try {
        const response = await fetch('https://localhost:7177/api/Bet');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const data = await response.json();

        
        //setBets(data);
        setFilteredBets(data); 
        setLoading(false);
      } catch (error) {
        setError(error.message);
        setLoading(false);
      }
    };

    fetchBets();
  }, []);

  /*useEffect(() => {
    
    filterBets();
  }, [selectedTeam, selectedMonth, selectedCoefficient]);*/

  const filterBets = async () => {
    var requestBody = {
      team_name: selectedTeam,
      coef: selectedCoefficient,
      month: selectedMonth
    }
   
    console.log(requestBody);
    try {
      const response = await fetch('https://localhost:7177/api/Bet/Filtered', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          team_name: selectedTeam,
          coef: selectedCoefficient,
          month: selectedMonth
        })
      });

      if (!response.ok) {
        throw new Error('Failed to filter bets');
      }

      const filteredBets = await response.json();
      
      setFilteredBets(filteredBets);
    } catch (error) {
      setError(error.message);
    }
    
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div className="bets-container">
      <h1>Placed Bets</h1>
      <div className="filter-container">
        <select value={selectedTeam} onChange={(e) => setSelectedTeam(e.target.value)}>
          <option value="">All Teams</option>
          {teamNames.map((team, index) => (
            <option key={index} value={team}>{team}</option>
          ))}
        </select>
        <select value={selectedMonth} onChange={(e) => setSelectedMonth(e.target.value)}>
          <option value="0">All Months</option>
          {[...Array(12)].map((_, index) => {
            const monthNumber = index + 1;
            return (
              <option key={index} value={monthNumber}>{monthNumber}</option>
            );
          })}
        </select>
        <select value={selectedCoefficient} onChange={(e) => setSelectedCoefficient(e.target.value)}>
          <option value="">All Coefficients</option>
          <option value="moreThanTwo">More than 2</option>
          <option value="lessThanTwo">Less than 2</option>
        </select>
        <button className="filter-btn" onClick={filterBets}>Filter</button>
      </div>
      <div className="bets-grid">
        {filteredBets.map((bet) => (
          <div key={bet.fixtureData.id} className="bet-card">
            <div className="team">
              <img src={bet.fixtureData.homeLogo} alt={bet.fixtureData.homeName} />
              <span>{bet.fixtureData.homeName}</span>
            </div>
            <div className="versus">-:-</div>
            <div className="team">
              <span>{bet.fixtureData.awayName}</span>
              <img src={bet.fixtureData.awayLogo} alt={bet.fixtureData.awayName} />
            </div>
            <div className="bet-info">
              {bet.betsOptions.map((option, index) => (
                <div key={index} className="option">
                  <div>{option.name}</div>
                  <div>Coefficient: {option.coefficient}</div>
                  <div>Number of Bets: {option.numberOfBets}</div>
                  <div>Income: {option.income}</div>
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default Bets;