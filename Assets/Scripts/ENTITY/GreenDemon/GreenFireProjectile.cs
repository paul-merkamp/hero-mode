using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFireProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public int lifetimeInSeconds = 20;
    private float lifetimeTimer;

    public enum Direction
    {
        down,
        right
    }

    public Direction direction;

    private void Start()
    {
        lifetimeTimer = lifetimeInSeconds;
    }

    private void Update()
    {
        // Move the projectile forward
        if (direction == Direction.right)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else if (direction == Direction.down)
            transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Decrease the lifetime timer
        lifetimeTimer -= Time.deltaTime;

        // Destroy the projectile if lifetime timer expires
        if (lifetimeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the projectile collided with an Entity or Player
        Entity entity = other.gameObject.GetComponent<Entity>();
        PlayerForm player = other.gameObject.GetComponent<PlayerForm>();

        if (entity != null)
        {
            // Deal damage to the entity
            entity.TakeDamage(damage);
        }
        else if (player != null)
        {
            // Deal damage to the player
            player.TakeDamage(damage, gameObject);
        }

        // Destroy the projectile
        Destroy(gameObject);
    }
}
