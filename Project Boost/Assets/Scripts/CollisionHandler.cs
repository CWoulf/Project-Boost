using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("The amount of time after the ship is destoryed or finishes level before it starts the level over")]
    [SerializeField] float LoadWait = 2f;
    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isTransistioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadTheNextScene();
        }        
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle collisions
            Debug.Log("Collision Disabled is: " + collisionDisabled);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransistioning || collisionDisabled) { return; }
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

        crashParticles.Play();
        StartCoroutine(ReloadLevel());
    }    
    private void StartSuccess()
    {
        isTransistioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(successSFX);
        successParticles.Play();
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

    void LoadTheNextScene()
    {
        GetComponent<Movement>().enabled = false;
        // TODO: will need to change once make actual game
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

}
