import React, { useState } from 'react';
import 'C:\\Users\\symbi\\Desktop\\kursova\\src\\Styles\\Export.css';

function Export() {
  const [selectedFact, setSelectedFact] = useState('');
  const [selectedColumns, setSelectedColumns] = useState([]);
  const [uploadSuccess, setUploadSuccess] = useState(false);
  const [limit, setLimit] = useState(100);
  const [selectedType, setSelectedType] = useState('csv');


  const handleFactChange = (event) => {
    setSelectedFact(event.target.value);
    setSelectedColumns([]);
  };

  const handleColumnChange = (event) => {
    const columnName = event.target.value;
    if (event.target.checked) {
      setSelectedColumns([...selectedColumns, columnName]);
    } else {
      setSelectedColumns(selectedColumns.filter(col => col !== columnName));
    }
  };
  const handleLimitChange = (event) => {
    const value = event.target.value;
    if (!isNaN(value)) {
      setLimit(parseInt(value));
    }
  };

  const handleTypeChange = (event) => {
    setSelectedType(event.target.value);
  };

  const handleDownloadOLAP = async () => {
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
  };

  const goToReports = async () => {
    window.location.href = 'https://app.powerbi.com/groups/me/lineage?experience=power-bi';
  };

  const exportData = async () => {
    try {
      const requestBody = {
        fact: selectedFact,
        columns: selectedColumns,
        type: selectedType,
        limit: limit
      };

      console.log(requestBody);

      const response = await fetch('https://localhost:7177/api/Stats', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestBody)
      });

      if (response.ok) {
        setUploadSuccess(true);
        setSelectedFact('');
        setSelectedColumns([]);
        if(requestBody.type ==='json'){
        const message = 'File has been exported to JONFiles directory';
        window.alert(message);
        }
        else if(requestBody.type === 'csv'){
          const message = 'File has been exported to CSVFiles directory';
          window.alert(message);
        }
      } else {
        console.error('Failed to upload data:', response.statusText);
      }
    } catch (error) {
      console.error('Error:', error.message);
    }
  };


  return (
    
    <div className="export-container">
      <div className="buttons-container">
          <button className="fillOLAP-btn" onClick={handleDownloadOLAP}>Fill OLAP</button>
          <button className="goToReports-btn" onClick={goToReports}>Go to reports</button>
        </div>
  <h2>Select Fact to Export:</h2>
  <select className="select-container" value={selectedFact} onChange={handleFactChange}>
    <option value="">Select Fact</option>
    <option value="Game">Game</option>
    <option value="Player">Player</option>
    <option value="Bet">Bet</option>
  </select>

  {selectedFact && (
    <>
      <h2>Select Columns to Export:</h2>
      
     
      <div className="checkbox-container">
        <label>
          <input
            type="checkbox"
            value="Date"
            checked={selectedColumns.includes('Date')}
            onChange={handleColumnChange}
          />
          Date
        </label>

        <div className="sub-checkbox-container">
          <label>
            <input
              type="checkbox"
              value="d.month"
              checked={selectedColumns.includes('d.month')}
              onChange={handleColumnChange}
              disabled={!selectedColumns.includes('Date')}
            />
            Month
          </label>
          <br />
          <label>
            <input
              type="checkbox"
              value="d.day"
              checked={selectedColumns.includes('d.day')}
              onChange={handleColumnChange}
              disabled={!selectedColumns.includes('Date')}
            />
            Day
          </label>
          <br />
          <label>
            <input
              type="checkbox"
              value="d.day_of_week"
              checked={selectedColumns.includes('d.day_of_week')}
              onChange={handleColumnChange}
              disabled={!selectedColumns.includes('Date')}
            />
            Day of Week
          </label>
          <br />
          <label>
            <input
              type="checkbox"
              value="d.year"
              checked={selectedColumns.includes('d.year')}
              onChange={handleColumnChange}
              disabled={!selectedColumns.includes('Date')}
            />
            Year
          </label>
        </div>
      </div>

      
      {selectedFact && (
        <>
          <div className="checkbox-container">
            
           
            <label>
              <input
                type="checkbox"
                value="Game"
                checked={selectedColumns.includes('Game')}
                onChange={handleColumnChange}
              />
              Game
            </label>
           
            <div className="sub-checkbox-container">
              
              <label>
                <input
                  type="checkbox"
                  value="League"
                  checked={selectedColumns.includes('League')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Game')}
                />
                League
              </label>
              <div className="sub-checkbox-container">
                <label>
                  <input
                    type="checkbox"
                    value="l.name"
                    checked={selectedColumns.includes('l.name')}
                    onChange={handleColumnChange}
                    disabled={!selectedColumns.includes('League') || !selectedColumns.includes('Game')}
                  />
                  Name
                </label>
                <br />
                <label>
                  <input
                    type="checkbox"
                    value="l.season"
                    checked={selectedColumns.includes('l.season')}
                    onChange={handleColumnChange}
                    disabled={!selectedColumns.includes('League') || !selectedColumns.includes('Game')}
                  />
                  Season
                </label>
                <br />
                <label>
                  <input
                    type="checkbox"
                    value="l.round"
                    checked={selectedColumns.includes('l.round')}
                    onChange={handleColumnChange}
                    disabled={!selectedColumns.includes('League') || !selectedColumns.includes('Game')}
                  />
                  Round
                </label>
              </div>
              
              <label>
                <input
                  type="checkbox"
                  value="Venue"
                  checked={selectedColumns.includes('Venue')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Game')}
                />
                Venue
              </label>
              <div className="sub-checkbox-container">
                <label>
                  <input
                    type="checkbox"
                    value="v.name"
                    checked={selectedColumns.includes('v.name')}
                    onChange={handleColumnChange}
                    disabled={!selectedColumns.includes('Venue') || !selectedColumns.includes('Game')}
                  />
                  Name
                </label>
                <br />
                <label>
                  <input
                    type="checkbox"
                    value="v.city"
                    checked={selectedColumns.includes('v.city')}
                    onChange={handleColumnChange}
                    disabled={!selectedColumns.includes('Venue') || !selectedColumns.includes('Game')}
                  />
                  City
                </label>
              </div>
              
              <label>
                <input
                  type="checkbox"
                  value="th.name"
                  checked={selectedColumns.includes('th.name')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Game')}
                />
                Home Team Name
              </label>
              <br/>
              <label>
                <input
                  type="checkbox"
                  value="ta.name"
                  checked={selectedColumns.includes('ta.name')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Game')}
                />
                Away Team Name
              </label>
            </div>
          </div>
        </>
      )}

      {selectedFact === 'Bet' && (
        <>
          <div className="checkbox-container">
            
            <label>
              <input
                type="checkbox"
                value="Result"
                checked={selectedColumns.includes('Result')}
                onChange={handleColumnChange}
              />
              Result
            </label>
           
            <div className="sub-checkbox-container">
              <label>
                <input
                  type="checkbox"
                  value="r.name"
                  checked={selectedColumns.includes('r.name')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Result')}
                />
                Name
              </label>
              <br/>
              <label>
                <input
                  type="checkbox"
                  value="r.score"
                  checked={selectedColumns.includes('r.score')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Result')}
                />
                Score
              </label>
            </div>
            <label>
                <input
                  type="checkbox"
                  value="br.number_of_bets"
                  checked={selectedColumns.includes('br.number_of_bets')}
                  onChange={handleColumnChange}
                />
                Number of bets
              </label>
              <br/>

              <label>
                <input
                  type="checkbox"
                  value="br.income"
                  checked={selectedColumns.includes('br.income')}
                  onChange={handleColumnChange}
                />
                Income
              </label>
              <br/>

              <label>
                <input
                  type="checkbox"
                  value="br.outcome"
                  checked={selectedColumns.includes('br.outcome')}
                  onChange={handleColumnChange}
                />
                Outcome
              </label>
              <br/>

              <label>
                <input
                  type="checkbox"
                  value="br.bet_result"
                  checked={selectedColumns.includes('br.bet_result')}
                  onChange={handleColumnChange}
                />
                Bet_result
              </label>
              <br/>


              <label>
                <input
                  type="checkbox"
                  value="br.coef"
                  checked={selectedColumns.includes('br.coef')}
                  onChange={handleColumnChange}
                />
                Coef
              </label>
              <br/>
          </div>
        </>
      )}

      {selectedFact === 'Game' && (
        <>
          <div className="checkbox-container">
            
            <label>
              <input
                type="checkbox"
                value="Team"
                checked={selectedColumns.includes('Team')}
                onChange={handleColumnChange}
              />
              Team
            </label>
            
            <div className="sub-checkbox-container">
              
              <label>
                <input
                  type="checkbox"
                  value="t.name"
                  checked={selectedColumns.includes('t.name')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Team')}
                />
                Name
              </label>
              <br/>
              <label>
                <input
                  type="checkbox"
                  value="Location"
                  checked={selectedColumns.includes('Location')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Team')}
                />
                Location
              </label>
              <div className="sub-checkbox-container">
                <label>
                  <input
                    type="checkbox"
                    value="l.city"
                    checked={selectedColumns.includes('l.city')}
                    onChange={handleColumnChange}
                    disabled={!selectedColumns.includes('Location')}
                  />
                  City
                </label>
                <br/>

                <label>
                  <input
                    type="checkbox"
                    value="l.state"
                    checked={selectedColumns.includes('l.state')}
                    onChange={handleColumnChange}
                    disabled={!selectedColumns.includes('Location')}
                  />
                  State
                </label>
                <br/>

                <label>
                  <input
                    type="checkbox"
                    value="l.country"
                    checked={selectedColumns.includes('l.country')}
                    onChange={handleColumnChange}
                    disabled={!selectedColumns.includes('Location')}
                  />
                  Country
                </label>
                <br/>
              </div>
            </div>

            <label>
              <input
                type="checkbox"
                value="Period"
                checked={selectedColumns.includes('Period')}
                onChange={handleColumnChange}
              />
              Period
            </label>
            <div className="sub-checkbox-container">
              <label>
                <input
                  type="checkbox"
                  value="p.period_name"
                  checked={selectedColumns.includes('p.period_name')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Period')}
                />
                Name
              </label>
              <br/>
            </div>

            <label>
              <input
                type="checkbox"
                value="gr.goals"
                checked={selectedColumns.includes('gr.goals')}
                onChange={handleColumnChange}
              />
              Goals
            </label>
            <br/>

            <label>
              <input
                type="checkbox"
                value="gr.assists"
                checked={selectedColumns.includes('gr.assists')}
                onChange={handleColumnChange}
              />
              Assists
            </label>
            <br/>

            <label>
              <input
                type="checkbox"
                value="gr.fouls"
                checked={selectedColumns.includes('gr.fouls')}
                onChange={handleColumnChange}
              />
              Fouls
            </label>
            <br/>

            <label>
              <input
                type="checkbox"
                value="gr.red_cards"
                checked={selectedColumns.includes('gr.red_cards')}
                onChange={handleColumnChange}
              />
              Red cards
            </label>
            <br/>

            <label>
              <input
                type="checkbox"
                value="gr.yellow_cards"
                checked={selectedColumns.includes('gr.yellow_cards')}
                onChange={handleColumnChange}
              />
              Yellow cards
            </label>
            <br/>

            <label>
              <input
                type="checkbox"
                value="gr.substitutions"
                checked={selectedColumns.includes('gr.substitutions')}
                onChange={handleColumnChange}
              />
              Substitutions
            </label>
            <br/>
          </div>
        </>
      )}

      {selectedFact === 'Player' && (
        <>
          <div className="checkbox-container">
            <label>
              <input
                type="checkbox"
                value="Player"
                checked={selectedColumns.includes('Player')}
                onChange={handleColumnChange}
              />
              Player
            </label>
            
            <div className="sub-checkbox-container">
              <label>
                <input
                  type="checkbox"
                  value="pl.first_name"
                  checked={selectedColumns.includes('pl.first_name')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Player')}
                />
                First Name
              </label>
              <br/>
              <label>
                <input
                  type="checkbox"
                  value="pl.last_name"
                  checked={selectedColumns.includes('pl.last_name')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Player')}
                />
                Last Name
              </label>
              <br/>

              <label>
                <input
                  type="checkbox"
                  value="pl.age"
                  checked={selectedColumns.includes('pl.age')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Player')}
                />
                Age
              </label>
              <br/>

              <label>
                <input
                  type="checkbox"
                  value="pl.nationality"
                  checked={selectedColumns.includes('pl.nationality')}
                  onChange={handleColumnChange}
                  disabled={!selectedColumns.includes('Player')}
                />
                Nationality
              </label>
              <br/>
            </div>

            <label>
              <input
                type="checkbox"
                value="pr.goals"
                checked={selectedColumns.includes('pr.goals')}
                onChange={handleColumnChange}
              />
              Goals
            </label>
            <br/>

            <label>
              <input
                type="checkbox"
                value="pr.assists"
                checked={selectedColumns.includes('pr.assists')}
                onChange={handleColumnChange}
              />
              Assists
            </label>
            <br/>

            <label>
              <input
                type="checkbox"
                value="pr.fouls"
                checked={selectedColumns.includes('pr.fouls')}
                onChange={handleColumnChange}
              />
              Fouls
            </label>
            <br/>

            <label>
              <input
                type="checkbox"
                value="pr.time"
                checked={selectedColumns.includes('pr.time')}
                onChange={handleColumnChange}
              />
              Time
            </label>
            <br/>
          </div>
        </>
      )}

      <div className="input-container">
        <label htmlFor="limit">Limit:</label>
        <input 
          type="number" 
          id="limit" 
          name="limit" 
          value={limit} 
          onChange={handleLimitChange} 
          min="1" 
          step="1"
        />
      </div>
      <div className="select-container">
        <label htmlFor="type">Export Type:</label>
        <select id="type" name="type" value={selectedType} onChange={handleTypeChange}>
          <option value="csv">CSV</option>
          <option value="json">JSON</option>
        </select>
      </div>

     
      <button className="export-button" onClick={exportData}>Export Data</button>
      
    </>
  )}
   {uploadSuccess && (
    <div>
      <p>File has been uploaded successfully!</p>
    </div>
  )}
</div>

    
  );
}

export default Export;
