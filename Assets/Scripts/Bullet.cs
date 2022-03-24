using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hittEffect;

    [SerializeField] private float autoDestroyTime = 5f;

    public Rigidbody2D rigid;
    
    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);

        //can add components 
    }

    private void Awake()
    {
       StartCoroutine(DestroySelfAfterSeconds(autoDestroyTime)); // Destruir ela depois de um tempo para não consumir memoria
    }

    private IEnumerator DestroySelfAfterSeconds(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}