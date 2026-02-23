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

    internal void EquipAmmunition(Ammunition ammunition)
    {
        _ammunition = ammunition;
    }
    
    private bool IsHit(Plane enemyPlane)
    {
        // Турбинные ракеты игнорируют уклонение
        // Но перезарядка 1 ход
        if (Type == WeaponType.TurbineRockets)
        {
            if (_reloadStep == 0)
            {
                _reloadStep = 1;
                return true;
            }

            _reloadStep -= 1;
            return false;
        }
        
        double accuracyBuffPercent = enemyPlane.IsMarked? 15: 0;
       
        
        var totalAccuracy = BaseAccuracyPercent + AccuracyPercent + accuracyBuffPercent - enemyPlane.EffectiveEvasionChancePercent;
        totalAccuracy = Math.Clamp(totalAccuracy, 0, 100);

        return Random.Shared.Next(100) <= totalAccuracy;
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

        var attackingPlane = Owner;

        // Крыльевые пушки стреляют по 2 выстрела
        var shotsCount = Type == WeaponType.WingGuns ? 2 : 1;
        
        for (var i = 0; i < shotsCount; i++)
        {
            if (!IsHit(enemyPlane))
            {
                Console.WriteLine($"{Owner.GetName()} промахнулся по {enemyPlane.GetName()}");
                continue;
            }
            
            var damageWeapon = Random.Shared.Next(MinDamage, MaxDamage + 1);
            var damage = damageWeapon + _ammunition.Damage;
            
            // Истребитель: +20% к урону, если атакует бомбардировщик
            if (attackingPlane.Type == PlaneType.Fighter && enemyPlane.Type == PlaneType.Bomber)
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

        var isBomber = Owner.Type == PlaneType.Bomber;
        var isSpacedArmor = Owner.Armor?.Type == ArmorType.SpacedArmor;
        
        // Если бомбардировщик без разнесенной брони
        // 10% нанести 15 урона всем врагам
        const int chancePercent = 10;
        const int bomberDamage = 15;

        if (!isBomber || isSpacedArmor)
            return;

        if (Random.Shared.Next(100) >= chancePercent)
            return;

        foreach (var enemy in allEnemies)
        {
            enemy.GetDamage(bomberDamage, Owner);
        }
    }
    
    public Weapon Clone() => new Weapon(Type,  MinDamage, MaxDamage, BaseAccuracyPercent, AccuracyPercent);
}