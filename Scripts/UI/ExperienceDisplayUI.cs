using TMPro;
using UnityEngine;


public class ExperienceDisplayUI : MonoBehaviour
{
    [SerializeField] Experience experience;
    [SerializeField] TextMeshProUGUI experiecneText;
    void Awake()
    {
        experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
    }

    void Update()
    {
        experiecneText.text = "Experience: "+experience.GetExperience().ToString() + "/" + Mathf.Max(experience.GetExperience(), experience.GetMaxExperience()).ToString();
    }
}