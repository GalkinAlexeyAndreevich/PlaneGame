using PlaneGame.Domain.Ammo;
using PlaneGame.Domain.Armors;
using PlaneGame.Domain.Weapons;
using PlaneGame.Extensions;

namespace PlaneGame.Domain.Planes;

public abstract class Plane
{
    public abstract PlaneType Type { get; }
    public int MaxHp { get; }
    public int Hp { get; private set; }
    private int BaseEvasionChancePercent { get; }

    private Weapon? _weapon;
    public Armor? Armor;

    public int TeamId;
    /// <summary>Отложенный урон, в конце хода вычитается из hp.</summary>
    private int _pendingDamage;
    public bool IsAlive => Hp > 0;
    
    public bool IsMarked { get; private set; }
    private bool _isSkipNextTurn;
    private bool _isFirstHitIgnored;
    
    // уклонение с учетом уклонения от брони
    public int EffectiveEvasionChancePercent => BaseEvasionChancePercent + (Armor?.EvasionChangePercent ?? 0);
    
    protected Plane(int hp, int evasionChancePercent)
    {
        MaxHp = hp;
        Hp = hp;
        BaseEvasionChancePercent = evasionChancePercent;
    }
    
    public abstract Plane Clone();

    public void GetDamage(int damage, Plane enemyPlane, bool isMarked = false, bool disableEngine = false)
    {
        if (damage <= 0) return;
        
        // Штурмовик игнорирует первый удар за бой
        if (Type == PlaneType.Attacker && !_isFirstHitIgnored)
        {
            _isFirstHitIgnored = true;
            Console.WriteLine($"Бронированная кабина {GetName()} поглотила удар {enemyPlane.GetName()}");
            return;
        }
        
        if (Armor is not null)
        {
            var defenceModifier = (100 - Armor.Defence) / 100.0;
            damage = (int)(damage * defenceModifier);
            damage = Math.Max(0, damage); 
        }
        
        _pendingDamage += damage;
        
        Console.WriteLine($"{enemyPlane.GetName()} нанес {damage} урона по {GetName()}");
        
        if (isMarked)
        {
            IsMarked = true;
            Console.WriteLine($"{enemyPlane.GetName()} пометил цель {GetName()}");
        }
        if (disableEngine)
        {
            _isSkipNextTurn = true;
            Console.WriteLine($"{enemyPlane.GetName()} заглушил двигатель у {GetName()}");
        }
    }
    
    public void ApplyTurnDamage()
    {
        if (_pendingDamage <= 0) return;

        Hp -= _pendingDamage;
        if (Hp < 0) Hp = 0;
        _pendingDamage = 0;
    }

    public void DoDamage(Plane enemyPlane, Plane[] allEnemies)
    {
        if (_isSkipNextTurn)
        {
            _isSkipNextTurn =  false;
            return;
        }
        
        _weapon?.DoDamage(enemyPlane, allEnemies);
    }
    
    // Истребитель переопределит (+20% против бомбардировщика)
    public virtual int ModifyOutgoingDamage(Plane enemy, int damage) => damage;

    private void Equip(Weapon weapon, Armor armor, Ammunition ammunition)
    {
        if (weapon.Type == WeaponType.TurbineRockets && ammunition.Type == AmmunitionType.Tracers)
        {
            throw new ArgumentException("Турбинные ракеты не могут стрелять трассирующими боеприпасами");
        }
        
        Armor = armor;
        _weapon = weapon;
        _weapon.Owner = this;
        _weapon.EquipAmmunition(ammunition);
    }
    
    public void EquipRandom(Weapon[] weapons, Armor[] armors, Ammunition[] ammunition)
    {
        if (weapons.Length == 0 || armors.Length == 0 || ammunition.Length == 0)
        {
            throw new InvalidOperationException("Наборы экипировки пусты.");
        }
        
        var weapon = weapons[Random.Shared.Next(weapons.Length)].Clone();
        var armor = armors[Random.Shared.Next(armors.Length)];

        var ammoPool = weapon.Type == WeaponType.TurbineRockets
            ? ammunition.Where(a => a.Type != AmmunitionType.Tracers).ToArray()
            : ammunition.ToArray();

        if (ammoPool.Length == 0)
        {
            throw new InvalidOperationException("Нет допустимых боеприпасов для выбранного оружия.");
        }
        
        var ammo = ammoPool[Random.Shared.Next(ammoPool.Length)];

        Equip(weapon, armor, ammo);
    }
    
    public string GetName()
    {
        var typeName = Type.GetDisplayName();
        var teamName = $"{typeName}_Team{TeamId}";
        return teamName;
    }
}
