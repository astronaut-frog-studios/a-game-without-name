using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public GameObject hittEffect;
    [SerializeField] private LayerMask layersToCollide;

    [SerializeField] private float autoDestroyTime = 5f;

    public Rigidbody2D rigid;

    [Header("Collision")]
    [ReadOnly, SerializeField] private string collisionTag;
    private Action onBulletCollideCallback;
    public event UnityAction<Action, String> BulletCollide;
    public void OnBulletCollide(Action callback, String tag) => BulletCollide?.Invoke(callback, tag);

    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
        BulletCollide += BulletCollideCallback;
    }

    private void OnDisable()
    {
        BulletCollide -= BulletCollideCallback;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((layersToCollide.value & (1 << other.gameObject.layer)) > 0)
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag(collisionTag))
        {
            onBulletCollideCallback?.Invoke();
        }

        //can add components 
    }

    private void BulletCollideCallback(Action callback, String tag)
    {
        collisionTag = tag;
        onBulletCollideCallback = callback;
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