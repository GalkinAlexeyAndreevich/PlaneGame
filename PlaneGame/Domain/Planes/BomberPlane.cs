using PlaneGame.Domain.Armors;

namespace PlaneGame.Domain.Planes;

public sealed class BomberPlane() : Plane(680, 5)
{
    public override PlaneType Type => PlaneType.Bomber;

    public override Plane Clone() => new BomberPlane();

    protected override void SplashAttack(Plane[] allEnemies)
    {
        // Бомбардировщик с разнесенной броней теряет способность к площадной бомбежке
        if (Armor?.Type == ArmorType.SpacedArmor)
        {
            return;
        }

        const int chancePercent = 10;
        const int splashDamage = 15;

        if (Random.Shared.Next(100) >= chancePercent)
        {
            return;
        }

        Console.WriteLine($"{GetName()} сбрасывает бомбы на всех противников сразу");
        foreach (var enemy in allEnemies)
        {
            enemy.GetDamage(splashDamage, this);
        }
    }
}
