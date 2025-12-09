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
    [SerializeField] private float xRotation = 0f;
    [SerializeField] private float yRotation = 0f;

    private bool isAiming = false;
    private bool isMenuOpen = false;

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuOpen) OpenMenu();
            else CloseMenu();
        }

        if (!isMenuOpen)
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

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 60f); // 위아래 회전 제한 

        cameraTarget.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void SwitchCamera(bool aiming)
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

    void OpenMenu()
    {
        isMenuOpen = true;
        UnlockCursor();
        // UI 매니저에서 메뉴 패널 활성화
    }

    void CloseMenu()
    {
        isMenuOpen = false;
        LockCursor();
        // UI 매니저에서 메뉴 패널 비활성화
    }
}