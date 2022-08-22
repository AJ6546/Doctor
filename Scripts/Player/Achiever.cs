using TMPro;
using UnityEngine;

public class Achiever : MonoBehaviour,ISaveable
{
    [SerializeField] int killCount, diagnosisSucceeded, totalDiseasesDiagnosed;
    [SerializeField] TextMeshProUGUI kills, diagnosis;

    // Getters and Setters
    public int GetKillCount()
    {
        return killCount; // Number of enemies player has killed
    }

    public int GetDiagnosisSucceeded()
    {
        return diagnosisSucceeded; ; // Numver of diseases diagnosed correctly
    }

    public int GetTotalDiseasesDiagnose()
    {
        return totalDiseasesDiagnosed; // Total number od disease diagnosed
    }

    public void SetKillCount(int killCount)
    {
        this.killCount = killCount; 
    }

    public void SetDiagnosisSucceeded(int diagnosisSeucceeded)
    {
        this.diagnosisSucceeded = diagnosisSeucceeded;
    }

    public void SetTotalDiseasesDiagnosed(int totalDiseasesDiagnosed)
    {
        this.totalDiseasesDiagnosed = totalDiseasesDiagnosed;
    }

    // Update  Methods
    public void UpdateKillCount()
    {
        killCount += 1;
    }
    public void UpdateDiagnoseSeucceeded()
    {
        diagnosisSucceeded += 1;
    }
    public void UpdateTotalDiseasesDiagnosed()
    {
        totalDiseasesDiagnosed += 1;
    }

    // Update UI elements
    private void Update()
    {
        kills.text = "Kills: " + killCount;
        diagnosis.text = "Diagnosis: " + diagnosisSucceeded + " / " + totalDiseasesDiagnosed;
    }

    // Saving Kill count, diagnosis succeeded and toatal diseases diagnosed
    // creating an int array - saving the array
    object ISaveable.CaptureState()
    {
        int[] playerAcheivements = new int[3];
        playerAcheivements[0] = killCount;
        playerAcheivements[1] = diagnosisSucceeded;
        playerAcheivements[2] = totalDiseasesDiagnosed;
        return playerAcheivements;
    }

    // Load from state in to an int array. setting kill count,
    // diagnosis succeeded and toatal diseases diagnosed from the array
    void ISaveable.RestoreState(object state)
    {
        int[] playerAcheivements = (int[])state;
        killCount = playerAcheivements[0];
        diagnosisSucceeded = playerAcheivements[1];
        totalDiseasesDiagnosed = playerAcheivements[2];
    }
}
