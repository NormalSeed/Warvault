using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("카메라")]
    public CinemachineVirtualCamera tpsCam;
    public CinemachineVirtualCamera fpsCam;

    [Header("바라볼 대상")]
    public Transform cameraTarget;

    [Header("회전 속도 조절 변수")]
    public float sensitivity = 100f;
    [SerializeField] float xRotation = 0f;
    [SerializeField] float yRotation = 0f;

    bool isAiming = false;

    void Start()
    {
        LockCursor();

        xRotation = 0f;
        yRotation = 0f;
        cameraTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        UIManager.Instance.isMenuOpen.Subscribe(OnMenuOpenChanged);
    }

    void Update()
    {
        if (UIManager.Instance.isMenuOpen.Value == false)
        {
            if (Input.GetMouseButtonDown(1)) // 우클릭 누름
            {
                isAiming = true;
                SwitchCamera(isAiming);
            }

            if (Input.GetMouseButtonUp(1)) // 우클릭 뗌
            {
                isAiming = false;
                SwitchCamera(isAiming);
            }

            RotateCamera();
        }
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 60f); // 위아래 회전 제한 

        cameraTarget.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void SwitchCamera(bool aiming)
    {
        if (aiming)
        {
            fpsCam.Priority = 20;
            tpsCam.Priority = 10;
        }
        else
        {
            fpsCam.Priority = 10;
            tpsCam.Priority = 20;
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnMenuOpenChanged(bool isOpened)
    {
        if (isOpened == true)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }
}