using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager Instance;
    
    [Header("Level System")]
    [SerializeField] private int currentLevel = 1;  
    [SerializeField] private float currentXP = 0f;
    [SerializeField] private float xpToNextLevel = 100f;
    [SerializeField] private float xpMultiplier = 1.2f; // XP cần cho level tiếp theo tăng theo tỷ lệ này
    
    // Events
    public event Action<int> OnLevelUp; // int = new level
    public event Action<float, float> OnXPUpdate; // currentXP, xpToNextLevel
    public event Action OnShowBuffSelection; // Khi cần hiển thị buff selection UI
    
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
        currentLevel = 1;
        currentXP = 0f;
        xpToNextLevel = 100f;
        
        OnXPUpdate?.Invoke(currentXP, xpToNextLevel);
    }
    
    public void AddXP(float xpAmount)
    {
        currentXP += xpAmount;
        
        // Kiểm tra level up
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        
        OnXPUpdate?.Invoke(currentXP, xpToNextLevel);
    }
    
    private void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel; // Giữ lại XP thừa
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpMultiplier); // Tăng XP cần cho level tiếp theo
        
        Debug.Log($"Level Up! New Level: {currentLevel}");
        
        OnLevelUp?.Invoke(currentLevel);
        OnShowBuffSelection?.Invoke(); // Hiển thị UI chọn buff
    }
    
    // Getters
    public int GetCurrentLevel() => currentLevel;
    public float GetCurrentXP() => currentXP;
    public float GetXPToNextLevel() => xpToNextLevel;
    public float GetXPProgress() => currentXP / xpToNextLevel;
    
    [Button("Add 50 XP")]
    private void TestAddXP()
    {
        AddXP(50f);
    }
    
    [Button("Force Level Up")]
    private void TestLevelUp()
    {
        currentXP = xpToNextLevel;
        AddXP(0f);
    }
}
