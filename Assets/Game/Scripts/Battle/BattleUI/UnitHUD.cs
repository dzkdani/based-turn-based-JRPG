using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnitHUD : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;
    [SerializeField] Slider hpBar;

    BattleUnit unit;

    public void Setup(BattleUnit target)
    {
        unit = target;

        Refresh();
    }

    void OnEnable()
    {
        BattleEvents.OnUnitDamaged += OnDamaged;
    }

    void OnDisable()
    {
        BattleEvents.OnUnitDamaged -= OnDamaged;
    }

    void OnDamaged(BattleUnit damaged)
    {
        if(damaged != unit)
            return;

        Refresh();
    }

    void Refresh()
    {
        hpText.text =
            $"{unit.Data.CurrentHP}/{unit.Data.MaxHP}";
        hpBar.DOValue(
            unit.Data.CurrentHP,
            0.3f
        );
    }
}