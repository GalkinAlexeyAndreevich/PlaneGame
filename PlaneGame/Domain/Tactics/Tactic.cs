using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Tactics;

public sealed class Tactic(TacticType tacticType)
{
   private ITargetSelectionStrategy _strategy = TacticStrategyFactory.Create(tacticType);

   public void SetTactic(TacticType tacticType)
   {
      _strategy = TacticStrategyFactory.Create(tacticType);
   }

   public void BeginTurn(Plane[] enemyPlanes)
   {
      var alive = enemyPlanes.Where(p => p.IsAlive).ToArray();
      if (alive.Length == 0) throw new InvalidOperationException("Не осталось живых врагов");
      _strategy.BeginTurn(alive);
   }

   public Plane GetTargetPlane(Plane attacker, Plane[] enemyPlanes)
   {
      var alive = enemyPlanes.Where(p => p.IsAlive).ToArray();
      if (alive.Length == 0)
      {
         throw new InvalidOperationException("Не осталось живых врагов");
      }
      return _strategy.SelectTarget(attacker, alive);
   }
}
