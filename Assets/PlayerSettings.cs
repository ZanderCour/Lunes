using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSettings : MonoBehaviour
{
    [Header("Camera")]
    public CinemachineFreeLook cameraSettings;
    public float CameraSensetivity;
    public float CameraFieldOfView;

    private void Awake() {
        cameraSettings.m_Lens.FieldOfView = CameraFieldOfView;
        cameraSettings.m_YAxis.m_MaxSpeed = cameraSettings.m_YAxis.m_MaxSpeed * CameraSensetivity;
        cameraSettings.m_XAxis.m_MaxSpeed = cameraSettings.m_XAxis.m_MaxSpeed * CameraSensetivity;
    }
}
