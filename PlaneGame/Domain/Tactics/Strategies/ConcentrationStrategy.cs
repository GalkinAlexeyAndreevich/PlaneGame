using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Tactics.Strategies;

public sealed class ConcentrationStrategy : ITargetSelectionStrategy
{
    private Plane? _concentrationTarget;

    public void BeginTurn(Plane[] aliveEnemies)
    {
        if (_concentrationTarget is null || !_concentrationTarget.IsAlive)
        { 
            _concentrationTarget = aliveEnemies[Random.Shared.Next(aliveEnemies.Length)];
        }
    }

    public Plane SelectTarget(Plane attacker, Plane[] aliveEnemies)
    {
        return _concentrationTarget is not null && _concentrationTarget.IsAlive
            ? _concentrationTarget
            : aliveEnemies[Random.Shared.Next(aliveEnemies.Length)];
    }
}
