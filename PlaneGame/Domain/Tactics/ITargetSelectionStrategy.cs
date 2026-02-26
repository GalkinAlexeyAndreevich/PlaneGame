using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Tactics;

public interface ITargetSelectionStrategy
{
    // Вызывается один раз на ход для команды
    void BeginTurn(Plane[] aliveEnemies);

    // Выбор цели для конкретного самолёта
    Plane SelectTarget(Plane attacker, Plane[] aliveEnemies);
}
