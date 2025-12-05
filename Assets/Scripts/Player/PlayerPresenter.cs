using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    private PlayerModel model;
    private PlayerView view;
    private PlayerMove move;
    private CharacterController controller;

    [SerializeField] private Transform playerOutlook;
    [SerializeField] private float currentSpeed;

    void Awake()
    {
        model = new PlayerModel();
        view = GetComponent<PlayerView>();
        move = GetComponent<PlayerMove>();
        controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    // 플레이어 캐릭터 이동 메서드
    public void HandleMovement()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v);
        Vector3 move = direction * model.MoveSpeed;
        currentSpeed = direction.magnitude;
        move = transform.TransformDirection(move);

        controller.Move(move);

        if (controller.isGrounded && model.Velocity.y < 0)
        {
            Vector3 velocity = model.Velocity;
            velocity.y = -2f;
            model.Velocity = velocity;
        }

        model.Velocity.y += model.Gravity * Time.deltaTime;
        controller.Move(model.Velocity * Time.deltaTime);

        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            model.Velocity.y = Mathf.Sqrt(model.JumpHeight * -2f * model.Gravity);
        }
    }

    private void Rotate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v);

        if (direction.sqrMagnitude > 0.01f)
        {
            
            direction.y = 0f; // 수직 회전 제거

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerOutlook.rotation = Quaternion.Slerp(playerOutlook.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    public void HandleAnimation()
    {
        view.Animator.SetFloat("Speed", currentSpeed);
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
            view.Animator.SetTrigger("Jump");
    }
}
