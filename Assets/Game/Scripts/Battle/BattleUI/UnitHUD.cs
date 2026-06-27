using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnitHUD : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text hpText;
    [SerializeField] Slider hpBar;

    BattleUnit unit;

    public void Setup(BattleUnit target)
    {
        unit = target;
        nameText.text = unit.Data.Name;
        hpBar.maxValue = unit.Data.MaxHP;

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

    public void Show()
    {
        
    }

    public void Hide()
    {
        
    }

    public void SetSelected(bool selected)
    {
        
    }
    
    void Refresh()
    {
        nameText.text = unit.Data.Name;
        hpText.text = $"{unit.Data.CurrentHP}/{unit.Data.MaxHP}";
        hpBar.DOValue(
            unit.Data.CurrentHP,
            0.3f
        );
    }
}