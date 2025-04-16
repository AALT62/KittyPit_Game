using UnityEngine;
using UnityEngine.Events;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    [System.Serializable]
    public class InventoryUpdateEvent : UnityEvent<int, int> { }

    [Header("Inventory Settings")]
    public int[] resourceCounts = new int[6];
    public int[] resourceValues = { 1, 3, 10, 30, 100, 300 };
    public int baseCapacity = 20;

    [Header("Events")]
    public InventoryUpdateEvent OnInventoryUpdated = new InventoryUpdateEvent();

    private int currentCapacity;
    private int currency;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentCapacity = baseCapacity;
    }

    public void AddResource(int tier, int amount)
    {
        if (GetTotalResources() + amount <= currentCapacity)
        {
            resourceCounts[tier - 1] += amount;
            OnInventoryUpdated.Invoke(tier - 1, resourceCounts[tier - 1]);
        }
    }

    public void SellAllResources()
    {
        for (int i = 0; i < resourceCounts.Length; i++)
        {
            currency += resourceCounts[i] * resourceValues[i];
            resourceCounts[i] = 0;
            OnInventoryUpdated.Invoke(i, 0);
        }
    }

    public bool CanAfford(int amount)
    {
        return currency >= amount;
    }

    public void SpendCurrency(int amount)
    {
        currency -= amount;
    }

    public void UpgradeCapacity(int newCapacity)
    {
        currentCapacity = newCapacity;
    }

    private int GetTotalResources()
    {
        int total = 0;
        foreach (int count in resourceCounts)
        {
            total += count;
        }
        return total;
    }

    public int GetCurrency() => currency;
}