using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject prefabToPool;
    public int poolSize = 10;

    private List<GameObject> pool;

    void Awake()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefabToPool);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        // Pool exhausted — return null instead of creating a new one
        return null;
    }

    private void OnApplicationQuit()
    {
        ClearPool();  // Clean up the pool when quitting the application
    }

    public void ClearPool()
    {
        foreach (var obj in pool)
        {
            Destroy(obj);
        }
        pool.Clear();
    }

}
