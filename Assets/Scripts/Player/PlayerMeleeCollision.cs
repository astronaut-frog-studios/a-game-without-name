using UnityEngine;

public class PlayerMeleeCollision : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private float pushBackForce;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform playerFront, playerBack;

    private bool meleeEnemyIsInFront;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isPaused) return;

        var rayHit2DFront = Physics2D.OverlapCircle(playerFront.position, 0.4f, enemyLayer);
        var rayHit2DBack = Physics2D.OverlapCircle(playerBack.position, 0.4f, enemyLayer);

        if (rayHit2DFront)
        {
            meleeEnemyIsInFront = true;
        }

        if (rayHit2DBack)
        {
            meleeEnemyIsInFront = false;
        }
    }

    public void DamageReceived()
    {
        // instantiate hit effect inside playerObject

        if (objectCommingFrontWhenFacingRight)
        {
            playerMovement.rigid.AddForce(-transform.right * pushBackForce);
        }
        else if (objectCommingBackWhenFacingRight)
        {
            playerMovement.rigid.AddForce(transform.right * pushBackForce);
        }
        else if (objectCommingBacktWhenFacingLeft)
        {
            playerMovement.rigid.AddForce(-transform.right * pushBackForce);
        }
        else if (objectCommingFrontWhenFacingLeft)
        {
            playerMovement.rigid.AddForce(transform.right * pushBackForce);
        }
    }

    private bool objectCommingFrontWhenFacingRight => playerMovement.isFacingRight && meleeEnemyIsInFront;
    private bool objectCommingBackWhenFacingRight => playerMovement.isFacingRight && !meleeEnemyIsInFront;
    private bool objectCommingBacktWhenFacingLeft => !playerMovement.isFacingRight && !meleeEnemyIsInFront;
    private bool objectCommingFrontWhenFacingLeft => !playerMovement.isFacingRight && meleeEnemyIsInFront;
}