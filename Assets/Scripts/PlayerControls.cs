using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float MaxMoveSpeed = 1;

    [SerializeField] private HealthAndEnergy _healthAndEnergy;

    public AudioSource DashSound;
    public AudioSource StepSound;

    private CharacterController controllerComponent;
    private Animator animatorComponent;
   
    private Vector3 moveSpeed;
    private float grabCooldown;
    private float dashingTimeLeft;

    private static readonly int GrabParam = Animator.StringToHash("grab");
    private static readonly int WalkSpeedParam = Animator.StringToHash("walk speed");
    private static readonly int RunParam = Animator.StringToHash("Run");

    private void Start()
    {
        animatorComponent = GetComponent<Animator>();
        controllerComponent = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (MainMenu.IsGameStarted)
        {
            UpdateWalk();

            //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.Z)) Grab();
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.X)) Dash(false);
            else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.X)) Dash(true);
            grabCooldown -= Time.deltaTime;
        }
        animatorComponent.SetFloat(WalkSpeedParam, new Vector3(moveSpeed.x,0,moveSpeed.z).magnitude);//moveSpeed.magnitude);//
        if (WalkSpeedParam > 0)
        {
            StepSound.Play();
        }
        MaxMoveSpeed = 1 * _healthAndEnergy.energy / 100;
    }

    private void Dash(bool holding)
    {
        if (dashingTimeLeft < (holding ? -.4f : -.2f))
        {
            dashingTimeLeft = .3f;
            DashSound.Play();
        }
    }

    public void StepAnimationCallback()
    {
        if (dashingTimeLeft > 0) return;

        if (StepSound.pitch < 1) StepSound.pitch = Random.Range(1.05f, 1.15f);
        else StepSound.pitch = Random.Range(0.9f, 0.95f);
    }

    private void UpdateWalk()
    {
        float ySpeed = moveSpeed.y;
        moveSpeed.y = 0;
        if (dashingTimeLeft <= 0)
        {
            //moveSpeed = MaxMoveSpeed * 3.5f * moveSpeed.normalized;
            Vector3 target = MaxMoveSpeed *
                            new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            moveSpeed = Vector3.MoveTowards(moveSpeed, target, Time.deltaTime * 150);

            if (moveSpeed.magnitude > 0.1f)
            {
               transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveSpeed),
                   Time.deltaTime * 300);
            }
        }
        else
        {
            moveSpeed = MaxMoveSpeed * 1.6f * moveSpeed.normalized;
            _healthAndEnergy.energy -= Time.deltaTime * 0.007f * 180f;
        }

        dashingTimeLeft -= Time.deltaTime;

        moveSpeed.y = ySpeed + Physics.gravity.y * Time.deltaTime;
        controllerComponent.Move(moveSpeed * Time.deltaTime);
        StepSound.Play();
    }
}