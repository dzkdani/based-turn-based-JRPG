using UnityEngine;

public class BattleInteraction : Interaction
{
    [Header("Battle Setup")]
    [SerializeField] private EnemyEncounterData encounterData;

    public override void StartInteraction()
    {
        base.StartInteraction();
    }

    public override void EndInteraction()
    {
        // Do not clear encounter state here; battle flow should manage encounter lifecycle.
        base.EndInteraction();
    }

    public void StartBattle()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager is missing in the scene!", this);
            return;
        }

        GameManager.Instance.SetEncounter(encounterData);
    }
}