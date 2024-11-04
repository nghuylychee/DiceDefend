using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceConfig", menuName = "DICE/DiceConfig")]
public class DiceConfig : ScriptableObject
{
    public int ID;
    public Sprite Icon;
    public string DiceName;
    [TextArea]
    public string DiceDesc;
    public int Price;
    public List<Sprite> DiceSprite;
}

