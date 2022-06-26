using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private float pushBackForce;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform playerFront, playerBack;

    private bool objectIsInFront;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isPaused) return;


        var rayHit2DFront = Physics2D.OverlapCircle(playerFront.position, 1.0f, enemyLayer);
        var rayHit2DBack = Physics2D.OverlapCircle(playerBack.position, 1.0f, enemyLayer);

        if (rayHit2DFront)
        {
            objectIsInFront = true;
        }

        if (rayHit2DBack)
        {
            objectIsInFront = false;
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

    private bool objectCommingFrontWhenFacingRight => playerMovement.isFacingRight && objectIsInFront;
    private bool objectCommingBackWhenFacingRight => playerMovement.isFacingRight && !objectIsInFront;
    private bool objectCommingBacktWhenFacingLeft => !playerMovement.isFacingRight && !objectIsInFront;
    private bool objectCommingFrontWhenFacingLeft => !playerMovement.isFacingRight && objectIsInFront;
}
