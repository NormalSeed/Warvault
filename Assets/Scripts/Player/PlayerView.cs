using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }

    void Awake()
    {
        Controller = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
    }

    public void PlayMoveAnimation(float speed)
    {
        Animator.SetFloat("Speed", speed);
    }

    public void PlayJumpAnimation()
    {
        Animator.SetTrigger("Jump");
    }
}
