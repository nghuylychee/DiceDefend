using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI textItemName, textItemDesc, textItemPrice;
    public Image imageItemIcon;
    public Button buttonItemIcon;
    public int itemDiceID, itemID;
    public void Init()
    {
        itemID = this.transform.GetSiblingIndex();
        buttonItemIcon.onClick.AddListener(() => ShopManager.Instance.BuyShopItem(itemID));
    }
    public void UpdateItem(int diceID, string itemName, string itemDesc, float itemPrice, Sprite itemIcon)
    {
        itemDiceID = diceID;
        textItemName.text = itemName;
        textItemDesc.text = itemDesc;
        textItemPrice.text = itemPrice.ToString();
        imageItemIcon.sprite = itemIcon;
    }
    
}
