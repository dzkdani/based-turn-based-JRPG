using System.Collections;
using System.Collections.Generic;

public class ActionSystem
{
    public IEnumerator ExecuteAction(
        BattleAction action,
        BattleUnit user,
        List<BattleUnit> targets)
    {
        yield return action.Execute(user, targets);
    }
}