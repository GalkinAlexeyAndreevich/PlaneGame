namespace PlaneGame.Domain.Armors;

public class Armor(ArmorType type, double weight, int defence, int evasionChancePercent = 0)
{
    public ArmorType Type { get; } = type;
    public double Weight { get; } = weight;
    public int Defence { get; } = defence;
    public int EvasionChangePercent { get; } = evasionChancePercent;
}