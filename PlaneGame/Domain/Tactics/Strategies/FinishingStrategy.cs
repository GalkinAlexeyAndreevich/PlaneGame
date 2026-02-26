using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Tactics.Strategies;

public sealed class FinishingStrategy : ITargetSelectionStrategy
{
    public void BeginTurn(Plane[] aliveEnemies) { }

    public Plane SelectTarget(Plane attacker, Plane[] aliveEnemies)
    {
        return aliveEnemies.OrderBy(p => p.Hp).ThenBy(p => p.MaxHp).First();
    }
}
