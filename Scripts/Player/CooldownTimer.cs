using System.Collections.Generic;
using UnityEngine;

public class CooldownTimer : MonoBehaviour
{
    [SerializeField] int[] actionTimers = new int[6];
    public Dictionary<string, int> coolDownTime = new Dictionary<string, int>();
    public Dictionary<string, int> nextActionTime = new Dictionary<string, int>();
    void Start()
    {
        for (int i = 0; i < actionTimers.Length; i++)
        {
            coolDownTime["Action0" + (i + 1).ToString()] = actionTimers[i];
            nextActionTime["Action0" + (i + 1).ToString()] = 0;
        }
    }
}
