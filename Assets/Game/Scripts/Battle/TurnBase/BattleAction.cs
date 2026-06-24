using System.Collections.Generic;
using System.Collections;

public abstract class BattleAction
{
    public abstract IEnumerator Execute(BattleUnit user, List<BattleUnit> targets);
}