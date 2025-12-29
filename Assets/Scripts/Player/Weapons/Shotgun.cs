using UnityEngine;

public class Shotgun : Weapon
{
    int pelletCount = 5;
    float spreadAngle = 10f;

    public override void Fire(Transform fPoint)
    {
        firePoint = fPoint;

        if (fireDelay <= 0f)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                Quaternion spread = Quaternion.Euler(
                    firePoint.eulerAngles.x,
                    firePoint.eulerAngles.y + Random.Range(-spreadAngle, spreadAngle),
                    firePoint.eulerAngles.z
                );

                PoolManager.Instance.SpawnFromPool("TestBullet", firePoint.position, spread);
            }
            fireDelay = fireRate * 5;
        }
    }
}
