using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public Transform firePoint;
    [SerializeField] protected float fireRate = 0.2f;
    public float fireDelay;

    protected virtual void Update()
    {
        if (fireDelay > 0f)
            fireDelay -= Time.deltaTime;
    }

    public abstract void Fire(Transform fPoint);
}
