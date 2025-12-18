using UnityEngine;

public class TestBullet : PooledObject
{
    [SerializeField] float lifeTime = 2.0f;
    float timer;

    public override void OnSpawn()
    {
        timer = 0f;
        GetComponent<Rigidbody>().velocity = transform.forward * 10f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            ReturnPool();
        }
    }
}
