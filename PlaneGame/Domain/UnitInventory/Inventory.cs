using PlaneGame.Domain.Ammo;
using PlaneGame.Domain.Armors;
using PlaneGame.Domain.Planes;
using PlaneGame.Domain.Weapons;

namespace PlaneGame.Domain.UnitInventory;

public class Inventory(double maxWeight)
{
    public Armor? Armor;
    public List<Weapon> Weapons = [];
    public List<Ammunition> Ammunition = [];
    public double MaxWeight = maxWeight;

    private double GetWeight()
    {
        var armorWeight = Armor?.Weight ?? 0;
        var weaponWeight = Weapons.Sum(w => w.Weight) ;
        var ammunitionWeight = Ammunition.Sum(a => a.Weight);

        return armorWeight + weaponWeight + ammunitionWeight;
    }

    private bool IsCanEquipNewItem(double weight)
    {
        return GetWeight() + weight <= MaxWeight;
    }

    public bool EquipWeapon(Weapon newWeapon, Plane owner)
    {
        if(!IsCanEquipNewItem(newWeapon.Weight)) return false;
        
        Weapons.Add(newWeapon);
        newWeapon.Owner = owner;
        return true;
    }
    
    public bool EquipArmor(Armor newArmor)
    {
        if(!IsCanEquipNewItem(newArmor.Weight)) return false;
        
        Armor = newArmor;
        return true;
    }
    
    public bool EquipAmmunition(Ammunition newAmmunition)
    {
        if(!IsCanEquipNewItem(newAmmunition.Weight)) return false;
        
        // Турбинные ракеты не могут стрелять трассирующими боеприпасами
        // TODO: если оружий больше 1, можно взять, но не стрелять именно с этого оружия этими боеприпасами
        var isTurbineRockets = Weapons.Exists(w => w.Type == WeaponType.TurbineRockets);
        var isTracers = newAmmunition.Type == AmmunitionType.Tracers;
        
        if (isTurbineRockets && isTracers) return false;
        
        Ammunition.Add(newAmmunition);
        return true;
    }

    public Ammunition? TryTakeAmmunitionOnWeapon(Weapon weapon)
    {
        return Ammunition.FirstOrDefault(weapon.CanUseAmmunition);
    }
}
