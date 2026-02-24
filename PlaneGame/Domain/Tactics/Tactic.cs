using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Tactics;

public class Tactic(TacticType tactic)
{
   private TacticType TacticType { get; set; } = tactic;
   private Plane? _concentrationTarget;
   
   public void SetTactic(TacticType tactic)
   {
      TacticType = tactic;
      _concentrationTarget = null;
   }

   public Plane GetTargetPlane(Plane attacker, Plane[] enemyPlanes)
   {
      var aliveEnemies = enemyPlanes.Where(p => p.IsAlive).ToArray();
      if (aliveEnemies.Length == 0)
      {
         throw new InvalidOperationException("Не осталось живых врагов");
      }

      return TacticType switch
      {
         TacticType.CommanderOrder => GetRandomEnemy(aliveEnemies),

         TacticType.HuntOnLeader =>
            aliveEnemies
               .OrderByDescending(p => p.Hp)
               .ThenByDescending(p => p.MaxHp)
               .First(),

         TacticType.Finishing =>
            aliveEnemies
               .OrderBy(p => p.Hp)
               .ThenBy(p => p.MaxHp)
               .First(),

         TacticType.Concentration =>
            GetConcentrationTarget(aliveEnemies),

         TacticType.PriorityType =>
            GetPriorityTypeTarget(attacker, aliveEnemies),

         _ => aliveEnemies[0]
      };
   }
   
   private static Plane GetRandomEnemy(Plane[] enemies)
   {
      return enemies[Random.Shared.Next(enemies.Length)];
   }
   
   private Plane GetConcentrationTarget(Plane[] aliveEnemies)
   {
      if (_concentrationTarget is null || !_concentrationTarget.IsAlive)
      {
         _concentrationTarget = GetRandomEnemy(aliveEnemies);
      }
         
      return _concentrationTarget;
   }

   private static Plane GetPriorityTypeTarget(Plane attacker, Plane[] aliveEnemies)
   {
      PlaneType? priorityType = attacker.Type switch
      {
         PlaneType.Fighter => PlaneType.Bomber,
         PlaneType.Attacker => PlaneType.Fighter,
         _ => null
      };

      if (priorityType is null)
      {
         return GetRandomEnemy(aliveEnemies);
      }

      var sameTypeEnemies = aliveEnemies
         .Where(p => p.Type == priorityType)
         .ToArray();

      return sameTypeEnemies.Length > 0
         ? GetRandomEnemy(sameTypeEnemies)
         : GetRandomEnemy(aliveEnemies);
   }
}

