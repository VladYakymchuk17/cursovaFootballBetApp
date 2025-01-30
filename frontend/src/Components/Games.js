import React, { useEffect, useState } from 'react';
import 'C:\\Users\\symbi\\Desktop\\kursova\\src\\Styles\\Games.css'; // Import your CSS file for styling
//import BetForm from './BetForm'; // Adjust the import path based on your project structure
import { Link } from 'react-router-dom';

const teamNames = [
  'VfL Wolfsburg', 'FC Heidenheim', 'Bayer Leverkusen', 'RB Leipzig', 'Borussia Dortmund',
  '1.FC Köln', '1899 Hoffenheim', 'Werder Bremen', 'Bayern Munich', 'Union Berlin',
  'FSV Mainz 05', 'Eintracht Frankfurt', 'SV Darmstadt 98', 'SC Freiburg', 'FC Augsburg',
  'Borussia Monchengladbach', 'VfB Stuttgart', 'Vfl Bochum'
];

function Games() {
  const [matches, setMatches] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [selectedTeam, setSelectedTeam] = useState('');
  const [selectedMonth, setSelectedMonth] = useState('0');

  useEffect(() => {
    const fetchMatches = async () => {
      try {
        const response = await fetch('https://localhost:7177/api/Fixture');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const data = await response.json();
        setMatches(data);
        setLoading(false);
      } catch (error) {
        setError(error.message);
        setLoading(false);
      }
    };

    fetchMatches();
  }, []);

  const handleFilter = async () => {

    console.log(JSON.stringify({
      month: selectedMonth,
      team_name: selectedTeam
    }))
    try {
      const response = await fetch('https://localhost:7177/api/Fixture/Filtered', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          month: selectedMonth,
          team_name: selectedTeam
        })
      });

      if (!response.ok) {
        throw new Error('Failed to filter matches');
      }

      const filteredMatches = await response.json();
      setMatches(filteredMatches);
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
    <div className="matches-container">
      <h2>Upcoming Matches</h2>
      <div className="filter-container">
        <select value={selectedTeam} onChange={(e) => setSelectedTeam(e.target.value)}>
          <option value="">All Teams</option>
          {teamNames.map((team, index) => (
            <option key={index} value={team}>{team}</option>
          ))}
        </select>
        <select value={selectedMonth} onChange={(e) => setSelectedMonth(e.target.value)}>
          <option value="0">All Months</option>
          {[...Array(2)].map((_, index) => {
            const monthNumber = new Date().getMonth() + 1 + index; 
            return (
              <option key={index} value={monthNumber}>{monthNumber}</option>
            );
          })}
        </select>
        <button className="filter-btn" onClick={handleFilter}>Filter</button>
      </div>
      <div className="matches-grid">
        {matches.map((match) => (
          <Link key={match.id} to={`/betting/${match.id}`} className="match-card">
            <div className="team">
              <img src={match.homeLogo} alt={match.homeName} />
              <span>{match.homeName}</span>
            </div>
            <div className="versus">-:-</div>
            <div className="team">
              <span>{match.awayName}</span>
              <img src={match.awayLogo} alt={match.awayName} />
            </div>
            <div className="match-info">
              <div className="info-item date">{new Date(match.date).toLocaleString()}</div>
              <div className="info-item">{match.season}</div>
              <div className="info-item">{match.stadium}</div>
              <div className="info-item">{match.city}</div>
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
}

export default Games;