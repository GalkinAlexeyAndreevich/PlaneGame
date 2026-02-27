using PlaneGame.Domain.Planes;
using PlaneGame.Domain.Tactics;

namespace PlaneGame.Domain;

public class Team(int teamId)
{
    public int TeamId { get; } =  teamId;
    public Plane[] Planes { get; private set; } = [];
    
    private Tactic TeamTactic { get; } = new Tactic(TacticType.HuntOnLeader);

    public void AddPlanesToTeam(Plane[] planes)
    {
        Planes = planes;
        foreach (var plane in planes)
        {
            plane.AssignToTeam(TeamId);
        }
    }

    public void SetTactic(TacticType tactic)
    {
        TeamTactic.SetTactic(tactic);
    }
    
    public void DoDamage(Plane[] enemyPlanes)
    {
        TeamTactic.BeginTurn(enemyPlanes);
        foreach (var plane in Planes)
        {
            if (!plane.IsAlive) continue;
            
            var enemyTarget = TeamTactic.GetTargetPlane(plane, enemyPlanes);
            plane.DoDamage(enemyTarget, enemyPlanes);
        }
    }

    public bool IsAlive => Planes.Any(p => p.IsAlive);
}
