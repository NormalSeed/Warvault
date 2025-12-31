using UnityEngine;

public class TestBullet : PooledObject
{
    [SerializeField] float lifeTime = 2.0f;
    [SerializeField] int damage = 10;
    float timer;

    public override void OnSpawn()
    {
        timer = 0f;
        GetComponent<Rigidbody>().velocity = transform.forward * 30f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            ReturnPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        ReturnPool();
    }
}
