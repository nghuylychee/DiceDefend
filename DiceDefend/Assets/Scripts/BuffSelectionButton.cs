using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BuffSelectionButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button button;
    [SerializeField] private Image buffIcon;
    [SerializeField] private TextMeshProUGUI buffNameText;
    [SerializeField] private TextMeshProUGUI buffDescriptionText;
    [SerializeField] private Image rarityBackground;
    
    [Header("Rarity Colors")]
    [SerializeField] private Color commonColor = Color.green;
    [SerializeField] private Color uncommonColor = Color.blue;
    [SerializeField] private Color rareColor = Color.magenta;
    [SerializeField] private Color epicColor = Color.yellow;
    [SerializeField] private Color legendaryColor = Color.red;
    
    private BuffConfig buffConfig;
    private int buttonIndex;
    
    private void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
    }
    
    public void Init(BuffConfig config, int index)
    {
        buffConfig = config;
        buttonIndex = index;
        
        // Set UI elements
        buffIcon.sprite = config.buffIcon;
        buffNameText.text = config.buffName;
        buffDescriptionText.text = config.buffDescription;
        
        // Set rarity color
        rarityBackground.color = GetRarityColor(config.rarity);
        
        // Add hover effect
        button.onClick.AddListener(() => OnButtonClicked());
    }
    
    private Color GetRarityColor(BuffRarity rarity)
    {
        return rarity switch
        {
            BuffRarity.Common => commonColor,
            BuffRarity.Uncommon => uncommonColor,
            BuffRarity.Rare => rareColor,
            BuffRarity.Epic => epicColor,
            BuffRarity.Legendary => legendaryColor,
            _ => Color.white
        };
    }
    
    private void OnButtonClicked()
    {
        // Animation feedback
        transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
        
        // Notify LevelUpUI
        var levelUpUI = FindObjectOfType<LevelUpUI>();
        if (levelUpUI != null)
        {
            levelUpUI.OnBuffSelected(buffConfig);
        }
    }
    
    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
