using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SavingSystem : MonoBehaviour
    {
        // Load last scene with the save file
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)state["lastSceneBuildIndex"];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);
        }
        
        // saving a file by calling CaptureState
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        // Loading a file by calling RestoreState
        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }
        
        // Delate save file
        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

    // De serializing from binary to load data
    private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

    // Serializing and saving Data to file
    private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
            
                formatter.Serialize(stream, state);
            }
        }

    // Loops through all the gameobjects with Saveable entity script on it, adds a Unique Id as key and 
    // the dictionary as the value.
        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

    // Loops through all the gameobjects with SaveableEntity script and uses the id to find which 
    // Gameobject the data belongs to and restores the field that was saved.
        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }

        // Get data from save file
        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }