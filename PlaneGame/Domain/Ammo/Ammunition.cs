namespace PlaneGame.Domain.Ammo;

public class Ammunition(AmmunitionType type, int damage)
{
    public AmmunitionType Type { get; } = type;
    public int Damage { get; } = damage;
}