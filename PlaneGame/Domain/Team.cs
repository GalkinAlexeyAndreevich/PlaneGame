using PlaneGame.Domain.Planes;
using PlaneGame.Domain.Tactics;

namespace PlaneGame.Domain;

public class Team(int teamId)
{
    public int TeamId { get; set; } =  teamId;
    public Plane[] Planes { get; private set; } = [];
    
    private Tactic TeamTactic { get; } = new Tactic(TacticType.HuntOnLeader);

    public void AddPlanesToTeam(Plane[] planes)
    {
        Planes =  planes;
        foreach (var plane in planes)
        {
            plane.TeamId = TeamId;
        }
    }

    public void SetTactic(TacticType tactic)
    {
        TeamTactic.SetTactic(tactic);
    }
    
    public void DoDamage(Plane[] enemyPlanes)
    {
        foreach (var plane in Planes)
        {
            if (!plane.IsAlive) continue;
            
            var enemyTarget = TeamTactic.GetTargetPlane(plane, enemyPlanes);
            plane.DoDamage(enemyTarget, enemyPlanes);
        }
    }

    public bool IsAlive => Planes.Any(p => p.IsAlive);
}
