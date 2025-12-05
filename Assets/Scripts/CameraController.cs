using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera tpsCam;
    public CinemachineVirtualCamera fpsCam;

    private bool isAiming = false;

    void Update()
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

}
