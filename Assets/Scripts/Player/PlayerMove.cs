using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 0.05f;
    public float gravity = -9.81f;
    public float jumpHeight = 0.3f;

    private CharacterController controller;
    private Vector3 velocity;

    Actions actions;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        actions = GetComponent<Actions>();
    }

    // 플레이어 캐릭터 이동 메서드
    public void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v) * moveSpeed;
        move = transform.TransformDirection(move);

        controller.Move(move);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
