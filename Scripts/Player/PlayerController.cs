using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FixedJoystick fixedJoystick;
    [SerializeField] UIAssigner uiAssigner;
    [SerializeField] FixedButton jumpButton, invntoryButton;

    [SerializeField] FixedTouchField touchField;

    [SerializeField] ThirdPersonUserControl control;

    [SerializeField] Camera camera;
    [SerializeField] float cameraAngle, cameraSpeed = 0.2f, rotOffset, touchRate = 0.01f;
    [SerializeField] Vector3 cameraOffset;

    [SerializeField] GameObject inventoryUI;


    void Awake()
    {
        uiAssigner = GetComponent<UIAssigner>();
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        control = GetComponent<ThirdPersonUserControl>();
        touchField = FindObjectOfType<FixedTouchField>();
    }

    void Update()
    {
        if (jumpButton == null || invntoryButton == null)
        {
            jumpButton = uiAssigner.GetFixedButtons()[0];
            invntoryButton = uiAssigner.GetFixedButtons()[1];
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        cameraAngle += touchField.TouchDist.x * cameraSpeed;
        camera.transform.position = transform.position + Quaternion.AngleAxis(cameraAngle, Vector3.up) * cameraOffset;
        camera.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * rotOffset - camera.transform.position, Vector3.up);
        if (touchField.TouchDist.x == 0)
        { cameraOffset.y -= touchField.TouchDist.y * touchRate; }
        control.m_Jump = Input.GetKey("space") || jumpButton.Pressed;
        bool inventory = Input.GetKey("i") || invntoryButton.Pressed;
        control.hInput = Input.GetAxis("Horizontal") + fixedJoystick.Horizontal;
        control.vInput = Input.GetAxis("Vertical") + fixedJoystick.Vertical;
        if(inventory)
        {
            inventoryUI.SetActive(true);
        }
        
    }
}
