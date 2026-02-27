using PlaneGame.Domain.Ammo;
using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Weapons;

public class Weapon(WeaponType type, double weight, int minDamage, int maxDamage, int baseAccuracyPercent, int accuracyPercent)
{
    public WeaponType Type { get; } = type;
    public double Weight { get; } = weight;
    private int MinDamage { get; } = minDamage;
    private int MaxDamage { get; } = maxDamage;
    private int BaseAccuracyPercent { get; } = baseAccuracyPercent;
    private int AccuracyPercent { get; } = accuracyPercent;

    /// <summary>Самолёт, на котором установлено оружие. Устанавливается при экипировке.</summary>
    public Plane? Owner { get; internal set; }

    private int _reloadStep;

    public Weapon Clone() => new(Type, Weight, MinDamage, MaxDamage, BaseAccuracyPercent, AccuracyPercent);

    public bool CanUseAmmunition(Ammunition ammunition)
    {
        var isTurbineRockets = Type == WeaponType.TurbineRockets;
        var isTracers = ammunition.Type == AmmunitionType.Tracers;
        
        if (isTurbineRockets && isTracers) return false;
        return true;
    }

    internal void DoDamage(Plane enemyPlane)
    {
        if (Owner is null) return;

        var ammo = Owner.Inventory.TryTakeAmmunitionOnWeapon(this);
        if (ammo is null) return;

        var attacker = Owner;

        // Крыльевые пушки стреляют по 2 выстрела
        var shotsCount = Type == WeaponType.WingGuns ? 2 : 1;
        for (var i = 0; i < shotsCount; i++)
        {
            if (!IsHit(enemyPlane))
            {
                Console.WriteLine($"{attacker.GetName()} промахнулся по {enemyPlane.GetName()}");
                continue;
            }

            var damageWeapon = Random.Shared.Next(MinDamage, MaxDamage);
            ammo.ShootOnEnemy(damageWeapon, attacker, enemyPlane);
        }
    }

    private bool IsHit(Plane enemyPlane)
    {
        if (Type == WeaponType.TurbineRockets)
        {
            return HandleTurbineRocketsHit();
        }

        double markBonusPercent = enemyPlane.IsMarked ? 15 : 0;

        var totalAccuracyPercent =
            BaseAccuracyPercent +
            AccuracyPercent +
            markBonusPercent -
            enemyPlane.EffectiveEvasionChancePercent;

        totalAccuracyPercent = Math.Clamp(totalAccuracyPercent, 0, 100);

        return Random.Shared.Next(100) <= totalAccuracyPercent;
    }

    private bool HandleTurbineRocketsHit()
    {
        // Турбинные ракеты:
        // - игнорируют уклонение
        // - имеют перезарядку 1 ход
        if (_reloadStep > 0)
        {
            _reloadStep--;
            return false;
        }

        _reloadStep = 1;
        return true;
    }
}
