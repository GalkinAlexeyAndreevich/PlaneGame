using PlaneGame.Domain.Ammo;
using PlaneGame.Domain.Armors;
using PlaneGame.Domain.Weapons;

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

    public bool EquipWeapon(Weapon newWeapon)
    {
        if(!IsCanEquipNewItem(newWeapon.Weight)) return false;
        
        Weapons.Add(newWeapon);
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
        
        Ammunition.Add(newAmmunition);
        return true;
    }
}
