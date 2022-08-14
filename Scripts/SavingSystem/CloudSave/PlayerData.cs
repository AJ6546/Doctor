using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string _userid;
    public string _email;
    public int _experiece;
    public int _maxExperience;
    public int _diagnosisSeucceeded;
    public int _totalDiseasesDiagnosed;
    public int _totalKillCount;

    public PlayerData() { }

    public PlayerData(string userid, string usermail, int experience,int maxExperience, 
        int diagnosisSeucceeded, int totalDiseasesDiagnosed, int totalKillCount)
    {
        _userid = userid;
        _email = usermail;
        _experiece = experience;
        _maxExperience = maxExperience;
        _diagnosisSeucceeded = diagnosisSeucceeded;
        _totalDiseasesDiagnosed = totalDiseasesDiagnosed;
        _totalKillCount = totalKillCount;
    }
}
