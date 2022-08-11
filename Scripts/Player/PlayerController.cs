using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FixedJoystick fixedJoystick;
    [SerializeField] UIAssigner uiAssigner;
    [SerializeField] FixedButton jumpButton, invntoryButton, controlsButton;
    [SerializeField] KeyCode jumpKeboardButton = KeyCode.Space, inventoryKeyboardButton=KeyCode.I, controlsKeyboardButton = KeyCode.G;
    [SerializeField] FixedTouchField touchField;

    [SerializeField] ThirdPersonUserControl control;

    [SerializeField] Camera camera;
    [SerializeField] float cameraAngle, cameraSpeed = 0.2f, rotOffset, touchRate = 0.01f;
    [SerializeField] Vector3 cameraOffset;

    [SerializeField] GameObject inventoryUI, controlsUI;


    void Awake()
    {
        uiAssigner = GetComponent<UIAssigner>();
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        control = GetComponent<ThirdPersonUserControl>();
        touchField = FindObjectOfType<FixedTouchField>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) return;
        if (jumpButton == null || invntoryButton == null || controlsButton==null)
        {
            jumpButton = uiAssigner.GetFixedButtons()[0];
            invntoryButton = uiAssigner.GetFixedButtons()[1];
            controlsButton = uiAssigner.GetFixedButtons()[5];
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        if(transform.position.y<=-20f)
        {
            GetComponent<Health>().Die();
        }
        cameraAngle += touchField.TouchDist.x * cameraSpeed;
        camera.transform.position = transform.position + Quaternion.AngleAxis(cameraAngle, Vector3.up) * cameraOffset;
        camera.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * rotOffset - camera.transform.position, Vector3.up);
        if (touchField.TouchDist.x == 0)
        { cameraOffset.y -= touchField.TouchDist.y * touchRate; }
        if (GetComponent<PlayerConversant>().IsTalking()) return;
        control.m_Jump = Input.GetKey(jumpKeboardButton) || jumpButton.Pressed;
        bool inventory = Input.GetKey(inventoryKeyboardButton) || invntoryButton.Pressed;
        bool controls = Input.GetKey(controlsKeyboardButton) || controlsButton.Pressed;
        if (GetComponent<Health>().IsDead()) return;
        control.hInput = Input.GetAxis("Horizontal") + fixedJoystick.Horizontal;
        control.vInput = Input.GetAxis("Vertical") + fixedJoystick.Vertical;
        if(inventory)
        {
            inventoryUI.SetActive(true);
        }
        if(controls)
        {
            controlsUI.SetActive(true);
        }
    }
}
