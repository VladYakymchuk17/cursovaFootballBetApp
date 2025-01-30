import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import 'C:\\Users\\symbi\\Desktop\\kursova\\src\\Styles\\BetForm.css';

function BetForm() {
  const [game, setGame] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const { id } = useParams();
  const [bets, setBets] = useState([]);
  const [showAddBetForm, setShowAddBetForm] = useState(false); 
  const [selectedOption, setSelectedOption] = useState('Home Win'); 
  const [homeGoals, setHomeGoals] = useState(0); 
  const [awayGoals, setAwayGoals] = useState(0); 
  const [betAmount, setBetAmount] = useState(''); 
  const [numberOfBets, setNumberOfBets] = useState(1);

  useEffect(() => {
    const fetchMatch = async () => {
      try {
        const response = await fetch(`https://localhost:7177/api/Fixture/${id}`);
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const data = await response.json();
        setGame(data);
        setLoading(false);
      } catch (error) {
        setError(error.message);
        setLoading(false);
      }
    };

    fetchMatch();
  }, [id]);

  useEffect( () => {
    const fetchBets = async ()=>{
    let bets =[]
    try {
      const response = await fetch(`https://localhost:7177/api/Bet/${id}`);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const data = await response.json();
      bets = data;
      
      setLoading(false);
    } catch (error) {
      setError(error.message);
      setLoading(false);
    }
    let betOptions =[];
    for(let i =0;i<bets.length; i++ ){
      let betName = bets[i].name;
      let exist = false;
      
      for(let j =0;j<betOptions.length;j++){
        if(betOptions[j].name.toLowerCase() === betName.toLowerCase()){
          
          exist = true;
          break;
        }
      }
      if(exist){
        continue;
      }
      let betCoefficient = generateCoefficient()
      if(bets[i].coefficient>1){
        betCoefficient = bets[i].coefficient;
      }
      
      var betOption = {
        name: betName, coefficient: betCoefficient, numberOfBets:1, income:0
      }
      betOptions.push(betOption);

    }

    /*const betOptions = [
      { name: 'Home win: 2-0', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Draw: 0-0', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Home win: 1-0', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Away win: 0-1', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Draw: 1-1', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Away win: 0-2', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Home win: 2-1', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Away win: 1-2', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Draw: 2-2', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Home win: 3-0', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Away win: 0-3', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Draw: 3-3', coefficient: generateCoefficient(), numberOfBets:1, income:0 },
      { name: 'Home win: 3-2', coefficient: generateCoefficient(),numberOfBets:1, income:0 },
      { name: 'Away win: 2-3', coefficient: generateCoefficient(), numberOfBets:1, income:0 }
    ];*/

    setBets(betOptions);
    };
    fetchBets();
  }, [id]);

  const handleBetSubmit = async (e, betOption, index) => {
    e.preventDefault();
    
    const url = `https://localhost:7177/api/Bet/${game.id}`;
    if(betOption.coefficient <0 || betOption.numberOfBets<0){
      window.alert("Negative number, check fields");
      return;
    }
    const data = {
      name: betOption.name,
      coefficient: betOption.coefficient,
      numberOfBets: betOption.numberOfBets,
      income: betOption.income
    };
  
    try {
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      });
  
      if (!response.ok) {
        throw new Error('Failed to update bet');
      }
  
      const updatedBet = await response.json();
      console.log('Bet updated successfully:', updatedBet);
     
    } catch (error) {
      console.error('Error updating bet:', error.message);
      
    }
  };
  
  const generateCoefficient = () => {
    return (Math.random() * (3 - 1) + 1).toFixed(2);
  };

  const generateCoefficientForAdd = () => {
    return (Math.random() * (3 - 1) + 3).toFixed(2);
  };

  function setAmount(index, value){
    bets[index].income = value;
  }

  function setNumber(index, value){
    bets[index].numberOfBets = value;
  }

   const handleAddBetClick = () => {
    setShowAddBetForm(true); 
  };

  const handleAddBetSubmit = async (e) => {
    e.preventDefault();
    let resName = `${selectedOption}: ${homeGoals}-${awayGoals}`;
    for(let i=0;i< bets.length; i++){
        if(bets[i].name === resName){
          window.alert("The result exists for this game");
          setShowAddBetForm(false);
          return;
        }
    }
    if(numberOfBets<0|| betAmount<0){
      window.alert("Negative numbers");
      setShowAddBetForm(false)
      return;
    }
    try {
      const response = await fetch('https://localhost:7177/api/Bet/AddNew', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          gameId: game.id,
          name: `${selectedOption}: ${homeGoals}-${awayGoals}`,
          coefficient: generateCoefficientForAdd(),
          numberOfBets: numberOfBets,
          income: betAmount
        })
      });

      if (!response.ok) {
        throw new Error('Failed to filter matches');
      }else{
        window.alert("Bet added successfully")
      }

      const bets = await response.json();
      console.log(bets);
    } catch (error) {
      setError(error.message);
    }


   
    setShowAddBetForm(false);
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div className="bet-form-container">
      {showAddBetForm ? (
        <div className="add-bet-form">
           <h2>Add Your Own Bet</h2>
           <form onSubmit={handleAddBetSubmit} className="add-bet-form">
                  <div className="form-group">
                    <label htmlFor="option">Select Option:</label>
                    <select id="option" value={selectedOption} onChange={(e) => setSelectedOption(e.target.value)}>
                      <option value="Home win">Home Win</option>
                      <option value="Away win">Away Win</option>
                      <option value="Draw">Draw</option>
                    </select>
                  </div>
                  <div className="form-group">
                    <label htmlFor="home-goals">Home Goals:</label>
                    <input type="number" id="home-goals" value={homeGoals} onChange={(e) => setHomeGoals(e.target.value)} />
                  </div>
                  <div className="form-group">
                    <label htmlFor="away-goals">Away Goals:</label>
                    <input type="number" id="away-goals" value={awayGoals} onChange={(e) => setAwayGoals(e.target.value)} />
                  </div>
                  <div className="form-group">
                    <label htmlFor="bet-amount">Bet Amount:</label>
                    <input type="number" id="bet-amount" value={betAmount} onChange={(e) => setBetAmount(e.target.value)} />
                  </div>
                  <div className="form-group">
                    <label htmlFor="number-of-bets">Number of Bets:</label>
                    <input type="number" id="number-of-bets" value={numberOfBets} onChange={(e) => setNumberOfBets(e.target.value)} />
                  </div>
                  <div className="form-buttons">
                    <button type="submit" className="btn-submit">Add Bet</button>
                    <button type="button" className="btn-cancel" onClick={() => setShowAddBetForm(false)}>Cancel</button>
                  </div>
                </form>
        </div>
      ) : (
        <div className="bet-options-container">
             {bets.map((betOption, index) => (
          <div key={index} className="bet-option">
            <div className="match-details">
            <h2 className="form-title">Place Your Bets</h2>
            <h3>{game.homeName} vs {game.awayName}</h3>
            <p>Date: {new Date(game.date).toLocaleString()}</p>
          </div>
            <h3>{betOption.name}</h3>
            <form className="bet-form" onSubmit={(e) => handleBetSubmit(e, betOption)}>
              <div className="form-group">
                <label htmlFor={`coefficient-${index}`}>Coefficient:</label>
                <input type="text" id={`coefficient-${index}`} name={`coefficient-${index}`} value={betOption.coefficient} readOnly />
              </div>
              <div className="form-group">
                <label htmlFor={`number-of-bets-${index}`}>Number of Bets:</label>
                <input type="number" id={`number-of-bets-${index}`} name={`number-of-bets-${index}`} defaultValue={1} onChange={(e) => setNumber(index, e.target.value)} />
              </div>
              <div className="form-group">
                <label htmlFor={`bet-amount-${index}`}>Bet Amount:</label>
                <input type="number" id={`bet-amount-${index}`} name={`bet-amount-${index}`} onChange={(e) => setAmount(index, e.target.value)} />
              </div>
              <button type="submit" className="btn-submit">Place Bet</button>
            </form>
          </div>
        ))}
          
        </div>
      )}
      {showAddBetForm === false && (
        <>
          <div className="button-container">
          <button className='addBet-btn' onClick={handleAddBetClick}>Add Own Bet</button>
          </div>
        </>
      )}
    </div>
  );
}

export default BetForm;
