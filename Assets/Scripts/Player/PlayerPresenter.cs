using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    PlayerModel model;
    PlayerView view;
    CharacterController controller;

    [SerializeField] Transform playerOutlook;
    [SerializeField] float currentSpeed;

    public Transform cameraTarget;
    [SerializeField] Transform firePoint;

    bool canControll = true;

    void Awake()
    {
        model = new PlayerModel();
        view = GetComponent<PlayerView>();
        controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        model.CurHp.Value = model.Hp;
        model.CurHp.Subscribe(OnHpChanged);
    }

    // Update is called once per frame
    void Update()
    {
        if (canControll)
        {
            HandleMovement();
            HandleAnimation();

            // 테스트용 총알 발사 로직
            if (Input.GetMouseButtonDown(0))
            {
                PoolManager.Instance.SpawnFromPool("TestBullet", firePoint.position, firePoint.rotation);
            }

            // 테스트용 TakeDamage
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TakeDamage(10f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canControll = !canControll;
        }
    }

    // 플레이어 캐릭터 이동 메서드
    public void HandleMovement()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v);

        // 카메라의 수평 방향만 추출
        Vector3 camForward = cameraTarget.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTarget.right;
        camRight.y = 0;
        camRight.Normalize();

        // 입력을 카메라 기준으로 변환
        Vector3 moveDir = camForward * v + camRight * h;

        // 속도 갱신 (입력 크기 기준)
        currentSpeed = moveDir.magnitude;

        // 이동 방향 정규화
        if (moveDir.sqrMagnitude > 0.01f)
            moveDir.Normalize();

        // 중력 처리
        if (controller.isGrounded && model.Velocity.y < 0)
            model.Velocity.y = -2f;

        model.Velocity.y += model.Gravity * Time.deltaTime;

        // 최종 이동 벡터 = 이동 + 중력
        Vector3 finalMove = moveDir * model.MoveSpeed + new Vector3(0, model.Velocity.y, 0);

        // CharacterController에 적용
        controller.Move(finalMove * Time.deltaTime);

        // 점프 처리
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
            model.Velocity.y = Mathf.Sqrt(model.JumpHeight * -2f * model.Gravity);
    }

    void Rotate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v);

        if (inputDir.sqrMagnitude > 0.01f)
        {
            // 카메라의 수평 방향만 추출
            Vector3 camForward = cameraTarget.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = cameraTarget.right;
            camRight.y = 0;
            camRight.Normalize();

            // 입력을 카메라 기준으로 변환 (yaw만 반영)
            Vector3 lookDir = camForward * v + camRight * h;
            lookDir.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            playerOutlook.rotation = Quaternion.Slerp(playerOutlook.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    public void HandleAnimation()
    {
        view.Animator.SetFloat("Speed", currentSpeed);
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
            view.Animator.SetTrigger("Jump");
    }

    public void TakeDamage(float damage)
    {
        model.CurHp.Value = Mathf.Max(model.CurHp.Value - damage, 0);
    }

    // ObservableProperty 구독 메서드
    void OnHpChanged(float newHp)
    {
        float normalizedHp = newHp / model.Hp;
        view.SetHpBar(normalizedHp);
        Debug.Log($"현재 HP : {newHp}\n현재 HP 비율 : {normalizedHp}");
    }
}
