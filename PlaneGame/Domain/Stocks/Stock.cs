using PlaneGame.Domain.Ammo;
using PlaneGame.Domain.Armors;
using PlaneGame.Domain.Weapons;

namespace PlaneGame.Domain.Stocks;

public class Stock
{
    public List<Armor> Armors { get; private set; } = [];
    public List<Weapon> Weapons { get; private set; } = [];
    public List<Ammunition> Ammunition { get; private set; } = [];
    
    public void GenerateStock()
    {
        GenerateArmors(10);
        GenerateWeapons(10);
        GenerateAmmunition(100);
    }

    private void GenerateArmors(int countArmors)
    {
        for (var i = 0; i < countArmors; i++)
        {
            var randomIndex = Random.Shared.Next(GameData.Armors.Length);
            var armor = GameData.Armors[randomIndex];
            Armors.Add(armor);
        }
    }
    
    private void GenerateWeapons(int countWeapons)
    {
        for (var i = 0; i < countWeapons; i++)
        {
            var randomIndex = Random.Shared.Next(GameData.Weapons.Length);
            var weapon = GameData.Weapons[randomIndex].Clone();
            Weapons.Add(weapon);
        }
    }
    
    private void GenerateAmmunition(int countAmmunition)
    {
        for (var i = 0; i < countAmmunition; i++)
        {
            var randomIndex = Random.Shared.Next(GameData.Ammunition.Length);
            var ammunition = GameData.Ammunition[randomIndex];
            Ammunition.Add(ammunition);
        }
    }
}
