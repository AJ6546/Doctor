using TMPro;
using UnityEngine;

public class Achiever : MonoBehaviour,ISaveable
{
    [SerializeField] int killCount, diagnosisSeucceeded, totalDiseasesDiagnosed;
    [SerializeField] TextMeshProUGUI kills, diagnosis;

    // Getters and Setters
    public int GetKillCount()
    {
        return killCount;
    }

    public int GetDiagnosisSucceeded()
    {
        return diagnosisSeucceeded; ;
    }

    public int GetTotalDiseasesDiagnose()
    {
        return totalDiseasesDiagnosed;
    }

    public void SetKillCount(int killCount)
    {
        this.killCount = killCount;
    }

    public void SetDiagnosisSucceeded(int diagnosisSeucceeded)
    {
        this.diagnosisSeucceeded = diagnosisSeucceeded;
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
        diagnosisSeucceeded += 1;
    }
    public void UpdateTotalDiseasesDiagnosed()
    {
        totalDiseasesDiagnosed += 1;
    }

    // Update UI elements
    private void Update()
    {
        kills.text = "Kills: " + killCount;
        diagnosis.text = "Diagnosis: " + diagnosisSeucceeded + " / " + totalDiseasesDiagnosed;
    }

    // Saving
    object ISaveable.CaptureState()
    {
        int[] playerAcheivements = new int[3];
        playerAcheivements[0] = killCount;
        playerAcheivements[1] = diagnosisSeucceeded;
        playerAcheivements[2] = totalDiseasesDiagnosed;
        return playerAcheivements;
    }

    void ISaveable.RestoreState(object state)
    {
        int[] playerAcheivements = (int[])state;
        killCount = playerAcheivements[0];
        diagnosisSeucceeded = playerAcheivements[1];
        totalDiseasesDiagnosed = playerAcheivements[2];
    }
}
