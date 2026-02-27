namespace PlaneGame.Domain.Planes;

public sealed class AttackerPlane() : Plane(480, 700, 10)
{
    public override PlaneType Type => PlaneType.Attacker;

    private bool _isFirstHitIgnored;
    
    public override Plane Clone() => new AttackerPlane();

    protected override bool TryIgnoreIncomingHit(Plane enemyPlane)
    {
        if (_isFirstHitIgnored) return false;
        
        _isFirstHitIgnored = true;
        Console.WriteLine($"Бронированная кабина {GetName()} поглотила удар {enemyPlane.GetName()}");
        return true;
    }
}
