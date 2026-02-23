using System.ComponentModel.DataAnnotations;

namespace PlaneGame.Domain.Armors;
public enum ArmorType
{
    [Display(Name = "Протектированный бензобак")]
    ProtectedGasTank = 0,
    [Display(Name = "Бронеспинка летчика")]
    PilotArmorPlate = 1,
    [Display(Name = "Разнесенная броня")]
    SpacedArmor = 2
}