using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    [SerializeField] Transform enemyHpBarTransform;
    [SerializeField] Image enemyHpBar;
    [SerializeField] Vector3 offset = new Vector3(0f, 0f, 0f);

    private void LateUpdate()
    {
        HpBarFollow();
    }

    public void SetEnemyHpBar(float normalizedHp)
    {
        enemyHpBar.DOFillAmount(normalizedHp, 0.5f).SetEase(Ease.OutCubic);
    }

    public void HpBarLookAtTarget(Transform target)
    {
        if (target != null)
        {
            enemyHpBarTransform.LookAt(target);
        }
    }

    public void HpBarFollow()
    {
        enemyHpBarTransform.position = gameObject.transform.position;
    }
}
