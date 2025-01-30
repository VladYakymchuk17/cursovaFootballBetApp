// Fixture.js

const mongoose = require('mongoose');

const fixtureSchema = new mongoose.Schema({
  fixture: {
    id: Number,
    referee: String,
    timezone: String,
    date: Date,
    timestamp: Number,
  },
  periods: {
    first: String,
    second: String,
  },
  venue: {
    id: Number,
    name: String,
    city: String,
  },
  status: {
    long: String,
    short: String,
    elapsed: Number,
  },
  league: {
    id: Number,
    name: String,
    country: String,
    logo: String,
    flag: String,
    season: Number,
    round: String,
  },
  teams: {
    home: {
      id: Number,
      name: String,
      logo: String,
      winner: String,
    },
    away: {
      id: Number,
      name: String,
      logo: String,
      winner: String,
    },
  },
  goals: {
    home: Number,
    away: Number,
  },
  score: {
    halftime: {
      home: Number,
      away: Number,
    },
    fulltime: {
      home: Number,
      away: Number,
    },
    extratime: {
      home: Number,
      away: Number,
    },
    penalty: {
      home: Number,
      away: Number,
    },
  },
});

const Fixture = mongoose.model('Fixture', fixtureSchema, 'Fixtures');

module.exports = Fixture;
