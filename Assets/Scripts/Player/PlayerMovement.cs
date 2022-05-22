using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rigid;
    public Camera filmadora;
    private Vector2 movement;
    private Vector2 mousePosit;

    [SerializeField] private Transform gun;
    [SerializeField] private float gunZMin = -50f, gunZMax = 70f;
    private float gunAngle;

    private Animator playerAnims;
    private readonly int IsWalking = Animator.StringToHash("isWalking");

    private void Start()
    {
        playerAnims = GetComponent<Animator>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (isPlayerHiding)
        {
            PlayerEvents.OnPlayerHided();
        }

        mousePosit = filmadora.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + movement * (moveSpeed * Time.fixedDeltaTime));

        SetGunAngle();
        PlayPlayerAnims();
        AdjustPlayerAndGunAngle();
    }

    private bool isPlayerMoving => movement.x != 0 || movement.y != 0;

    private bool isPlayerHiding => Input.GetKey(KeyCode.C);

    private bool isFacingRight => gunAngle >= gunZMin && gunAngle < gunZMax;

    private void PlayPlayerAnims()
    {
        playerAnims.SetBool(IsWalking, isPlayerMoving);

        if (!isPlayerMoving)
        {
            AudioSystem.Instance.StopPlaying("foot");
            return;
        }

        if (AudioSystem.Instance.soundSources["foot"].source.isPlaying) return;
        AudioSystem.Instance.PlaySfx("foot");
    }

    private void AdjustPlayerAndGunAngle()
    {
        if (!isFacingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            gun.localScale = new Vector3(-1, 1, 1);
            return;
        }

        transform.localScale = new Vector3(1, 1, 1);
        gun.localScale = new Vector3(1, 1, 1);
    }

    private void SetGunAngle()
    {
        var lookDirection = mousePosit - rigid.position;
        gunAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        var newGunRotation = isFacingRight
            ? Quaternion.Euler(0, 0, gunAngle)
            : Quaternion.Euler(180, 0, -gunAngle);

        gun.rotation = newGunRotation;
    }
}