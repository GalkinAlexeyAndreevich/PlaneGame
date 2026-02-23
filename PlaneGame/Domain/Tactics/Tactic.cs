using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Tactics;

public class Tactic(TacticType tactic)
{
   private TacticType TacticType { get; set; } = tactic;
   private Plane? _commanderTarget;
   private Plane? _concentrationTarget;
   
   public void SetTactic(TacticType tactic)
   {
      TacticType = tactic;
      _commanderTarget = null;
      _concentrationTarget = null;
   }

   public void BeginTurn(Plane[] enemyPlanes)
   {
      if (TacticType == TacticType.CommanderOrder)
      {
         _commanderTarget = GetRandomAlive(enemyPlanes);
      }

      if (TacticType == TacticType.Concentration && (_concentrationTarget == null || !_concentrationTarget.IsAlive))
      {
         _concentrationTarget = GetRandomAlive(enemyPlanes);
      }
   }

   public Plane GetTargetPlane(Plane plane, Plane[] enemyPlanes)
   {
      var aliveEnemies = enemyPlanes.Where(p => p.IsAlive).ToArray();
      if (aliveEnemies.Length == 0)
      {
         throw new InvalidOperationException("Не осталось живых врагов");
      }

      switch (TacticType)
      {
         // все атакуют цель, которую указал командир команды (меняется каждый ход)
         case TacticType.CommanderOrder:
            _commanderTarget = GetRandomAlive(aliveEnemies);
            if (_commanderTarget == null || !_commanderTarget.IsAlive)
            {
               throw new InvalidOperationException("Не осталось живых врагов");
            }
            return _commanderTarget;
         // атакуют самый сильный (с максимальным HP) танк
         case TacticType.HuntOnLeader:
            return aliveEnemies
               .OrderByDescending(p => p.Hp)
               .ThenByDescending(p => p.MaxHp)
               .First();
         // атакуют самый слабый (с минимальным HP) танк
         case TacticType.Finishing:
            return aliveEnemies
               .OrderBy(p => p.Hp)
               .ThenBy(p => p.MaxHp)
               .First();
         // все атакуют одного противника, пока он не уничтожен
         case TacticType.Concentration:
            if (_concentrationTarget == null || !_concentrationTarget.IsAlive)
            {
               _concentrationTarget = GetRandomAlive(aliveEnemies);
               if (_concentrationTarget == null || !_concentrationTarget.IsAlive)
               {
                  throw new InvalidOperationException("Не осталось живых врагов");
               }
            }
            return _concentrationTarget;
         case TacticType.PriorityType:
            return GetPriorityTypeTarget(plane, aliveEnemies);
         default:
            return aliveEnemies[0];
      }
   }

   private static Plane GetPriorityTypeTarget(Plane attacker, Plane[] aliveEnemies)
   {
      PlaneType priorityType;

      if (attacker.Type == PlaneType.Fighter)
      {
         priorityType = PlaneType.Bomber;
      }
      else if (attacker.Type == PlaneType.Attacker)
      {
         priorityType = PlaneType.Fighter;
      }
      else
      {
         return aliveEnemies[Random.Shared.Next(aliveEnemies.Length)];
      }

      var sameTypeEnemies = aliveEnemies
         .Where(p => p.Type == priorityType)
         .ToArray();

      if (sameTypeEnemies.Length > 0)
      {
         return sameTypeEnemies[Random.Shared.Next(sameTypeEnemies.Length)];
      }

      return aliveEnemies[Random.Shared.Next(aliveEnemies.Length)];
   }

   private static Plane? GetRandomAlive(Plane[] enemyPlanes)
   {
      var aliveEnemies = enemyPlanes.Where(p => p.IsAlive).ToArray();

      return aliveEnemies.Length == 0
         ? null
         : aliveEnemies[Random.Shared.Next(aliveEnemies.Length)];
   }
}

