using UnityEngine;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{
    public GameObject[] blockPrefabs;
    public Transform pitArea;
    public Vector3 spawnAreaSize = new Vector3(20f, 5f, 20f);

    private List<GameObject> activeBlocks = new List<GameObject>();

    public void RemoveBlock(GameObject block)
    {
        if (activeBlocks.Contains(block))
        {
            activeBlocks.Remove(block);
            Destroy(block);
        }
    }

    private void SpawnBlock(int tier)
    {
        Vector3 randomPos = pitArea.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        GameObject block = Instantiate(blockPrefabs[tier], randomPos, Quaternion.identity, pitArea);
        activeBlocks.Add(block);
    }
}