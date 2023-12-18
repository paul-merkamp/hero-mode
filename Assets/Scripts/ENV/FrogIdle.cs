using UnityEngine;

public class FrogIdle : MonoBehaviour
{
    public float moveRadius = 5f;
    public float moveSpeed = 2f;
    public float minDelay = 1f;
    public float maxDelay = 5f;

    private Vector2 startingLocation;
    private Animator animator;

    private void Start()
    {
        startingLocation = transform.position;
        animator = GetComponent<Animator>();

        StartCoroutine(MoveToRandomPosition());
    }

    private System.Collections.IEnumerator MoveToRandomPosition()
    {
        while (true)
        {
            Vector2 targetPosition = GetRandomPosition();
            float distance = Vector2.Distance(transform.position, targetPosition);

            animator.SetBool("IsMoving", true);

            while (distance > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPosition - (Vector2)transform.position);
                
                distance = Vector2.Distance(transform.position, targetPosition);

                float normalizedDistance = 1f - (distance / moveRadius);
                float animationSpeed = Mathf.Lerp(1f, 2f, normalizedDistance);
                animator.SetFloat("Speed", animationSpeed);

                yield return null;
            }

            animator.SetBool("IsMoving", false);

            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 randomPosition = startingLocation + randomDirection * Random.Range(0f, moveRadius);
        return randomPosition;
    }
}