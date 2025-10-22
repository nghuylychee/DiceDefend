using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;
    
    [Header("Buff System")]
    [SerializeField] private List<BuffConfig> allBuffs = new List<BuffConfig>();
    [SerializeField] private List<ActiveBuff> activeBuffs = new List<ActiveBuff>();
    
    [Header("Buff Selection")]
    [SerializeField] private int buffSelectionCount = 3; // Số buff để chọn khi level up
    
    public event Action<List<BuffConfig>> OnShowBuffSelection; // Hiển thị UI chọn buff
    public event Action<BuffConfig> OnBuffApplied; // Khi buff được áp dụng
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        Init();
    }
    
    public void Init()
    {
        activeBuffs.Clear();
        LoadAllBuffs();
    }
    
    private void LoadAllBuffs()
    {
        // Load tất cả BuffConfig từ Resources hoặc từ thư mục Config
        allBuffs.Clear();
        
        // Tạm thời tạo một số buff mẫu
        CreateSampleBuffs();
    }
    
    private void CreateSampleBuffs()
    {
        // Tạo buff mẫu để test
        var damageBuff = CreateBuffConfig("Dice Damage", "Tăng damage của tất cả dice", BuffType.DiceDamage, 10f, 0f, 0f, BuffRarity.Common);
        var speedBuff = CreateBuffConfig("Attack Speed", "Tăng tốc độ tấn công", BuffType.DiceAttackSpeed, 0.2f, 0f, 0f, BuffRarity.Uncommon);
        var healthBuff = CreateBuffConfig("Dice Health", "Tăng máu của dice", BuffType.DiceHealth, 20f, 0f, 0f, BuffRarity.Common);
        
        allBuffs.Add(damageBuff);
        allBuffs.Add(speedBuff);
        allBuffs.Add(healthBuff);
    }
    
    private BuffConfig CreateBuffConfig(string name, string desc, BuffType type, float val1, float val2, float val3, BuffRarity rarity)
    {
        var buff = ScriptableObject.CreateInstance<BuffConfig>();
        buff.buffID = allBuffs.Count;
        buff.buffName = name;
        buff.buffDescription = desc;
        buff.buffType = type;
        buff.value1 = val1;
        buff.value2 = val2;
        buff.value3 = val3;
        buff.rarity = rarity;
        return buff;
    }
    
    public void ShowBuffSelection()
    {
        var availableBuffs = GetRandomBuffs(buffSelectionCount);
        OnShowBuffSelection?.Invoke(availableBuffs);
    }
    
    private List<BuffConfig> GetRandomBuffs(int count)
    {
        // Lọc buff theo rarity và level hiện tại
        var playerLevel = PlayerLevelManager.Instance.GetCurrentLevel();
        var filteredBuffs = FilterBuffsByLevel(allBuffs, playerLevel);
        
        // Random chọn buff
        var shuffledBuffs = filteredBuffs.OrderBy(x => UnityEngine.Random.value).ToList();
        return shuffledBuffs.Take(count).ToList();
    }
    
    private List<BuffConfig> FilterBuffsByLevel(List<BuffConfig> buffs, int playerLevel)
    {
        var filteredBuffs = new List<BuffConfig>();
        
        foreach (var buff in buffs)
        {
            // Logic lọc buff theo level (có thể customize)
            bool canShow = true;
            
            switch (buff.rarity)
            {
                case BuffRarity.Common:
                    canShow = playerLevel >= 1;
                    break;
                case BuffRarity.Uncommon:
                    canShow = playerLevel >= 3;
                    break;
                case BuffRarity.Rare:
                    canShow = playerLevel >= 5;
                    break;
                case BuffRarity.Epic:
                    canShow = playerLevel >= 8;
                    break;
                case BuffRarity.Legendary:
                    canShow = playerLevel >= 12;
                    break;
            }
            
            if (canShow)
                filteredBuffs.Add(buff);
        }
        
        return filteredBuffs;
    }
    
    public void ApplyBuff(BuffConfig buffConfig)
    {
        // Kiểm tra xem buff đã tồn tại chưa
        var existingBuff = activeBuffs.FirstOrDefault(b => b.buffConfig.buffType == buffConfig.buffType);
        
        if (existingBuff != null)
        {
            if (buffConfig.canStack && existingBuff.stackCount < buffConfig.maxStacks)
            {
                // Stack buff
                existingBuff.stackCount++;
                Debug.Log($"Stacked {buffConfig.buffName} (Stack: {existingBuff.stackCount})");
            }
            else
            {
                Debug.Log($"Buff {buffConfig.buffName} already exists and cannot stack");
                return;
            }
        }
        else
        {
            // Thêm buff mới
            var newActiveBuff = new ActiveBuff
            {
                buffConfig = buffConfig,
                stackCount = 1,
                appliedTime = Time.time
            };
            activeBuffs.Add(newActiveBuff);
            Debug.Log($"Applied buff: {buffConfig.buffName}");
        }
        
        OnBuffApplied?.Invoke(buffConfig);
        
        // Refresh tất cả dice để áp dụng buff mới
        RefreshAllDiceBuffs();
    }
    
    public float GetBuffValue(BuffType buffType, float baseValue)
    {
        var buff = activeBuffs.FirstOrDefault(b => b.buffConfig.buffType == buffType);
        if (buff != null)
        {
            return baseValue + (buff.buffConfig.value1 * buff.stackCount);
        }
        return baseValue;
    }
    
    public float GetBuffMultiplier(BuffType buffType, float baseMultiplier = 1f)
    {
        var buff = activeBuffs.FirstOrDefault(b => b.buffConfig.buffType == buffType);
        if (buff != null)
        {
            return baseMultiplier + (buff.buffConfig.value1 * buff.stackCount);
        }
        return baseMultiplier;
    }
    
    public List<ActiveBuff> GetActiveBuffs()
    {
        return new List<ActiveBuff>(activeBuffs);
    }
    
    private void RefreshAllDiceBuffs()
    {
        // Refresh tất cả dice để áp dụng buff mới
        var allDice = FindObjectsOfType<Dice>();
        foreach (var dice in allDice)
        {
            dice.RefreshBuffs();
        }
    }
    
    [Button("Test Show Buff Selection")]
    private void TestShowBuffSelection()
    {
        ShowBuffSelection();
    }
    
    [Button("Test Apply Random Buff")]
    private void TestApplyRandomBuff()
    {
        if (allBuffs.Count > 0)
        {
            var randomBuff = allBuffs[UnityEngine.Random.Range(0, allBuffs.Count)];
            ApplyBuff(randomBuff);
        }
    }
}

[System.Serializable]
public class ActiveBuff
{
    public BuffConfig buffConfig;
    public int stackCount;
    public float appliedTime;
}
