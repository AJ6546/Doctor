using Firebase.Auth;
using Firebase.Database;
using System.Collections;
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
        player = GameObject.FindGameObjectWithTag("Player"); // ref to player gameobject
        acheiver = player.GetComponent<Achiever>(); // ref to player's Achiever
        experience = player.GetComponent<Experience>(); // ref to player's Experience
        onlineAuth = player.GetComponent<OnlineAuthenticator>(); // ref to player's OnlineAuthenticator

        // Setting messages for Registering or Saving/Loading Data
        if (onlineAuth.IsRegistered())
        {
            messageString = "Enter MailId and Password to Save or Load Data.";
        }
        else
        {
            messageString = "Create an account to save data.";
        }

        auth = FirebaseAuth.DefaultInstance; // ref to firebase authenticator
    }

    // Log in to firebase account
    public void Login(bool save)
    {
        // check if both input fields are not empty
        if (email.text == "" || password.text == "")
        {
            messageString = "Pease enter a valid Email and Password !!";
            return;
        }
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(
           task =>
           {
               // If task gets canceled
               if (task.IsCanceled)
               {
                   messageString = "LogIn cancled";
                   return;
               }
               // If task is faulted
               if (task.IsFaulted)
               {
                   message.text = task.Exception.Flatten().InnerExceptions[0].Message;
                   return;
               }
               // If task gets completed successfully
               if (task.IsCompleted)
               {
                   // getting firebase user. User is loaded here with credentials provided
                   FirebaseUser user = task.Result;
                   // Setting the message when logged in
                   messageString = "Logged in as " + user.Email;
                   // If user wants to save or load
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

    // Creating a new account with fire base
    public void Register()
    {
        // check if both input fields are not empty
        if (email.text == "" || password.text == "")
        {
            messageString = "Pease enter a valid Email and Password !!";
            return;
        }
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(
            task =>
            {
                // If task gets canceled
                if (task.IsCanceled)
                {
                    messageString = "Registeration cancled";
                    return;
                }
                // If task gets faulted
                if (task.IsFaulted)
                {
                    print(task.Exception);
                    messageString = task.Exception.Flatten().InnerExceptions[0].Message;
                    return;
                }
                // If task gets completed successfully
                if (task.IsCompleted)
                {
                    // getting firebase user. New user is created here
                    FirebaseUser newUser = task.Result;
                    // Setting the message when registered
                    string tempMessage = "User Created Successfully\nEmail: " + newUser.Email + "\nUserId: " + newUser.UserId;
                    // Saving the current data to firebase
                    SaveData(newUser.UserId, newUser.Email);
                    messageString = tempMessage;
                    // Setting isregistered
                    onlineAuth.Regisetered();
                }
            }
            );
        // Saving the data locally
        StartCoroutine(SaveAfterRegister());
    }

    IEnumerator SaveAfterRegister()
    {
        yield return new WaitForSeconds(5);
        FindObjectOfType<SavingWrapper>().Save();
    }

    // When save button is clicked
    public void Save()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Login(true);
        }
    }

    // When load button is clicked
    public void Load()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Login(false);
        }
    }
    // Show Ui pop up for saving data to firebase
    public void OpenOnlineSave()
    {
        onlineSaveUI.SetActive(true);
    }

    // Saving data
    public void  SaveData(string userid, string usermail)
    {
        // Getting db reference
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        // getting the data to be saved
        data = new PlayerData(userid, usermail, (int) experience.GetExperience(), (int) experience.GetMaxExperience(),
            acheiver.GetDiagnosisSucceeded(), acheiver.GetTotalDiseasesDiagnose(), acheiver.GetKillCount());
        // converting data to jason string
        string jsonData = JsonUtility.ToJson(data);
        // Saving data
        reference.Child("Player_" + userid).SetRawJsonValueAsync(jsonData);
        // Setting messageafter save is success
        messageString = "Save Success";
        // Signing out
        FirebaseAuth.DefaultInstance.SignOut();
    }

    public void LoadData(string userid)
    {
        // Getting db reference
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.GetValueAsync().ContinueWith(
            task =>
            {
                // setting message when task is canceled
                if (task.IsCanceled) { message.text = "Loading data canceled"; return; }
                // setting message when task is faulted
                if (task.IsFaulted) { message.text=task.Exception.Flatten().InnerExceptions[0].Message; }
                if (task.IsCompleted)
                {
                    // getting a data snapshot
                    DataSnapshot data = task.Result;
                    // converting from data snapshot to string for the user
                    string playerData = data.Child("Player_" + userid).GetRawJsonValue();

                    // Updating player data
                    PlayerData pd = JsonUtility.FromJson<PlayerData>(playerData);
                    acheiver.SetKillCount(pd._totalKillCount);
                    acheiver.SetDiagnosisSucceeded(pd._diagnosisSeucceeded);
                    acheiver.SetTotalDiseasesDiagnosed(pd._totalDiseasesDiagnosed);
                    experience.SetExperience(pd._experiece);
                    experience.SetMaxExperience(pd._maxExperience);
                    // Setting message for load success
                    messageString = "Load Success";
                    // signing out
                    FirebaseAuth.DefaultInstance.SignOut();
                }
            }
            );
    }
    private void Update()
    {
        // setting ui message depending on if the player is registered or not
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
