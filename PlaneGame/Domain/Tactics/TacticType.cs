using System.ComponentModel.DataAnnotations;

namespace PlaneGame.Domain.Tactics;

public enum TacticType
{
    [Display(Name = "Приказ командира")]
    CommanderOrder = 0,
    [Display(Name = "Охота на лидера")]
    HuntOnLeader = 1,
    [Display(Name = "Добивание")]
    Finishing = 2,
    [Display(Name = "Концентрация")]
    Concentration = 3,
    [Display(Name = "По приоритету типов")]
    PriorityType = 4
}
