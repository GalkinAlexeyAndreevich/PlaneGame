using System.ComponentModel.DataAnnotations;

namespace PlaneGame.Domain.Ammo;

public enum AmmunitionType
{
    [Display(Name = "Трассирующие")]
    Tracers = 0,
    [Display(Name = "Бронебойные")]
    ArmorPiercing = 1,
    [Display(Name = "Бронебойно-фугасные")]
    ExplosivePiercing = 2
}