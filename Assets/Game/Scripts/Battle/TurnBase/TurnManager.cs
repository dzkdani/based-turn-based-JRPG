using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager
{
    private readonly List<BattleUnit> turnOrder = new();
    private int currentIndex;

    public IReadOnlyList<BattleUnit> Units => turnOrder;

    public void Initialize(List<BattleUnit> units)
    {
        turnOrder.Clear();

        if (units != null)
        {
            turnOrder.AddRange(units.Where(unit => unit != null));
            SortBySpeed();
        }

        currentIndex = 0;
    }

    public BattleUnit GetCurrentUnit()
    {
        if (turnOrder.Count == 0)
            return null;

        return turnOrder[currentIndex];
    }

    public void AdvanceTurn()
    {
        if (turnOrder.Count == 0)
        {
            Debug.LogError("Turn order is empty.");
            return;
        }

        int safetyCounter = 0;
        do
        {
            currentIndex = (currentIndex + 1) % turnOrder.Count;
            safetyCounter++;
        }
        while (GetCurrentUnit().IsDead && safetyCounter < turnOrder.Count);

        if (GetCurrentUnit().IsDead)
        {
            Debug.LogWarning("All remaining units are dead.");
        }
    }

    public void AddUnit(BattleUnit unit)
    {
        if (unit == null || turnOrder.Contains(unit))
            return;

        turnOrder.Add(unit);
        SortBySpeed();
    }

    public void RemoveUnit(BattleUnit unit)
    {
        if (unit == null)
            return;

        int index = turnOrder.IndexOf(unit);
        if (index < 0)
            return;

        turnOrder.RemoveAt(index);

        if (currentIndex >= turnOrder.Count)
            currentIndex = 0;
    }

    public void RefreshOrder()
    {
        if (turnOrder.Count == 0)
            return;

        BattleUnit current = GetCurrentUnit();
        SortBySpeed();

        if (current != null)
        {
            currentIndex = turnOrder.IndexOf(current);
            if (currentIndex < 0)
                currentIndex = 0;
        }
    }

    private void SortBySpeed()
    {
        turnOrder.Sort((a, b) => b.Data.CurrentSpd.CompareTo(a.Data.CurrentSpd));
    }
}