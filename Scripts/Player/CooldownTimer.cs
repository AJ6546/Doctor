using System.Collections.Generic;
using UnityEngine;

public class CooldownTimer : MonoBehaviour
{
    [SerializeField] int[] attackTimers = new int[4];
    public Dictionary<string, int> coolDownTime = new Dictionary<string, int>();
    public Dictionary<string, int> nextAttackTime = new Dictionary<string, int>();
    void Start()
    {
        for (int i = 0; i < attackTimers.Length; i++)
        {
            coolDownTime["Attack0" + (i + 1).ToString()] = attackTimers[i];
            nextAttackTime["Attack0" + (i + 1).ToString()] = 0;
        }
    }
}
