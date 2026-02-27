namespace PlaneGame.Domain.Ammo;

public class Ammunition(AmmunitionType type, double weight, int damage)
{
    public AmmunitionType Type { get; } = type;
    public double Weight { get; } = weight;
    public int Damage { get; } = damage;
}