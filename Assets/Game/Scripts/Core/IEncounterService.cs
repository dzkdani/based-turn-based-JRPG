public interface IEncounterService
{
    EnemyEncounterData CurrentEncounter { get; }
    void SetEncounter(EnemyEncounterData encounter);
}
