using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "ScriptableObjects/Unit/Skills", order = 1)]
public class BattleActionSO : ScriptableObject
{
    public string ActionName;

    public BattleActionType ActionType;

    public TargetType TargetType;

    public TargetRequirement TargetRequirement;

    public int Multiplier;

    public AudioClip SFX;
}

public enum BattleActionType
{
    Attack,
    Heal,
    Run
}

public enum TargetRequirement
{
    None,
    Required
}