using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVcam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;
    [SerializeField]
    private int priorityBoostAmount = 10;

    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;
    
    private void Awake() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];

        thirdPersonCanvas.enabled = true;
        aimCanvas.enabled = false;
    }

    private void OnEnable() {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable() {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim(){
        virtualCamera.Priority += priorityBoostAmount;
        aimCanvas.gameObject.SetActive(true);
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }

    private void CancelAim(){
        virtualCamera.Priority -= priorityBoostAmount;
        aimCanvas.gameObject.SetActive(false);
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
