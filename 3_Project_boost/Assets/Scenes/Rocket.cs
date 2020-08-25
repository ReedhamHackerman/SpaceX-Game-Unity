using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsthrust = 100f;
    [SerializeField] float mainthrust = 100f;
    [SerializeField] AudioClip MainEngine;
    [SerializeField] AudioClip Success;
    [SerializeField] AudioClip Death;
    [SerializeField] ParticleSystem MainenginePartical;
    [SerializeField] ParticleSystem SuccessPartical;
    [SerializeField] ParticleSystem DeathPartical;
    


    enum State {Alive, died, transcating}
    State state = State.Alive;
    void Start()
    {


        //to access the component of the rigidbosy it must have a valid syntax
        //rigidBody = GetComponent<Rigidbody>();
        //rigidBody = GetComponent<Rigidbody>();
        
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Rotate();
            Thrusting();
        }
        if(Debug.isDebugBuild)
        {
            respondtodebugkeys();
        }
        
    }

    private  void respondtodebugkeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)

        {
            case "S1":

                break;
            case "Finish":
                StartSuccess();
                break;
            default:
                StartDeath();
                break;




        }


    }

    private void StartDeath()
    {
        state = State.died;
        audioSource.Stop();
        audioSource.PlayOneShot(Death);
        MainenginePartical.Stop();
        DeathPartical.Play();
        Invoke("LoadFirstLevel", 7f);
    }

    private void StartSuccess()
    {
        state = State.transcating;
        audioSource.Stop();
        audioSource.PlayOneShot(Success);
        MainenginePartical.Stop();
        SuccessPartical.Play();
        Invoke("LoadNextScene", 7f);
    }

    private void LoadFirstLevel()
    {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(CurrentScene);
    }

    private void LoadNextScene()
    {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        int NextSceneindex = CurrentScene + 1;
        if(NextSceneindex == SceneManager.sceneCountInBuildSettings)
        {

            NextSceneindex = 0;
        }
            SceneManager.LoadScene(NextSceneindex);
        
        

    }

    private void Rotate()
    {
        
        rigidBody.freezeRotation = true; // give a control to user
        if (Input.GetKey(KeyCode.A))
        {
            float rotationthisframe = rcsthrust * Time.deltaTime;
            transform.Rotate(Vector3.forward * rotationthisframe);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            float rotationthisframe = rcsthrust * Time.deltaTime;
            transform.Rotate(-Vector3.forward * rotationthisframe);
        }
        rigidBody.freezeRotation = false; // give  a control to physics
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space)) //thrusting can be done during rotation
        {
            ApplyThrust();

        }
        else
        {
            audioSource.Stop();
            MainenginePartical.Stop();
        }
    }

    private void ApplyThrust()
    {
        float rotationthisframe = mainthrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * rotationthisframe);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(MainEngine);
        }
        MainenginePartical.Play();
    }
}
