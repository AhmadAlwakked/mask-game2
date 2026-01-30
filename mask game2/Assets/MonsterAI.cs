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
    [Tooltip("Max afstand dat het monster kan wandelen bij elke stap")]
    public float wanderRadius = 15f;

    [Header("Chase Settings")]
    public float chaseRadius = 10f; // maximale afstand monster kan zien
    public int rayCount = 120;
    public float viewHeightOffset = 1.5f;

    [Header("Automatic References")]
    private Transform[] spawnPoints;
    private Transform player;
    private PlayerMovement playerMovement;

    private NavMeshAgent agent;
    private float timer;
    private bool isSprinting = false;

    // Raycast punten voor gizmos
    private Vector3[] rayPoints;

    void Awake()
    {
        // Zoek player automatisch op basis van tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerMovement = playerObj.GetComponent<PlayerMovement>();
        }

        // Zoek alle spawnpoints met tag "SpawnpointV"
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnpointV");
        spawnPoints = new Transform[spawns.Length];
        for (int i = 0; i < spawns.Length; i++)
            spawnPoints[i] = spawns[i].transform;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Zet monster op een random spawnpoint
        if (spawnPoints.Length > 0)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            transform.position = spawn.position;
        }

        PickNewDestination();
    }

    void Update()
    {
        if (player == null || agent == null || !agent.isOnNavMesh) return;

        if (playerMovement != null && playerMovement.inside)
        {
            // Player is in vault, wanderen
            isSprinting = false;
            agent.speed = walkSpeed;

            timer += Time.deltaTime;
            if (timer >= wanderInterval || agent.remainingDistance <= agent.stoppingDistance)
                PickNewDestination();
        }
        else
        {
            // Player detectie
            if (IsPlayerInRadius())
            {
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

        UpdateRayPoints();
    }

    bool IsPlayerInRadius()
    {
        if (player == null) return false;

        Vector3 monsterEye = transform.position + Vector3.up * viewHeightOffset;
        Vector3 playerPos = player.position + Vector3.up * 0.5f;

        float dist = Vector3.Distance(monsterEye, playerPos);
        if (dist > chaseRadius) return false;

        if (Physics.Raycast(monsterEye, (playerPos - monsterEye).normalized, out RaycastHit hit, chaseRadius))
        {
            if (hit.transform != player)
                return false;
        }

        return true;
    }

    void PickNewDestination()
    {
        timer = 0f;
        Vector3 randomPoint = GetRandomNavMeshPoint();
        if (agent != null && agent.isOnNavMesh)
            agent.SetDestination(randomPoint);
    }

    Vector3 GetRandomNavMeshPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius + transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            return hit.position;

        return transform.position;
    }

    void UpdateRayPoints()
    {
        Vector3 center = transform.position + Vector3.up * viewHeightOffset;
        rayPoints = new Vector3[rayCount];

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * 360f / rayCount;
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

            if (Physics.Raycast(center, dir, out RaycastHit hit, chaseRadius))
                rayPoints[i] = hit.point;
            else
                rayPoints[i] = center + dir * chaseRadius;
        }
    }

    private void OnDrawGizmosSelected()
    {
        UpdateRayPoints();
        if (rayPoints == null || rayPoints.Length == 0) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < rayPoints.Length; i++)
        {
            Vector3 from = rayPoints[i];
            Vector3 to = rayPoints[(i + 1) % rayPoints.Length];
            Gizmos.DrawLine(from, to);
        }

#if UNITY_EDITOR
        Handles.color = new Color(1f, 0f, 0f, 0.1f);
        Handles.DrawAAConvexPolygon(rayPoints);
#endif
    }
}
