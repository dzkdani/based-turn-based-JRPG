using UnityEngine;

public class BattleInteraction : Interaction
{
    [Header("Battle Setup")]
    [SerializeField] private EnemyEncounterData encounterData;

    public override void StartInteraction()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager Instance is missing in the scene!");
            return;
        }

        base.StartInteraction();
    }

    public override void EndInteraction()
    {
        // 1. Clear the encounter data from GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CurrentEncounter = null;
        }

        // 2. Run the base logic (Fires OnInteractionEnd event)
        base.EndInteraction();
    }

    public void StartBattle()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager Instance is missing in the scene!");
            return;
        }

        // 1. Set the current encounter data in GameManager
        GameManager.Instance.SetEncounter(encounterData);
    }
}