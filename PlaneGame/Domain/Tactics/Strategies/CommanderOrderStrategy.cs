using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Tactics.Strategies;

public sealed class CommanderOrderStrategy : ITargetSelectionStrategy
{
    private Plane? _commanderTarget;

    public void BeginTurn(Plane[] aliveEnemies)
    {
        _commanderTarget = aliveEnemies[Random.Shared.Next(aliveEnemies.Length)];
    }

    public Plane SelectTarget(Plane attacker, Plane[] aliveEnemies)
    {
        return _commanderTarget ?? aliveEnemies[0];
    }
}
