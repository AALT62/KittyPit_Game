using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public float gameSpeed = 1f;
    public bool isPaused = false;

    [Header("Player References")]
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private GameObject currentPlayer;

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

        InitializeGame();
    }

    private void InitializeGame()
    {
        SpawnPlayer();
        Time.timeScale = gameSpeed;
    }

    private void SpawnPlayer()
    {
        if (currentPlayer != null) Destroy(currentPlayer);
        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : gameSpeed;
    }
}