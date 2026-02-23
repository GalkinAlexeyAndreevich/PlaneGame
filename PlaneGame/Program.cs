using PlaneGame;
using PlaneGame.Domain;

var game = new Game();

var teamCount = ConsoleInput.ReadTeamCount();
var tactic = ConsoleInput.ReadTactic();

game.NewGame(teamCount, tactic);
