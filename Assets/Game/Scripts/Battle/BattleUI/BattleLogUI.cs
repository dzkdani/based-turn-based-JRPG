using UnityEngine;
using TMPro;

public class BattleLogUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text logText;

    void OnEnable()
    {
        BattleEvents.OnBattleLog += UpdateLog;
    }

    void OnDisable()
    {
        BattleEvents.OnBattleLog -= UpdateLog;
    }

    void UpdateLog(string msg)
    {
        logText.text = msg;
    }
}