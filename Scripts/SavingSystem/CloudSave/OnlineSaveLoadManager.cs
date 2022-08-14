using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class OnlineSaveLoadManager : MonoBehaviour
{
    [SerializeField] GameObject onlineSaveUI;
    [SerializeField] Text email, password, message;
    PlayerData data;
    DatabaseReference reference;
    FirebaseAuth auth;
    [SerializeField] GameObject player;

    [SerializeField] bool isSaving;
    string messageString;
    [SerializeField]
    Achiever acheiver;
    [SerializeField]
    Experience experience;
    [SerializeField]
    OnlineAuthenticator onlineAuth;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        acheiver = player.GetComponent<Achiever>();
        experience = player.GetComponent<Experience>();
        onlineAuth = player.GetComponent<OnlineAuthenticator>();
        if(onlineAuth.IsRegistered())
        {
            messageString = "Enter MailId and Password to Save or Load Data.";
        }
        else
        {
            messageString = "Create an account to save data.";
        }
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Login(bool save)
    {
        if (email.text == "" || password.text == "")
        {
            messageString = "Pease enter a valid Email and Password !!";
            return;
        }
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(
           task =>
           {
               if (task.IsCanceled)
               {
                   messageString = "LogIn cancled";
                   return;
               }
               if (task.IsFaulted)
               {
                   message.text = task.Exception.Flatten().InnerExceptions[0].Message;
                   return;
               }
               if (task.IsCompleted)
               {
                   FirebaseUser user = task.Result;
                   messageString = "Logged in as " + user.Email;
                   if (save)
                   {
                       SaveData(user.UserId, user.Email);
                   }
                   else
                   {
                       LoadData(user.UserId);
                   }
               }
           });
    }


    public void Register()
    {
        if (email.text == "" || password.text == "")
        {
            messageString = "Pease enter a valid Email and Password !!";
            return;
        }
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(
            task =>
            {
                if (task.IsCanceled)
                {
                    messageString = "Registeration cancled";
                    return;
                }
                if (task.IsFaulted)
                {
                    print(task.Exception);
                    messageString = task.Exception.Flatten().InnerExceptions[0].Message;
                    return;
                }
                if (task.IsCompleted)
                {
                    FirebaseUser newUser = task.Result;
                    string tempMessage = "User Created Successfully\nEmail: " + newUser.Email + "\nUserId: " + newUser.UserId;
                    SaveData(newUser.UserId, newUser.Email);
                    messageString = tempMessage;
                    onlineAuth.Regisetered();
                    return;
                }
            }
            );
    }

    public void Save()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Login(true);
        }
    }

    public void Load()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Login(false);
        }
    }
    public void OpenOnlineSave()
    {
        onlineSaveUI.SetActive(true);
    }

    public void  SaveData(string userid, string usermail)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        data = new PlayerData(userid, usermail, (int) experience.GetExperience(), (int) experience.GetMaxExperience(),
            acheiver.GetDiagnosisSucceeded(), acheiver.GetTotalDiseasesDiagnose(), acheiver.GetKillCount());
        string jsonData = JsonUtility.ToJson(data);
        reference.Child("Player_" + userid).SetRawJsonValueAsync(jsonData);
        messageString = "Save Success";
        FirebaseAuth.DefaultInstance.SignOut();
    }

    public void LoadData(string userid)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.GetValueAsync().ContinueWith(
            task =>
            {
                if (task.IsCanceled) { message.text = "Loading data canceled"; return; }
                if (task.IsFaulted) { message.text=task.Exception.Flatten().InnerExceptions[0].Message; }
                if (task.IsCompleted)
                {
                    DataSnapshot data = task.Result;
                    string playerData = data.Child("Player_" + userid).GetRawJsonValue();
                    PlayerData pd = JsonUtility.FromJson<PlayerData>(playerData);
                    
                    acheiver.SetKillCount(pd._totalKillCount);
                    acheiver.SetDiagnosisSucceeded(pd._diagnosisSeucceeded);
                    acheiver.SetTotalDiseasesDiagnosed(pd._totalDiseasesDiagnosed);
                    experience.SetExperience(pd._experiece);
                    experience.SetMaxExperience(pd._maxExperience);
                    messageString = "Load Success";
                    FirebaseAuth.DefaultInstance.SignOut();
                }
            }
            );
    }
    private void Update()
    {
        message.text = messageString;
        if (onlineSaveUI.active)
        {
            isSaving = true;
        }
        else
        {
            if (onlineAuth.IsRegistered())
            {
                messageString = "Enter MailId and Password to Save or Load Data.";
            }
            else
            {
                messageString = "Create an account to save data.";
            }
            isSaving = false;
        }
    }



    public bool IsSaving()
    {
        return isSaving;
    }
}
