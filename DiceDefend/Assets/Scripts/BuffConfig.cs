using UnityEngine;

[CreateAssetMenu(fileName = "BuffConfig", menuName = "DICE/BuffConfig")]
public class BuffConfig : ScriptableObject
{
    [Header("Buff Info")]
    public int buffID;
    public string buffName;
    [TextArea(3, 5)]
    public string buffDescription;
    public Sprite buffIcon;
    
    [Header("Buff Type")]
    public BuffType buffType;
    
    [Header("Buff Values")]
    public float value1; // Giá trị chính (damage, speed, etc.)
    public float value2; // Giá trị phụ (duration, cooldown, etc.)
    public float value3; // Giá trị bổ sung
    
    [Header("Buff Rarity")]
    public BuffRarity rarity = BuffRarity.Common;
    
    [Header("Stack Info")]
    public bool canStack = false;
    public int maxStacks = 1;
}

public enum BuffType
{
    // Dice Buffs
    DiceDamage,
    DiceAttackSpeed,
    DiceHealth,
    DiceRange,
    
    // Player Buffs
    PlayerGoldMultiplier,
    PlayerXPMultiplier,
    
    // Special Buffs
    CriticalChance,
    CriticalDamage,
    Pierce,
    SplashDamage,
    
    // Utility Buffs
    DiceMoveSpeed,
    DiceCooldownReduction,
    
    // Defensive Buffs
    DamageReduction,
    HealthRegen,
    Shield
}

public enum BuffRarity
{
    Common,     // Xanh lá
    Uncommon,   // Xanh dương
    Rare,       // Tím
    Epic,       // Cam
    Legendary   // Vàng
}
