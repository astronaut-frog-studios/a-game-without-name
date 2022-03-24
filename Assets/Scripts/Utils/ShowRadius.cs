using UnityEngine;

public class ShowRadius : MonoBehaviour
{
    public float radius = 5f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}