using System.ComponentModel.DataAnnotations;

namespace PlaneGame.Domain.Weapons;
public enum WeaponType
{
    [Display(Name = "Синхронные пулеметы")]
    SyncMachineGuns = 0,
    [Display(Name = "Крыльевые пушки")]
    WingGuns = 1,
    [Display(Name = "Турбинные ракеты")]
    TurbineRockets = 2
}