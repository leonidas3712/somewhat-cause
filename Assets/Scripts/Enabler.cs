using UnityEngine;

public class Enabler : MonoBehaviour
{
    public GameObject gameObjectToEnable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObjectToEnable.SetActive(true);
    }
}
