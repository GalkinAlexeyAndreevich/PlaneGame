using System.ComponentModel.DataAnnotations;

namespace PlaneGame.Domain.Planes;
public enum PlaneType
{
    [Display(Name = "Истребитель")]
    Fighter = 0,
    [Display(Name = "Штурмовик")]
    Attacker = 1,
    [Display(Name = "Бомбардировщик")]
    Bomber = 2
}