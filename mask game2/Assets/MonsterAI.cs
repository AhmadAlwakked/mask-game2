using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.AI;

[ExecuteAlways]
public class MonsterChase : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 2.5f;
    public float sprintSpeed = 5.5f;

    [Header("Wander Settings")]
    public float wanderInterval = 4f;

    [Header("Chase Settings")]
    public float chaseRadius = 10f; // maximale afstand monster kan zien
    public int rayCount = 120;       // aantal rays voor de fan
    public Transform player;
    public float viewHeightOffset = 1.5f; // hoogte-offset van de FOV

    private NavMeshAgent agent;
    private float timer;
    private bool isSprinting = false;

    // Raycast punten voor gizmos
    private Vector3[] rayPoints;

    // Referentie naar player movement
    private PlayerMovement playerMovement;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();

        PickNewDestination();
    }

    void Update()
    {
        // Check of agent actief is op de NavMesh
        if (player == null || agent == null || !agent.isOnNavMesh) return;

        // Als de player "inside" is, volg niet
        if (playerMovement != null && playerMovement.inside)
        {
            // Stop sprinten en volg niet
            isSprinting = false;
            agent.speed = walkSpeed;

            // Blijf normaal wandelen
            timer += Time.deltaTime;
            if (timer >= wanderInterval || agent.remainingDistance <= agent.stoppingDistance)
                PickNewDestination();
        }
        else
        {
            // Player detection
            if (IsPlayerInRadius())
            {
                // Sprint en volg player
                isSprinting = true;
                agent.speed = sprintSpeed;
                agent.SetDestination(player.position);
            }
            else
            {
                // Wandering
                if (isSprinting) timer = wanderInterval;
                isSprinting = false;
                agent.speed = walkSpeed;

                timer += Time.deltaTime;
                if (timer >= wanderInterval || agent.remainingDistance <= agent.stoppingDistance)
                    PickNewDestination();
            }
        }

        // Update FOV rays voor Gizmos
        UpdateRayPoints();
    }

    // =================== Player Detection ===================
    bool IsPlayerInRadius()
    {
        if (player == null) return false;

        Vector3 monsterEye = transform.position + Vector3.up * viewHeightOffset;
        Vector3 playerPos = player.position + Vector3.up * 0.5f; // speler half-height

        // Check afstand
        float dist = Vector3.Distance(monsterEye, playerPos);
        if (dist > chaseRadius) return false;

        // Check line-of-sight: geen muren ertussen
        RaycastHit hit;
        if (Physics.Raycast(monsterEye, (playerPos - monsterEye).normalized, out hit, chaseRadius))
        {
            if (hit.transform != player)
                return false; // muur blokkeert zicht
        }

        return true; // speler binnen zicht en zonder obstakel
    }

    // =================== Wandering ===================
    void PickNewDestination()
    {
        timer = 0f;
        Vector3 randomPoint = GetRandomNavMeshPoint();
        if (agent != null && agent.isOnNavMesh)
            agent.SetDestination(randomPoint);
    }

    Vector3 GetRandomNavMeshPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 1000f + transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 1000f, NavMesh.AllAreas))
            return hit.position;
        return transform.position;
    }

    // =================== Raycast FOV ===================
    void UpdateRayPoints()
    {
        Vector3 center = transform.position + Vector3.up * viewHeightOffset;
        rayPoints = new Vector3[rayCount];

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * 360f / rayCount;
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

            RaycastHit hit;
            if (Physics.Raycast(center, dir, out hit, chaseRadius))
            {
                rayPoints[i] = hit.point;
            }
            else
            {
                rayPoints[i] = center + dir * chaseRadius;
            }
        }
    }

    // =================== Gizmos ===================
    private void OnDrawGizmosSelected()
    {
        UpdateRayPoints();
        if (rayPoints == null || rayPoints.Length == 0) return;

        // Rood outline
        Gizmos.color = Color.red;
        for (int i = 0; i < rayPoints.Length; i++)
        {
            Vector3 from = rayPoints[i];
            Vector3 to = rayPoints[(i + 1) % rayPoints.Length];
            Gizmos.DrawLine(from, to);
        }

#if UNITY_EDITOR
        // Semi-transparant rood ingevuld polygon
        Handles.color = new Color(1f, 0f, 0f, 0.1f);
        Handles.DrawAAConvexPolygon(rayPoints);
#endif
    }
}
