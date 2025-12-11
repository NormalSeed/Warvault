using UnityEngine;

public class PooledObject : MonoBehaviour
{
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
        OnDespawn();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnSpawn();
    }
}
