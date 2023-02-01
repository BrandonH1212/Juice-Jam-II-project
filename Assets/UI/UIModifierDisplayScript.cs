using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class OperatorIcons
{
    public static Dictionary<EffectOperator, Sprite> Data = new()
    {
        { EffectOperator.Summation, Resources.Load<Sprite>("icons/add icon")},
        { EffectOperator.Multiplication, Resources.Load<Sprite>("icons/percentage icon")},
    };

}

public static class ModiferTargetIcons
{
    public static Dictionary<ModiferTarget, Sprite> sprite = new()
    {
        { ModiferTarget.All, Resources.Load<Sprite>("icons/global icon") },
        { ModiferTarget.Adjacent, Resources.Load<Sprite>("icons/double arrow icon") },
        { ModiferTarget.Left, Resources.Load<Sprite>("icons/arrow left icon") },
        { ModiferTarget.Right, Resources.Load<Sprite>("icons/arrow right icon") },
        { ModiferTarget.AllLeft, Resources.Load<Sprite>("icons/arrow left icon") },
        { ModiferTarget.AllRight, Resources.Load<Sprite>("icons/arrow right icon") },
    };
    
    public static Dictionary<ModiferTarget, string> descriptions = new()
       {
        { ModiferTarget.All, "Affects all Cards" },
        { ModiferTarget.Adjacent, "Affects all adjacent Cards" },
        { ModiferTarget.Left, "Affects all Cards to the left" },
        { ModiferTarget.Right, "Affects all Cards to the right" },
        { ModiferTarget.AllLeft, "Affects all Cards to the left" },
        { ModiferTarget.AllRight, "Affects all Cards to the right" },
    };

}

public class UIModifierDisplayScript : MonoBehaviour
{
    private Image AffectedIcon;
    private Image OperatorIcon;
    //private TooltipMouseOver _toolTipMouseOver;
    private UIStatDisplayScript _statDisplay;


    void Awake()
    {
        AffectedIcon = GetComponentsInChildren<Image>()[1];
        OperatorIcon = GetComponentsInChildren<Image>()[2];
        _statDisplay = GetComponentsInChildren<UIStatDisplayScript>()[0];
        //_toolTipMouseOver = GetComponentsInChildren<TooltipMouseOver>()[0];

    }

    public void SetModifierDisplay(Modifier mod)
    {
        AffectedIcon.sprite = ModiferTargetIcons.sprite[mod.Targets];
        OperatorIcon.sprite = OperatorIcons.Data[mod.Operator];
        //print(mod.StatToAffect.ToString());
        _statDisplay.SetStatDisplay(StatInfo.Data[mod.StatToAffect].Icon, mod.Value, mod.StatToAffect);
        //_toolTipMouseOver.title = mod.Targets.ToString();
        //_toolTipMouseOver.description = ModiferTargetIcons.descriptions[mod.Targets];

    }
}
