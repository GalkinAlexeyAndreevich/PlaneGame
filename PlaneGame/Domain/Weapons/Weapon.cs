using PlaneGame.Domain.Ammo;
using PlaneGame.Domain.Armors;
using PlaneGame.Domain.Planes;

namespace PlaneGame.Domain.Weapons;

public class Weapon(WeaponType type, int minDamage, int maxDamage, int baseAccuracyPercent, int accuracyPercent)
{
    public WeaponType Type { get; } = type;
    private int MinDamage { get; } = minDamage;
    private int MaxDamage { get; } = maxDamage;
    private int BaseAccuracyPercent { get; } = baseAccuracyPercent;
    private int AccuracyPercent { get; } = accuracyPercent;
    
    /// <summary>Самолёт, на котором установлено оружие. Устанавливается при экипировке.</summary>
    public Plane? Owner { get; internal set; }
    
    private Ammunition? _ammunition;

    private int _reloadStep; 
    
    public Weapon Clone() => new(Type,  MinDamage, MaxDamage, BaseAccuracyPercent, AccuracyPercent);

    internal void EquipAmmunition(Ammunition ammunition)
    {
        _ammunition = ammunition;
    }
    
    internal void DoDamage(Plane enemyPlane, Plane[] allEnemies)
    {
        if (Owner == null)
        {
            throw new InvalidOperationException("Оружие не установлено на самолёт");
        }
        if (_ammunition == null)
        {
            throw new InvalidOperationException("Не экипированы боеприпасы");
        }

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
            var damage = damageWeapon + _ammunition.Damage;
            
            // Истребитель: +20% к урону, если атакует бомбардировщик
            if (attacker.Type == PlaneType.Fighter && enemyPlane.Type == PlaneType.Bomber)
            {
                damage = (int)(damage * 1.2);
            }
            
            // Определяем эффекты боеприпасов
            var isMarked = _ammunition.Type == AmmunitionType.Tracers;
            var disableEngine = _ammunition.Type == AmmunitionType.ExplosivePiercing;
            
            enemyPlane.GetDamage(damage, Owner, isMarked, disableEngine);
            
            if (isMarked)
            {
                Console.WriteLine($"{Owner.GetName()} пометил цель {enemyPlane.GetName()}");
            }
            if (disableEngine)
            {
                Console.WriteLine($"{Owner.GetName()} заглушил двигатель у {enemyPlane.GetName()}");
            }
        }
        
        BomberSplashAttack(allEnemies);
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
    
    private void BomberSplashAttack(Plane[] allEnemies)
    {
        if (Owner is null)
        {
            throw new InvalidOperationException("Оружие не установлено на самолёт");
        }
        
        var isBomber = Owner.Type == PlaneType.Bomber;
        var isSpacedArmor = Owner.Armor?.Type == ArmorType.SpacedArmor;

        if (isBomber || isSpacedArmor)
        {
            return;
        }   

        const int chancePercent = 10;
        const int splashDamage = 15;

        if (Random.Shared.Next(100) >= chancePercent)
        {
            return; 
        }

        foreach (var enemy in allEnemies)
        {
            enemy.GetDamage(splashDamage, Owner);
        }
    }
}
