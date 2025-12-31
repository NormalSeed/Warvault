using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : PooledObject
{
    [SerializeField] float lifeTime = 2.0f;
    [SerializeField] int damage = 10;
    float timer;

    public override void OnSpawn()
    {
        timer = 0f;
        GetComponent<Rigidbody>().velocity = transform.forward * 5f;
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
        if (other.CompareTag("Player"))
        {
            PlayerPresenter player = other.GetComponent<PlayerPresenter>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
            ReturnPool();
        }
    }
}
