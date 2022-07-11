using System.Collections;
using TMPro;
using UnityEngine;

public class Shrink : MonoBehaviour, ISaveable
{
    [SerializeField] UIAssigner uiAssigner;
    [SerializeField] bool shrink,startTimer,enlarge;
    [SerializeField] ThirdPersonCharacter tpc;
    Vector3 newSize = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 originalScale = new Vector3(1, 1, 1);
    CooldownTimer cdTimer;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float jumpPower;
    void Start()
    {
        uiAssigner = GetComponent<UIAssigner>();
        cdTimer = GetComponent<CooldownTimer>();
        tpc = GetComponent<ThirdPersonCharacter>();
        jumpPower = tpc.GetJumpPower();
    }
    void Update()
    {
        if (shrink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newSize, Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime);
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
        timer.text = (Mathf.Max(cdTimer.nextActionTime["Action01"] - (int)Time.time, 0)).ToString();
    }

    IEnumerator UpdateJumpPower()
    {
        yield return new WaitForSeconds(3f);
        tpc.SetJupmPower(25);
    }

    public object CaptureState()
    {
        return new SerializableVector3(transform.localScale);
    }

    public void RestoreState(object state)
    {
        SerializableVector3 scale = (SerializableVector3)state;
        transform.localScale = scale.ToVector();
    }
}
