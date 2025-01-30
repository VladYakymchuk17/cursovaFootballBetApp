import React, { useEffect, useState } from 'react';
import 'C:\\Users\\symbi\\Desktop\\kursova\\src\\Styles\\Home.css'; // Import your CSS file for styling


const logos = [
    { name: 'VfL Wolfsburg', logo: 'https://media.api-sports.io/football/teams/161.png' },
    { name: 'FC Heidenheim', logo: 'https://media.api-sports.io/football/teams/180.png' },
    { name: 'Bayer Leverkusen', logo: 'https://media.api-sports.io/football/teams/168.png' },
    { name: 'RB Leipzig', logo: 'https://media.api-sports.io/football/teams/173.png' },
    { name: 'Borussia Dortmund', logo: 'https://media.api-sports.io/football/teams/165.png' },
    { name: '1.FC Köln', logo: 'https://media.api-sports.io/football/teams/192.png' },
    { name: '1899 Hoffenheim', logo: 'https://media.api-sports.io/football/teams/167.png' },
    { name: 'Werder Bremen', logo: 'https://media.api-sports.io/football/teams/162.png' },
    { name: 'Bayern Munich', logo: 'https://media.api-sports.io/football/teams/157.png' },
    { name: 'Union Berlin', logo: 'https://media.api-sports.io/football/teams/182.png' },
    { name: 'FSV Mainz 05', logo: 'https://media.api-sports.io/football/teams/164.png' },
    { name: 'Eintracht Frankfurt', logo: 'https://media.api-sports.io/football/teams/169.png' },
    { name: 'SV Darmstadt 98', logo: 'https://media.api-sports.io/football/teams/181.png' },
    { name: 'SC Freiburg', logo: 'https://media.api-sports.io/football/teams/160.png' },
    { name: 'FC Augsburg', logo: 'https://media.api-sports.io/football/teams/170.png' },
    { name: 'Borussia Monchengladbach', logo: 'https://media.api-sports.io/football/teams/163.png' },
    { name: 'VfB Stuttgart', logo: 'https://media.api-sports.io/football/teams/172.png' },
    { name: 'Vfl Bochum', logo: 'https://media.api-sports.io/football/teams/176.png' },
  ];
  
  function Home() {
    const [teams, setTeams] = useState([]);
    const [selectedTeam, setSelectedTeam] = useState(null);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);
    const [teamStats, setTeamStats] = useState([]);
    const [matches, setMatches] = useState([]);
    const [selectedSeason, setSelectedSeason] = useState("All rounds");
  
    useEffect(() => {
      const fetchTeams = async () => {
        try {
          const response = await fetch('https://localhost:7177/api/Team');
          if (!response.ok) {
            throw new Error('Failed to fetch data');
          }
          const data = await response.json();
          setTeams(data);
          setLoading(false);
        } catch (error) {
          setError(error.message);
          setLoading(false);
        }
      };
  
      fetchTeams();
    }, []);
  
    useEffect(() => {
      const fetchTeamsStats = async () => {
        try {
          const response = await fetch('https://localhost:7177/api/Team/AllStat');
          if (!response.ok) {
            throw new Error('Failed to fetch data');
          }
          const data = await response.json();
          setTeamStats(data);
        } catch (error) {
          setError(error.message);
          setLoading(false);
        }
      };
  
      fetchTeamsStats();
    }, []);
  
    useEffect(() => {
      const fetchMatches = async () => {
        try {
          const response = await fetch('https://localhost:7177/api/Fixture/Finished');
          if (!response.ok) {
            throw new Error('Failed to fetch data');
          }
          const data = await response.json();
          setMatches(data);
        } catch (error) {
          setError(error.message);
          setLoading(false);
        }
      };
  
      fetchMatches();
    }, []);
  
    const handleTeamClick = (team) => {
      setSelectedTeam(team);
    };

    const handleSeasonChange = (e) => {
      setSelectedSeason(e.target.value);
    };
  
    const handleFilter = async () => {
      const response = await fetch('https://localhost:7177/api/Team/RoundStat', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(selectedSeason)
      });
      if(response.ok){
        let newStats = await response.json();
        console.log(newStats);
        setTeamStats(newStats);
      }

      const responseMatches = await fetch('https://localhost:7177/api/Fixture/FinishedByRound', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(selectedSeason)
      });
      if(response.ok){
        let newMatches = await responseMatches.json();
       
        setMatches(newMatches);
      }
    };
  
    const setLogo = (teamName) => {
      const logo = logos.find((logo) => logo.name === teamName);
      return logo ? logo.logo : '';
    };
  
   /* const handleDownloadOLAP = async () => {
      try {
        const response = await fetch('https://localhost:7177/api/Fixture', {
          method: 'PUT'
        });
        if (!response.ok) {
          throw new Error('Failed to check files');
        }
        const result = await response.json();
        if (result) {
          const message = `${result}`;
          window.alert(message);
        } else {
          throw new Error('Incorrect response format');
        }
      } catch (error) {
        console.error('Error checking files:', error);
        window.alert('Error checking files. Please try again.');
      }
    };*/
  
    if (loading) {
      return <div>Loading...</div>;
    }
  
    if (error) {
      return <div>Error: {error}</div>;
    }
  
    return (
      <div className="home-container">
         <div className="filters-container">
        <select value={selectedSeason} onChange={handleSeasonChange}>
          {Array.from({ length: 18 }, (_, i) => (
            <option key={i} value={`Regular Season - ${i + 1}`}>{`Regular Season - ${i + 1}`}</option>
          ))}
        </select>
        <button className='filterRound-btn' onClick={handleFilter}>Filter</button>
      </div>
        <div className="teams-container">
          <h1>Teams</h1>
          <ul className="teams">
            {teams.map((team) => (
              <li key={team.id} className="team" onClick={() => handleTeamClick(team)}>
                <img src={setLogo(team.name)} alt={`${team.name} Logo`} className="team-logo" />
                <div className='team-info'>
                  <h3>{team.name}</h3>
                  <p>{team.location.city}, {team.location.country}</p>
                </div>
              </li>
            ))}
          </ul>
        </div>
        {selectedTeam && (
          <div className="team-details">
            <div className="team-stats-container">
            <h2>{selectedTeam.name} Stats</h2>
            <p>Goals: {teamStats.find(stats => stats.team_name === selectedTeam.name)?.goals}</p>
            <p>Assists: {teamStats.find(stats => stats.team_name === selectedTeam.name)?.assists}</p>
            <p>Fouls: {teamStats.find(stats => stats.team_name === selectedTeam.name)?.fouls}</p>
            <p>Substitutions: {teamStats.find(stats => stats.team_name === selectedTeam.name)?.substitutions}</p>
          </div>
            <h2>{selectedTeam.name} Players</h2>
            <table>
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Last Name</th>
                  <th>Age</th>
                  <th>Nationality</th>
                </tr>
              </thead>
              <tbody>
                {selectedTeam.players.map((player) => (
                  <tr key={player.id}>
                    <td>{player.name}</td>
                    <td>{player.last_name}</td>
                    <td>{player.age}</td>
                    <td>{player.nationality}</td>
                  </tr>
                ))}
              </tbody>
            </table>
            
          </div>
        )}
        <div className="recent-matches-container">
          <h1>Recent Matches</h1>
          <div className="recent-matches">
            {matches.map((match) => (
              <div key={match.id} className="match">
                <div className="teams">
                  <div className="team">
                    <img src={match.homeLogo} alt={match.homeName} className="team-logo" />
                    <span>{match.homeName}</span>
                  </div>
                  <div className="result">
                    <span>{match.result}</span>
                  </div>
                  <div className="team">
                    <span>{match.awayName}</span>
                    <img src={match.awayLogo} alt={match.awayName} className="team-logo" />
                  </div>
                </div>
                <div className="match-info">
                  <div>{new Date(match.date).toLocaleString()}</div>
                  <div>{match.stadium}</div>
                  <div>{match.city}</div>
                </div>
              </div>
            ))}
          </div>
        </div>
        
      </div>
    );
  }
  
  export default Home;