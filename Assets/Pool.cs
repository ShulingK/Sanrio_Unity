using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class Pool : NetworkBehaviour
{
    public GameObject[] prefab;

    public int count;

    private List<GameObject> pool = new();

    public void Start()
    {
        if(count == -1)
        {
            foreach(var prefab in prefab)
            {
                prefab.SetActive(false);

                pool.Add(prefab);
            }
        }
        else
        {
            for (int i = 0; i < count; ++i)
            {
                Instantiate(prefab[0]);

                prefab[0].SetActive(false);

                pool.Add(prefab[0]);
            }
        }
    }

    [SyncVar(hook = nameof(HandleSpawn))]
    public Vector3 spawn;
    private void HandleSpawn(Vector3 oldValue, Vector3 newValue) 
    {
        if(pool.Count == 0) { return; }
        
        pool[0].transform.position = newValue;

        pool[0].SetActive(true);

        pool.Remove(pool[0]);
    }
}
