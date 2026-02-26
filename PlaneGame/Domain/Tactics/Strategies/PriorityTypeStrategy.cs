using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Tactics.Strategies;

public sealed class PriorityTypeStrategy : ITargetSelectionStrategy
{
    public void BeginTurn(Plane[] aliveEnemies) { }

    public Plane SelectTarget(Plane attacker, Plane[] aliveEnemies)
    {
        PlaneType? priorityType = attacker.Type switch
        {
            PlaneType.Fighter => PlaneType.Bomber,
            PlaneType.Attacker => PlaneType.Fighter,
            _ => null
        };

        if (priorityType is null)
        {
            return aliveEnemies[Random.Shared.Next(aliveEnemies.Length)];
        }

        var sameTypeEnemies = aliveEnemies.Where(p => p.Type == priorityType).ToArray();
        var enemies = sameTypeEnemies.Length > 0 ? sameTypeEnemies : aliveEnemies;
        return enemies[Random.Shared.Next(enemies.Length)];
    }
}
