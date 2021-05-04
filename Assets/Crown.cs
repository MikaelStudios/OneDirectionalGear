using UnityEngine;

public class Crown : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.AddCrown();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.RemoveCrown();
            gameObject.SetActive(false);
        }
    }
}
