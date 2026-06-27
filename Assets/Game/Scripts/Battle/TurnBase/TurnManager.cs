using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    Queue<BattleUnit> turnQueue;

    public void Initialize(List<BattleUnit> units)
    {
        units.Sort((a,b) => b.Data.CurrentSpd.CompareTo(a.Data.CurrentSpd));
        turnQueue = new Queue<BattleUnit>(units);
    }

    public BattleUnit GetCurrentUnit()
    {
        return turnQueue.Peek();
    }

    public void AdvanceTurn()
    {
        if(turnQueue == null || turnQueue.Count == 0)
        {
            Debug.LogError("Turn Queue Empty!");
            return;
        }

        int safetyCounter = 0;
        do
        {
            BattleUnit unit = turnQueue.Dequeue();
            turnQueue.Enqueue(unit);
            safetyCounter++;

        } while(GetCurrentUnit().IsDead && safetyCounter < turnQueue.Count);

        if(GetCurrentUnit().IsDead)
        {
            Debug.LogWarning("All remaining units are dead.");
            return;
        }
    }
}