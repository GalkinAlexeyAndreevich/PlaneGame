using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Ammo;

public class Ammunition(AmmunitionType type, double weight, int damage)
{
    public AmmunitionType Type { get; } = type;
    public double Weight { get; } = weight;
    public int Damage { get; } = damage;
    private int _airDefenceAmmo = 10;

    public void ShootOnEnemy(int damageWeapon, Plane attacker, Plane target)
    {
        // Пво с 70% вероятностью сбивает ракету и имеет 10 снарядов
        if (Type == AmmunitionType.ExplosivePiercing && _airDefenceAmmo > 0)
        {
            _airDefenceAmmo--;
            if (Random.Shared.Next(100) <= 70)
            {
                return;
            }
        }

        var damage = damageWeapon + Damage;

        // Истребитель: +20% к урону, если атакует бомбардировщик
        damage = attacker.ModifyOutgoingDamage(target, damage);

        // Определяем эффекты боеприпасов
        var isMarked = Type == AmmunitionType.Tracers;
        var disableEngine = Type == AmmunitionType.ExplosivePiercing;

        target.GetDamage(damage, attacker, isMarked, disableEngine);
    }
}
