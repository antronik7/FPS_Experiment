using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("ADS")]
    [SerializeField] Transform gun;
    [SerializeField] AnimationCurve ADS_Curve;
    [SerializeField] float ADS_AnimDuration;
    [SerializeField] Vector3 Idle_Position;
    [SerializeField] Vector3 ADS_Position;
    [SerializeField] float Base_FOV;
    [SerializeField] float ADS_FOV;

    private bool doADS = false;
    private float ADS_CurrentTime = 0;
    private int ADS_AnimDirection = -1;



    private PlayerControls input = null;

    void Awake()
    {
        input = new PlayerControls();
    }

    private void OnEnable()
    {
        input.Enable();
        input.InputLayout.ADS.performed += OnADSPerformed;
        input.InputLayout.ADS.canceled += OnADSCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.InputLayout.ADS.performed -= OnADSPerformed;
        input.InputLayout.ADS.canceled -= OnADSCanceled;
    }

    private void OnADSPerformed(InputAction.CallbackContext value)
    {
        StartAimDownSight(true);
    }

    private void OnADSCanceled(InputAction.CallbackContext value)
    {
        StartAimDownSight(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ADS
        if (doADS)
            AimDownSight();
    }

    void StartAimDownSight(bool value)
    {
        doADS = true;
        if (value)
            ADS_AnimDirection = 1;
        else
            ADS_AnimDirection = -1;
    }

    void AimDownSight()
    {
        ADS_CurrentTime += (Time.deltaTime * ADS_AnimDirection);

        gun.transform.position = Vector3.Lerp(Idle_Position, ADS_Position, ADS_Curve.Evaluate(ADS_CurrentTime/ADS_AnimDuration));
        Camera.main.fieldOfView = Mathf.Lerp(Base_FOV, ADS_FOV, ADS_Curve.Evaluate(ADS_CurrentTime / ADS_AnimDuration));

        if (ADS_CurrentTime > ADS_AnimDuration || ADS_CurrentTime < 0)
        {
            doADS = false;
            ADS_CurrentTime = Mathf.Clamp(ADS_CurrentTime, 0, ADS_AnimDuration);
        }
    }
}
