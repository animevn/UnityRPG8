using System;
using Script.Saving;
using UnityEngine;

namespace Script.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSave = "save";
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        private void Save()
        {
            GetComponent<SavingSystem>().Save(DefaultSave);
        }

        private void Load()
        {
            GetComponent<SavingSystem>().Load(DefaultSave);
        }
    }
}
