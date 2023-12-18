using UnityEngine;

public class AggroAI : MonoBehaviour
{
    private Entity entity;

    private int layerMask;
    private Transform player;

    public float aggroRange;

    private void Start()
    {
        int enemyMask = 1 << LayerMask.NameToLayer("damaging_entity");
        int coverMask = 1 << LayerMask.NameToLayer("vision_cover");
        int ignoreRaycastMask = 1 << LayerMask.NameToLayer("Ignore Raycast");

        layerMask = enemyMask | coverMask | ignoreRaycastMask;
        layerMask = ~layerMask;

        entity = GetComponent<Entity>();
        player = GameObject.Find("PlayerFollower").transform;
    }

    private void Update()
    {
        if (PlayerInRange())
        {
            if (CanSeePlayer())
            {
                MoveTowardsPlayer();
            }
        }
    }

    private bool PlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) < aggroRange;
    }
    
    private bool CanSeePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y += 0.03f; // Aim 0.05 higher
        direction.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask);
        Debug.DrawLine(transform.position, hit.point, Color.blue);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    private void MoveTowardsPlayer()
    {
        // Calculate the direction towards the player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        entity.Move(direction);
    }
}