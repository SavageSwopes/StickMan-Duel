using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAttack : MonoBehaviour
{ 
    [SerializeField] private string enemyTag;
    [SerializeField] private float knockbackForce = 5f; 
    [SerializeField] private float knockbackForceUp = 2f;
    [SerializeField] private BoxCollider2D attackHitbox;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attackHitbox.enabled) return;

        if (collision.gameObject.CompareTag(enemyTag))
        {
            if (attackHitbox.IsTouching(collision))
            {
                Debug.Log("Collided with Enemy: " + collision.gameObject.name);

                Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

                    knockbackDirection.y = knockbackForceUp;
                    enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
                // Handle collision with enemy (e.g., take damage, knockback, etc.)
            }

        }
    }
}