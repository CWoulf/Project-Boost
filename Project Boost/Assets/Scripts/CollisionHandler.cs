using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("The amount of time after the ship is destoryed before it starts the level over")]
    [SerializeField] float wait = 3f;

    private void OnCollisionEnter(Collision other)
    {
        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is Friendly");
                break;
            case "Finish":
                Debug.Log("Congrats you finished");
                break;
            case "Fuel":
                Debug.Log("You picked up fuel");
                break;
            default:
                Debug.Log("Sorry you blew up");
                StartCoroutine(ReloadLevel());
                break;
        }
    }
    
    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(wait);
        // TODO: will need to change once make actual game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
