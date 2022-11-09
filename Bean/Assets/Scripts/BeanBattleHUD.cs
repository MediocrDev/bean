using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeanBattleHUD : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text levelText;
    public TMP_Text hpText_Current;
    public TMP_Text hpText_Max;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "" + unit.unitLevel;
        hpText_Current.text = "" + unit.currentHP;
        hpText_Max.text = "" + unit.maxHP;
    }

    public void SetHP(int hp)
    {
        hpText_Current.text = "" + hp;
    }
}
