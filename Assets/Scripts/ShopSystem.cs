using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    [Header("UI References")]
    public GameObject ToolsContent;
    public GameObject BackpacksContent;
    public GameObject UpgradesContent;

    

    public void ShowBackpacksTab()
    {
        ToolsContent.SetActive(false);
        BackpacksContent.SetActive(true);
        UpgradesContent.SetActive(false);
    }

    public void ShowUpgradesTab()
    {
        ToolsContent.SetActive(false);
        BackpacksContent.SetActive(false);
        UpgradesContent.SetActive(true);
    }

    [Header("UI References")]
    public GameObject shopPanel;
    public Button[] toolUpgradeButtons;
    public Button[] backpackUpgradeButtons;
    public Button[] generalUpgradeButtons;
    public TMP_Text currencyText;

    [Header("Upgrade Settings")]
    public int[] toolUpgradeCosts = { 100, 250, 500, 1000 };
    public float[] toolUpgradeValues = { 1.5f, 2f, 3f, 5f };
    public int[] backpackUpgradeCosts = { 150, 400, 800 };
    public int[] backpackUpgradeValues = { 30, 50, 100 };
    public int[] generalUpgradeCosts = { 200, 500, 1000 };

    private DiggingSystem diggingSystem;

    private void Start()
    {
        diggingSystem = FindObjectOfType<DiggingSystem>();
        UpdateShopUI();
    }

    public void ToggleShop(bool open)
    {
        shopPanel.SetActive(open);
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = open ? 0f : 1f;
    }

    public void PurchaseToolUpgrade(int level)
    {
        if (InventorySystem.Instance.CanAfford(toolUpgradeCosts[level - 1]))
        {
            InventorySystem.Instance.SpendCurrency(toolUpgradeCosts[level - 1]);
            diggingSystem.UpgradeTool(level);
            UpdateShopUI();
        }
    }

    public void PurchaseBackpackUpgrade(int level)
    {
        if (InventorySystem.Instance.CanAfford(backpackUpgradeCosts[level - 1]))
        {
            InventorySystem.Instance.SpendCurrency(backpackUpgradeCosts[level - 1]);
            InventorySystem.Instance.UpgradeCapacity(backpackUpgradeValues[level - 1]);
            UpdateShopUI();
        }
    }
    public void ShowToolsTab()
    {
        ToolsContent.SetActive(true);
        BackpacksContent.SetActive(false);
        UpgradesContent.SetActive(false);
    }

    public void PurchaseGeneralUpgrade(int type)
    {
        if (InventorySystem.Instance.CanAfford(generalUpgradeCosts[type]))
        {
            InventorySystem.Instance.SpendCurrency(generalUpgradeCosts[type]);
            // Apply specific upgrade based on type
            UpdateShopUI();
        }
    }

    private void UpdateShopUI()
    {
        currencyText.text = $"${InventorySystem.Instance.GetCurrency()}";

        // Update button interactability
        for (int i = 0; i < toolUpgradeButtons.Length; i++)
        {
            toolUpgradeButtons[i].interactable = InventorySystem.Instance.CanAfford(toolUpgradeCosts[i]);
        }

        for (int i = 0; i < backpackUpgradeButtons.Length; i++)
        {
            backpackUpgradeButtons[i].interactable = InventorySystem.Instance.CanAfford(backpackUpgradeCosts[i]);
        }

        for (int i = 0; i < generalUpgradeButtons.Length; i++)
        {
            generalUpgradeButtons[i].interactable = InventorySystem.Instance.CanAfford(generalUpgradeCosts[i]);
        }
    }
}