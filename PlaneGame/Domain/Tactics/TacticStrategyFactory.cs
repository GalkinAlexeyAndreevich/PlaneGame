using PlaneGame.Domain.Tactics.Strategies;

namespace PlaneGame.Domain.Tactics;

public static class TacticStrategyFactory
{
    public static ITargetSelectionStrategy Create(TacticType tacticType) => tacticType switch
    {
        TacticType.CommanderOrder => new CommanderOrderStrategy(),
        TacticType.HuntOnLeader => new HuntOnLeaderStrategy(),
        TacticType.Finishing => new FinishingStrategy(),
        TacticType.Concentration => new ConcentrationStrategy(),
        TacticType.PriorityType => new PriorityTypeStrategy(),
        _ => new HuntOnLeaderStrategy()
    };
}
