﻿using UnityEngine;
using RPG.Saving;
using System.Collections;

namespace RPG.SceneManagement
{

    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile="save";
        [SerializeField] float fadeInTime = 1f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();
            yield return fader.FadeIn(fadeInTime);
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public
            
            void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}

