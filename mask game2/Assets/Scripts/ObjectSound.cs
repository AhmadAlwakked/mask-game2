using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform player;

    [Header("Trigger Settings")]
    public Vector3 triggerSize = new Vector3(50f, 50f, 50f); // halve afmetingen van de kubus
    public float cooldown = 10f;

    private bool canPlay = true;
    private float cooldownTimer = 0f;

    void Start()
    {
        // Als player niet handmatig is ingesteld, zoek automatisch naar object met tag "Player"
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogWarning("Geen object gevonden met tag 'Player' voor ObjectSound!");
        }
    }

    void Update()
    {
        if (player == null) return; // safety check

        // Bereken of de speler binnen de triggerkubus zit
        Vector3 delta = player.position - transform.position;

        if (Mathf.Abs(delta.x) <= triggerSize.x &&
            Mathf.Abs(delta.y) <= triggerSize.y &&
            Mathf.Abs(delta.z) <= triggerSize.z)
        {
            // speler binnen de kubus
            if (canPlay)
            {
                audioSource.Play();
                canPlay = false;
                cooldownTimer = cooldown;
            }
        }

        // cooldown aftellen
        if (!canPlay)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                canPlay = true;
        }
    }

    // Tekent de trigger kubus in de Scene View
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f); // half-transparant groen
        Vector3 size = triggerSize * 2f; // full size van de kubus
        Gizmos.DrawCube(transform.position, size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
