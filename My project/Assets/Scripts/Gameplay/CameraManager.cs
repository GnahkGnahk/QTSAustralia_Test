using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager> {
    [SerializeField] CinemachineVirtualCamera CM_TopDown, CM_TopDown2;

    public void SetCameraOn(CameraType cam, bool isActive = true)
    {
        CinemachineVirtualCamera tempCam = cam == CameraType.TOP_DOWN ? CM_TopDown :
                                            cam == CameraType.TOP_DOWN2 ? CM_TopDown2 : null;

        tempCam.Priority = isActive ? 99 : 1;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position += new Vector3(position.x, 0, position.z);
    }
}
