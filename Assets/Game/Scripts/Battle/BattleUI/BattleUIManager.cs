using UnityEngine;
using System.Collections.Generic;

public class BattleUIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private UnitHUD playerHUDPrefab;
    [SerializeField] private UnitHUD enemyHUDPrefab;

    [SerializeField] private Transform playerHUDRoot;
    [SerializeField] private Transform enemyHUDRoot;

    private readonly List<UnitHUD> playerHUDs = new();
    private readonly List<UnitHUD> enemyHUDs = new();

    public void Initialize(List<BattleUnit> players, List<BattleUnit> enemies)
    {
        CreatePlayerHUDs(players);
        CreateEnemyHUDs(enemies);
    }

    private void CreatePlayerHUDs(List<BattleUnit> players)
    {
        foreach(var player in players)
        {
            UnitHUD hud =
                Instantiate(playerHUDPrefab, playerHUDRoot);

            hud.Setup(player);

            playerHUDs.Add(hud);
        }
    }

    private void CreateEnemyHUDs(
        List<BattleUnit> enemies)
    {
        foreach(var enemy in enemies)
        {
            UnitHUD hud =
                Instantiate(
                    enemyHUDPrefab,
                    enemyHUDRoot);

            hud.Setup(enemy);

            enemyHUDs.Add(hud);
        }
    }
}