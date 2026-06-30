using System.Collections;
using System.Collections.Generic;

public class ActionSystem
{
    public List<BattleUnit> ResolveTargets(
        BattleUnit caster,
        BattleActionSO action,
        List<BattleUnit> players,
        List<BattleUnit> enemies,
        BattleUnit preferredTarget,
        TargetSystem targetSystem)
    {
        return targetSystem.GetTargets(
            caster,
            action.TargetType,
            players,
            enemies,
            preferredTarget);
    }

    public IEnumerator ExecuteAction(
    BattleUnit attacker,
    List<BattleUnit> targets,
    BattleActionSO actionData)
    {
        if (actionData == null)
            yield break;

        BattleAction action = CreateAction(actionData.ActionType);

        if (action == null)
            yield break;

        yield return action.Execute(attacker, targets, actionData);
    }

    private BattleAction CreateAction(BattleActionType type)
    {
        switch (type)
        {
            case BattleActionType.Attack:
                return new AttackAction();

            case BattleActionType.Run:
                return new RunAction();

            default:
                return null;
        }
    }
}
