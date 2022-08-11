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
        if (GetComponent<Health>().IsDead()) return;
        if (GetComponent<PlayerConversant>().IsTalking()) return;
        if (shrink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newSize, Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime);
        }
        if(Input.GetKeyDown(shrinkButton))
        {
            ShrinkCharacter();
        }
    }

    public void ShrinkCharacter()
    {
        shrink = !shrink;
        if (shrink)
            StartCoroutine(UpdateJumpPower());
        else
            tpc.SetJupmPower(jumpPower);
    }
    void Timer()
    {
        timer.text = (Mathf.Max(cdTimer.nextAttackTime["Action01"] - (int)Time.time, 0)).ToString();
    }

    IEnumerator UpdateJumpPower()
    {
        yield return new WaitForSeconds(3f);
        tpc.SetJupmPower(25);
    }

    object ISaveable.CaptureState()
    {
        return new SerializableVector3(transform.localScale);
    }

    void ISaveable.RestoreState(object state)
    {
        SerializableVector3 scale = (SerializableVector3)state;
        transform.localScale = scale.ToVector();
    }
}
