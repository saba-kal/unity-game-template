using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _footStepInterval = 0.4f;

    private CharacterController _characterController;
    private float _timeSinceLastFootstep = 0;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        var velocity = moveDirection * _speed * Time.deltaTime;
        _characterController.Move(velocity);
        if (velocity.sqrMagnitude > 0)
        {
            PlayFootStepAudio();
        }
        else
        {
            _timeSinceLastFootstep = float.MaxValue;
        }
    }

    private void PlayFootStepAudio()
    {
        if (_timeSinceLastFootstep > _footStepInterval)
        {
            AkSoundEngine.PostEvent("Footsteps", gameObject);
            _timeSinceLastFootstep = 0;
        }
        _timeSinceLastFootstep += Time.deltaTime;
    }
}
