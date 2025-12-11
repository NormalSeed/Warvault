using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("Pool Info")]
    [SerializeField] List<ObjectPool> pools;
    private Dictionary<string, List<PooledObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, List<PooledObject>>();

        foreach (ObjectPool pool in pools)
        {
            List<PooledObject> list = new List<PooledObject>();
            poolDictionary.Add(pool.name, list);
            AddPoolObject(pool.name);
        }
    }

    void AddPoolObject(string name)
    {
        ObjectPool pool = pools.Find(obj => obj.name == name);

        // (풀사이즈만큼 반복) 오브젝트 생성 -> 비활성화 -> 풀에 추가
        for (int i = 0; i < pool.size; i++)
        {
            PooledObject pooledObj = Instantiate(pool.prefab, pool.parentTransform).GetComponent<PooledObject>();
            pooledObj.gameObject.SetActive(false);
            
            poolDictionary[name].Add(pooledObj);
        }
    }

    public PooledObject SpawnFromPool(string name)
    {
        if (!poolDictionary.ContainsKey(name))
            return null;

        PooledObject poolObject = null;

        for (int i = 0; i < poolDictionary[name].Count; i++)
        {
            if (!poolDictionary[name][i].gameObject.activeSelf)
            {
                poolObject = poolDictionary[name][i];
                break;
            }

            if (i == poolDictionary[name].Count - 1)
            {
                AddPoolObject(name);
                poolObject = poolDictionary[name][i + 1];
            }
        }

        poolObject.gameObject.SetActive(true);
        poolObject.OnSpawn();

        return poolObject;
    }
}
