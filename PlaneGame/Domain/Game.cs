using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain;

public class Game()
{
    private readonly List<Team> _teams = [];
    private const int CountInTeam = 3;
    private void AddTeam(TacticType tactic)
    {
        List<Plane> planes = [];
        for (var i = 0; i < CountInTeam; i++)
        {
            var plane = GameData.Planes[i].Clone();
            plane.EquipRandom(GameData.Weapons,  GameData.Armors, GameData.Ammunition);
            planes.Add(plane);
        }

        var newTeam = new Team(_teams.Count + 1);
        newTeam.AddPlanesToTeam(planes.ToArray());
        newTeam.SetTactic(tactic);

        _teams.Add(newTeam);
    }
    
    private int IsAliveCount => _teams.Count(t => t.IsAlive);
    
    public void NewGame(int countTeam, TacticType tactic)
    {
        if (countTeam < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(countTeam), "Количество команд должно быть не меньше 2.");
        }

        var maxSteps = countTeam * 50;

        for (var i = 0; i < countTeam; i++)
        {
            AddTeam(tactic);
        }
        
        var currentStep = 0;
        while (IsAliveCount > 1 && currentStep < maxSteps)
        {
            foreach (var team in _teams)
            {
                var enemyPlanes = _teams
                    .Where(t => t.TeamId != team.TeamId && team.IsAlive)
                    .SelectMany(t => t.Planes)
                    .ToArray();
                if(enemyPlanes.Length == 0) continue;
                team.DoDamage(enemyPlanes);
            }
            
            // Считаем у всех самолетов отложенный урон после хода
            foreach (var team in _teams)
            {
                foreach (var plane in team.Planes)
                {
                    plane.ApplyTurnDamage();
                }     
            }

            currentStep++;
        }

        if (IsAliveCount > 1)
        {
            Console.WriteLine($"Игра закончилась ничьей после {maxSteps} ходов");
            return;
        }

        if (IsAliveCount == 0)
        {
            Console.WriteLine($"Игра закончилась ничьей уничтожением всех команд");
            return;
        }

        var lastTeam = _teams.First(t => t.IsAlive);
        
        Console.WriteLine($"Победила команда {lastTeam.TeamId}");
    }
}
