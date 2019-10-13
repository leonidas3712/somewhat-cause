using UnityEngine;
using UnityEngine.SceneManagement;

public class WinComponent : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex+1);
    }
}
