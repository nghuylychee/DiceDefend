using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelUpUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI xpText;
    
    [Header("Buff Selection")]
    [SerializeField] private Transform buffContainer;
    [SerializeField] private GameObject buffButtonPrefab;
    [SerializeField] private List<BuffSelectionButton> buffButtons = new List<BuffSelectionButton>();
    
    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.3f;
    
    private void Start()
    {
        Init();
    }
    
    public void Init()
    {
        levelUpPanel.SetActive(false);
        
        // Subscribe to events
        PlayerLevelManager.Instance.OnXPUpdate += UpdateXPUI;
        PlayerLevelManager.Instance.OnLevelUp += OnLevelUp;
        BuffManager.Instance.OnShowBuffSelection += ShowBuffSelection;
    }
    
    private void UpdateXPUI(float currentXP, float xpToNextLevel)
    {
        xpSlider.value = currentXP / xpToNextLevel;
        xpText.text = $"{currentXP:F0} / {xpToNextLevel:F0}";
    }
    
    private void OnLevelUp(int newLevel)
    {
        levelText.text = $"Level {newLevel}";
        
        // Animation cho level up
        levelText.transform.localScale = Vector3.zero;
        levelText.transform.DOScale(1.2f, animationDuration).SetEase(Ease.OutBack);
    }
    
    private void ShowBuffSelection(List<BuffConfig> availableBuffs)
    {
        levelUpPanel.SetActive(true);
        
        // Clear existing buttons
        foreach (var button in buffButtons)
        {
            if (button != null)
                Destroy(button.gameObject);
        }
        buffButtons.Clear();
        
        // Create buff selection buttons
        for (int i = 0; i < availableBuffs.Count; i++)
        {
            var buffConfig = availableBuffs[i];
            var buttonObj = Instantiate(buffButtonPrefab, buffContainer);
            var buffButton = buttonObj.GetComponent<BuffSelectionButton>();
            
            if (buffButton != null)
            {
                buffButton.Init(buffConfig, i);
                buffButtons.Add(buffButton);
                
                // Animation entrance
                buttonObj.transform.localScale = Vector3.zero;
                buttonObj.transform.DOScale(1f, animationDuration)
                    .SetDelay(i * 0.1f)
                    .SetEase(Ease.OutBack);
            }
        }
        
        // Pause game
        Time.timeScale = 0f;
    }
    
    public void OnBuffSelected(BuffConfig selectedBuff)
    {
        // Apply buff
        BuffManager.Instance.ApplyBuff(selectedBuff);
        
        // Close UI
        CloseLevelUpUI();
    }
    
    private void CloseLevelUpUI()
    {
        // Animation exit
        foreach (var button in buffButtons)
        {
            if (button != null)
            {
                button.transform.DOScale(0f, animationDuration).SetEase(Ease.InBack);
            }
        }
        
        levelUpPanel.transform.DOScale(0f, animationDuration).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                levelUpPanel.SetActive(false);
                levelUpPanel.transform.localScale = Vector3.one;
                
                // Resume game
                Time.timeScale = 1f;
            });
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        if (PlayerLevelManager.Instance != null)
        {
            PlayerLevelManager.Instance.OnXPUpdate -= UpdateXPUI;
            PlayerLevelManager.Instance.OnLevelUp -= OnLevelUp;
        }
        
        if (BuffManager.Instance != null)
        {
            BuffManager.Instance.OnShowBuffSelection -= ShowBuffSelection;
        }
    }
}
