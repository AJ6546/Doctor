using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FixedJoystick fixedJoystick;
    [SerializeField] UIAssigner uiAssigner;
    [SerializeField] FixedButton jumpButton, invntoryButton, controlsButton;
    [SerializeField] KeyCode jumpKeboardButton = KeyCode.Space, inventoryKeyboardButton=KeyCode.I, controlsKeyboardButton = KeyCode.G;
    [SerializeField] FixedTouchField touchField;

    [SerializeField] ThirdPersonUserControl control;

    [SerializeField] Camera camera, minimapCamera;
    [SerializeField] float cameraAngle, cameraSpeed = 0.2f, rotOffset, touchRate = 0.01f;
    [SerializeField] Vector3 cameraOffset;

    [SerializeField] GameObject inventoryUI, controlsUI,minimapUI, minimapButton;

    Slider minimapSlider;

    void Awake()
    {
        uiAssigner = GetComponent<UIAssigner>();
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        control = GetComponent<ThirdPersonUserControl>();
        touchField = FindObjectOfType<FixedTouchField>();
    }

    void Update()
    {
        // Build Index is 2 we don't need player movement as this scene only plays a video of the gameplay
        if (SceneManager.GetActiveScene().buildIndex == 2) return;

        // if any of the fixed buttons or camera or minimap camera or minimap slider is not set set them before starting the game.
        if (jumpButton == null || invntoryButton == null || controlsButton == null || minimapSlider == null 
            || minimapCamera==null)
        {
            jumpButton = uiAssigner.GetFixedButtons()[0];
            invntoryButton = uiAssigner.GetFixedButtons()[1];
            controlsButton = uiAssigner.GetFixedButtons()[5];
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            // setting minimap to active in the beginning
            minimapUI.SetActive(true);
            minimapCamera = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Camera>();
            minimapSlider = FindObjectOfType<Slider>();
        }

        // if player falls off on level 2 he dies after hitting a specific height
        if(transform.position.y<=-20f)
        {
            GetComponent<Health>().Die();
        }

        // Zoom in and out on the minimap
        minimapCamera.orthographicSize = minimapSlider.value;

        // Camera follow and touch camera look around in x axis
        cameraAngle += touchField.TouchDist.x * cameraSpeed;
        camera.transform.position = transform.position + Quaternion.AngleAxis(cameraAngle, Vector3.up) * cameraOffset;
        camera.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * rotOffset - 
            camera.transform.position, Vector3.up);

        // Touch camera look around in y axis
        if (touchField.TouchDist.x == 0)
        { cameraOffset.y -= touchField.TouchDist.y * touchRate; }

        // If Player is in middle of conversation movement and other actions are switched off
        if (GetComponent<PlayerConversant>().IsTalking()) return;

        // If Player is Saving the game movement and other actions are switched off
        if (FindObjectOfType<OnlineSaveLoadManager>().IsSaving()) return;

        bool inventory = Input.GetKey(inventoryKeyboardButton) || invntoryButton.Pressed;
        bool controls = Input.GetKey(controlsKeyboardButton) || controlsButton.Pressed;

        // If player is dead return here to avoid any further movement
        if (GetComponent<Health>().IsDead()) return;

        // Player Jump
        control.m_Jump = Input.GetKey(jumpKeboardButton) || jumpButton.Pressed;

        // Player Movement
        control.hInput = Input.GetAxis("Horizontal") + fixedJoystick.Horizontal;
        control.vInput = Input.GetAxis("Vertical") + fixedJoystick.Vertical;

        if(inventory)
        {
            inventoryUI.SetActive(true); // Opening Inventory
        }
        if(controls)
        {
            controlsUI.SetActive(true); // Opening Controls
        }
        if(minimapUI.active)
        {
            minimapButton.SetActive(false); // Setting minimap Button to inactive when minimap is open
        }
        else
        {
            minimapButton.SetActive(true); // Setting minimap Button to active when minimap is closed/collapsed
        }
    }
    public void OpenMinimap()
    {
        minimapUI.SetActive(true); // Setting minimap to active
    }
}
