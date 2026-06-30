using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAction : BattleAction
{
    public override IEnumerator Execute(
        BattleUnit attacker,
        List<BattleUnit> targets,
        BattleActionSO actionData)
    {
        BattleEvents.OnBattleLog?.Invoke($"{attacker.name} can't run!!\nnot implemented yet ehe..");
        yield return new WaitForSeconds(1.5f);

        // yield return null;
    }
}