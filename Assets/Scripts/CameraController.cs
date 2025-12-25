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
    public ObservableProperty<bool> isMenuOpen { get; private set; } = new();

    void Start()
    {
        LockCursor();

        xRotation = 0f;
        yRotation = 0f;
        cameraTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        isMenuOpen.Value = false;
        isMenuOpen.Subscribe(OnMenuOpenChanged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpen.Value = !isMenuOpen.Value;
        }

        if (isMenuOpen.Value == false)
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

    void OpenMenu()
    {
        isMenuOpen.Value = true;
        UnlockCursor();
        // UI 매니저에서 메뉴 패널 활성화
    }

    void CloseMenu()
    {
        isMenuOpen.Value = false;
        LockCursor();
        // UI 매니저에서 메뉴 패널 비활성화
    }

    void OnMenuOpenChanged(bool isOpened)
    {
        if (isOpened == true)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }
}