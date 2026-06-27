using UnityEngine;

public class TargetPanel : MonoBehaviour
{
    public void SelectTarget(BattleUnit target)
    {
        BattleEvents.OnTargetSelected?.Invoke(target);
    }
}
