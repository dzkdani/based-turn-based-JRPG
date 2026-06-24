using System.Collections.Generic;

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
        BattleUnit unit = turnQueue.Dequeue();
        turnQueue.Enqueue(unit);
    }
}