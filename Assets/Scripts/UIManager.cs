using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Digging UI")]
    public TMP_Text digStatusText;
    public TMP_Text digTimerText;
    public Image digProgressBar;

    [Header("Inventory UI")]
    public TMP_Text[] resourceTexts = new TMP_Text[6]; // For tiers 1-6
    public TMP_Text currencyText;

    [Header("Shop UI")]
    public GameObject shopPanel;
    public GameObject toolsTab;
    public GameObject backpacksTab;
    public GameObject upgradesTab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateDigStatus(bool isDigging, int tier, float currentTime, float totalTime)
    {
        if (!isDigging)
        {
            digStatusText.text = "Nothing to dig!";
            digTimerText.text = "";
            digProgressBar.gameObject.SetActive(false);
            return;
        }

        digStatusText.text = $"Digging: Tier {tier} Block";
        digTimerText.text = $"{currentTime:F1}s / {totalTime:F1}s";
        digProgressBar.gameObject.SetActive(true);
        digProgressBar.fillAmount = currentTime / totalTime;
    }

    public void UpdateInventoryUI(int[] resourceCounts, int currency)
    {
        for (int i = 0; i < resourceTexts.Length; i++)
        {
            resourceTexts[i].text = $"Tier {i + 1}: {resourceCounts[i]}";
        }
        currencyText.text = $"${currency}";
    }

    public void ToggleShop(bool show)
    {
        shopPanel.SetActive(show);
        if (show) ShowToolsTab(); // Default to tools tab
    }

    public void ShowToolsTab()
    {
        toolsTab.SetActive(true);
        backpacksTab.SetActive(false);
        upgradesTab.SetActive(false);
    }

    public void ShowBackpacksTab()
    {
        toolsTab.SetActive(false);
        backpacksTab.SetActive(true);
        upgradesTab.SetActive(false);
    }

    public void ShowUpgradesTab()
    {
        toolsTab.SetActive(false);
        backpacksTab.SetActive(false);
        upgradesTab.SetActive(true);
    }

    public void UpdateButtonStates(int currency, int[] toolPrices, int[] backpackPrices, int[] upgradePrices)
    {
        // Update interactability based on currency
        for (int i = 0; i < toolPrices.Length; i++)
        {
            Button toolButton = toolsTab.transform.GetChild(i).GetComponent<Button>();
            toolButton.interactable = currency >= toolPrices[i];
        }

        // Similar logic for backpacks and upgrades...
    }
}