using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private PoolManager poolManager;
    private string poolName;

    public void InitPool(PoolManager manager, string name)
    {
        poolManager = manager;
        poolName = name;
    }

    // 활성화 될 때 호출
    public virtual void OnSpawn()
    {

    }

    // 풀로 돌아갈 때 호출
    public virtual void OnDespawn()
    {

    }

    // 외부에서 오브젝트 비활성화 시 호출
    public void ReturnPool()
    {
        poolManager.ReturnToPool(this, poolName);
    }

    private void OnEnable()
    {
        OnSpawn();
    }
}
