using UnityEngine;

public class DiggingSystem : MonoBehaviour
{
    [Header("Settings")]
    public float digRange = 3.5f;
    public LayerMask diggableLayer;
    public float[] baseDigTimes = { 1f, 2f, 4f, 8f, 16f, 32f };
    public float toolMultiplier = 1f; // Added this line

    [Header("Current Dig")]
    [SerializeField] private float currentDigTime;
    [SerializeField] private float totalDigTime;
    [SerializeField] private GameObject currentBlock;
    [SerializeField] private int currentTier;
    private bool isDigging;

    // Event setup points
    public System.Action OnDigStarted;
    public System.Action OnDigCompleted;

    // Added this method to fix the error
    public void UpgradeTool(float multiplier)
    {
        toolMultiplier = multiplier;
        Debug.Log($"Tool upgraded to {multiplier}x efficiency");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryStartDigging();
        }

        if (isDigging)
        {
            if (Input.GetKey(KeyCode.E))
            {
                ContinueDigging();
            }
            else
            {
                StopDigging();
            }
        }
    }

    private void TryStartDigging()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, digRange, diggableLayer))
        {
            string hitTag = hit.collider.tag;
            if (hitTag.StartsWith("Tier") && hitTag.EndsWith("Block"))
            {
                currentBlock = hit.collider.gameObject;
                currentTier = int.Parse(hitTag.Replace("Tier", "").Replace("Block", ""));
                StartDigging();
            }
        }
    }

    private void StartDigging()
    {
        isDigging = true;
        currentDigTime = 0f;
        totalDigTime = baseDigTimes[currentTier - 1] / toolMultiplier; // Modified this line
        OnDigStarted?.Invoke();
    }

    private void ContinueDigging()
    {
        currentDigTime += Time.deltaTime;

        if (currentDigTime >= totalDigTime)
        {
            CompleteDigging();
        }
    }

    private void CompleteDigging()
    {
        OnDigCompleted?.Invoke();
        StopDigging();
    }

    private void StopDigging()
    {
        isDigging = false;
        currentBlock = null;
    }
}