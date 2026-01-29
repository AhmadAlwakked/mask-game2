using UnityEngine;

public class Vault : MonoBehaviour
{
    [Header("Link the vault door here")]
    public VaultDoor door; // 🔹 sleep hier de VaultDoor in de Inspector

    [HideInInspector] public bool inside = false; // true als speler bij een gesloten vault staat

    private void OnTriggerEnter(Collider other)
    {
        if (door == null) return;

        if (other.CompareTag("Player"))
        {
            if (!door.IsOpen())
                inside = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (door == null) return;

        if (other.CompareTag("Player"))
        {
            inside = !door.IsOpen();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
        }
    }
}
