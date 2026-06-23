using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data")]
public class CharacterDataSO : ScriptableObject
{
    public string Name;
    public float MaxHealth;
    public float MaxMana;
    public float Attack;
    public float Defense;
    public Mesh model;
    public CharacterType characterType;
}

public enum CharacterType
{
    Player,
    NPC,
    Enemy
}