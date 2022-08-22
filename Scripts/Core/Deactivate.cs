using System.Collections;
using UnityEngine;

public class Deactivate : MonoBehaviour
{
    // Deactivates an object after a certain time.
    public void Disable() => StartCoroutine(Disable(2f));
    IEnumerator Disable(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
