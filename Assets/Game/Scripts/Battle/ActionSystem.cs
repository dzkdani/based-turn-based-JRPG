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
        if (actionData == null || targets == null || targets.Count == 0)
        {
            yield break;
        }

        if (actionData.ActionType != BattleActionType.Attack)
        {
            BattleEvents.OnBattleLog?.Invoke($"{actionData.ActionName} is not implemented yet.");
            yield break;
        }

        AttackAction attack = new AttackAction();
        yield return attack.Execute(attacker, targets, actionData);
    }
}
