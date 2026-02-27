using PlaneGame.Domain.Ammo;
using PlaneGame.Domain.Armors;
using PlaneGame.Domain.Planes;
using PlaneGame.Domain.Weapons;

namespace PlaneGame.Domain;

public static class GameData
{
    public static readonly Plane[] Planes =
    [
        new FighterPlane(),
        new AttackerPlane(),
        new BomberPlane(),
    ];

    public static readonly Weapon[] Weapons =
    [
        new Weapon(WeaponType.SyncMachineGuns,150, 10, 15, 70, 15),
        new Weapon(WeaponType.WingGuns, 200,20, 30, 70, -10),
        new Weapon(WeaponType.TurbineRockets, 230, 35, 40, 70, 0),
    ];

    public static readonly Armor[] Armors =
    [
        new Armor(ArmorType.ProtectedGasTank, 300, 15),
        new Armor(ArmorType.PilotArmorPlate, 310, 20),
        new Armor(ArmorType.SpacedArmor,  25,-10),
    ];

    public static readonly Ammunition[] Ammunition =
    [
        new Ammunition(AmmunitionType.Tracers, 1.5, 12),
        new Ammunition(AmmunitionType.ArmorPiercing, 0.8, 10),
        new Ammunition(AmmunitionType.ExplosivePiercing, 2.4,18)
    ];
}
