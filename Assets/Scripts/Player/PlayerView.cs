using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }

    public Actions Actions { get; private set; }

    [SerializeField] Image hpBar;

    void Awake()
    {
        Controller = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        Actions = GetComponentInChildren<Actions>();
    }

    public void PlayIdleAnimation()
    {
        Animator.SetFloat("Speed", 0);
    }

    public void PlayMoveAnimation(float speed)
    {
        Animator.SetFloat("Speed", speed);
    }

    public void PlayJumpAnimation()
    {
        Animator.SetBool("IsJump", true);
    }

    public void PlayAttackAnimation()
    {
        Animator.SetTrigger("Attack");
    }

    public void SetHpBar(float normalizedHp)
    {
        hpBar.DOFillAmount(normalizedHp, 0.5f).SetEase(Ease.OutCubic);
    }
}
