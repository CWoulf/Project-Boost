﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("The amount of time after the ship is destoryed or finishes level before it starts the level over")]
    [SerializeField] float LoadWait = 2f;
    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip crashSFX;

    AudioSource audioSource;
    bool isTransistioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransistioning) { return; }
        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is Friendly");
                break;
            case "Finish":
                StartSuccess();
                break;
            default:
                Debug.Log("Sorry you blew up");
                StartCrash();
                break;
        }
    }

    private void StartCrash()
    {
        isTransistioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(crashSFX);
        
        //TODO: add partilce effect upon crash
        StartCoroutine(ReloadLevel());
    }    
    private void StartSuccess()
    {
        isTransistioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(successSFX);
        //TODO: add partilce effect upon win
        StartCoroutine(LoadNextLevel());
    }
    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(LoadWait);
        // TODO: will need to change once make actual game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    
    IEnumerator LoadNextLevel()
    {
        GetComponent<Movement>().enabled = false;
        yield return new WaitForSeconds(LoadWait);
        // TODO: will need to change once make actual game
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
