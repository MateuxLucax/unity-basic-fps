using Hero;
using UnityEngine;

public class FinishGame : MonoBehaviour
{

    private Inventory _inventory;

    private void Start()
    {
        _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(_inventory.GameOver(false));
        }
    }
}
