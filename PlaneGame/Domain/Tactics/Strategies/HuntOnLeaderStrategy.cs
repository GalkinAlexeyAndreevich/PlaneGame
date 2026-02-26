using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Tactics.Strategies;

public sealed class HuntOnLeaderStrategy : ITargetSelectionStrategy
{
    public void BeginTurn(Plane[] aliveEnemies) { }

    public Plane SelectTarget(Plane attacker, Plane[] aliveEnemies)
    {
        return aliveEnemies.OrderByDescending(p => p.Hp).ThenByDescending(p => p.MaxHp).First();
    }
}
