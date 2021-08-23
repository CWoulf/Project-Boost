using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float rotationThrust = 10f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }
    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            if(!mainBoosterParticles.isPlaying)
            {
                mainBoosterParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainBoosterParticles.Stop();
        }
    }
    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            Rotation(rotationThrust);
            if(!rightBoosterParticles.isPlaying)
            {
                rightBoosterParticles.Play();
            }
        }
        else
        {
            rightBoosterParticles.Stop();
        }
        if (Input.GetKey(KeyCode.D))
        {
            Rotation(-rotationThrust);
            if (!leftBoosterParticles.isPlaying)
            {
                leftBoosterParticles.Play();
            }
        }
        else
        {
            leftBoosterParticles.Stop();
        }
    }

    private void Rotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotatino so the physics system can take over
    }
}
