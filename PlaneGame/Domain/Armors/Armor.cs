namespace PlaneGame.Domain.Armors;

public class Armor(ArmorType type, int defence, int evasionChancePercent = 0)
{
    public ArmorType Type { get; } = type;
    public int Defence { get; } = defence;
    public int EvasionChangePercent { get; } = evasionChancePercent;
}