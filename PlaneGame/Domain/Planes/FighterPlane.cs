namespace PlaneGame.Domain.Planes;

public sealed class FighterPlane(): Plane(320, 500, 25)
{
    public override PlaneType Type => PlaneType.Fighter;

    public override Plane Clone() => new FighterPlane();

    public override int ModifyOutgoingDamage(Plane enemy, int damage)
    {
        // +20% к урону, если атакует бомбардировщик
        if (enemy.Type == PlaneType.Bomber)
        {
            return (int)(damage * 1.2);
        }

        return damage;
    }
}
