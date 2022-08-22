using System.Collections;
using TMPro;
using UnityEngine;

public class Shrink : MonoBehaviour, ISaveable
{
    [SerializeField] bool shrink,startTimer,enlarge;
    [SerializeField] ThirdPersonCharacter tpc;
    Vector3 newSize = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 originalScale = new Vector3(1, 1, 1);
    CooldownTimer cdTimer;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float jumpPower;
    [SerializeField] KeyCode shrinkButton = KeyCode.C;
    void Start()
    {
        cdTimer = GetComponent<CooldownTimer>();
        tpc = GetComponent<ThirdPersonCharacter>();
        jumpPower = tpc.GetJumpPower();
    }
    void Update()
    {
        // If Player is dead he cannot shrink
        if (GetComponent<Health>().IsDead()) return;
        // If Player is in the middle of a conversation He cannot shrink
        if (GetComponent<PlayerConversant>().IsTalking()) return;
        // If Player is Saving the game, He cannot shrink
        if (FindObjectOfType<OnlineSaveLoadManager>().IsSaving()) return;
   
        if (shrink)
        {
            // reducing size of player
            transform.localScale = Vector3.Lerp(transform.localScale, newSize, Time.deltaTime);
        }
        else
        {
            // getting back to original size of player
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime);
        }

        if(Input.GetKeyDown(shrinkButton))
        {
            ShrinkCharacter();
        }
    }

    public void ShrinkCharacter()
    {
        // Resetting shrink
        shrink = !shrink;
        // Increase jum power on shrink and get it back to normal on button press
        if (shrink)
            StartCoroutine(UpdateJumpPower());
        else
            tpc.SetJupmPower(jumpPower);
    }


    IEnumerator UpdateJumpPower()
    {
        yield return new WaitForSeconds(3f);
        tpc.SetJupmPower(25);
    }

    // Saving the player size
    object ISaveable.CaptureState()
    {
        return new SerializableVector3(transform.localScale);
    }
    // Loading pkayer size
    void ISaveable.RestoreState(object state)
    {
        SerializableVector3 scale = (SerializableVector3)state;
        transform.localScale = scale.ToVector();
    }
}
