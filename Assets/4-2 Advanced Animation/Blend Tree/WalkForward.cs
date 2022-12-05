using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WalkForward : MonoBehaviour
{
    [SerializeField] float _power = 1f;
    [SerializeField] float _idleTime = 3f;
    Rigidbody _rb = default;
    Animator _anim = default;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (_idleTime > 0)
        {
            _idleTime -= Time.fixedDeltaTime;
        }
        else
        {
            _rb.AddForce(this.transform.forward * _power, ForceMode.Force);
        }
    }

    void LateUpdate()
    {
        _anim.SetFloat("Speed", _rb.velocity.magnitude);
    }
}
