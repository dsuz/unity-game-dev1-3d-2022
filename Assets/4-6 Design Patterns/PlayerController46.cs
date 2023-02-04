using UnityEngine;

/// <summary>
/// プレイヤー操作に必要なコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController46 : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpSpeed = 5f;
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 velocity = _rb.velocity;
        velocity.x = h * _moveSpeed;
        _rb.velocity = velocity;

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Jump()
    {
        Vector3 velocity = _rb.velocity;
        velocity.y = _jumpSpeed;
        _rb.velocity = velocity;
    }
}
