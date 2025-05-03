using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public ObjectPooler pooler;
    public int amountToSpawn = 10;
    public Terrain terrain;
    public float edgeValue = 20f;

    private Vector2 spawnAreaMin;
    private Vector2 spawnAreaMax;

    void Start()
    {
        SetSpawnBoundsFromTerrain(terrain);
        SpawnObjects();
    }

    void SetSpawnBoundsFromTerrain(Terrain terrain)
    {
        Vector3 terrainPos = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        spawnAreaMin = new Vector2(terrainPos.x, terrainPos.z);
        spawnAreaMax = new Vector2(terrainPos.x + terrainSize.x, terrainPos.z + terrainSize.z);

    }

    void SpawnObjects()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject obj = pooler.GetPooledObject();

            if (obj == null)
            {
                Debug.LogWarning("Pool exhausted — not enough objects to spawn!");
                continue; // Skip this spawn
            }

            Vector3 spawnPos = new Vector3(Random.Range(spawnAreaMin.x + edgeValue, spawnAreaMax.x - edgeValue), 0f, Random.Range(spawnAreaMin.y + edgeValue, spawnAreaMax.y - edgeValue));
            float terrainHeight = terrain.SampleHeight(spawnPos);
            spawnPos.y = terrainHeight + 1f;

            obj.transform.position = spawnPos;
            obj.SetActive(true);
        }
    }

}
