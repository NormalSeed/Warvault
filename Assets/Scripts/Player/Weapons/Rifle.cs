using UnityEngine;

public class Rifle : Weapon
{
    public override void Fire(Transform fPoint)
    {
        firePoint = fPoint;

        if (fireDelay <= 0f)
        {
            PoolManager.Instance.SpawnFromPool("TestBullet", firePoint.position, firePoint.rotation);
            fireDelay = fireRate;
        }
    }
}
