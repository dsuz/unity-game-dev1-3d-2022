using UnityEngine;

/// <summary>
/// キャラクターを動かすコンポーネント
/// WASD / カーソルで移動し、Space でジャンプする
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CharacterMove3D : MonoBehaviour
{
    [Tooltip("移動速度")]
    [SerializeField] float _moveSpeed = 1f;
    [Tooltip("ジャンプ速度")]
    [SerializeField] float _jumpSpeed = 1f;
    [Tooltip("弾を生成する場所")]
    [SerializeField] Transform _muzzle;
    [Tooltip("弾のプレハブ")]
    [SerializeField] Bullet3D _bullet;
    [Tooltip("空中ジャンプ可能な回数")]
    [SerializeField] int _maxJumpCountInTheAir = 1;
    Rigidbody _rb = default;
    bool _isGrounded = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 方向の入力を処理する
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        
        if (dir != Vector3.zero)
        {
            this.transform.forward = dir;
        }   // 方向の入力がない場合は何もしない, 入力されたらその方向を向く

        _rb.velocity = dir.normalized * _moveSpeed + _rb.velocity.y * Vector3.up;  // Y 軸方向の速度は変えず、XZ 軸方向に移動する

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Vector3 velocity = _rb.velocity;
            velocity.y = _jumpSpeed;
            _rb.velocity = velocity;
        }   // ジャンプ処理は直接速度を操作する（AddForce だと２段ジャンプの挙動で問題になる）

        if (Input.GetButtonDown("Fire1"))
        {
            var bullet = Instantiate(_bullet);
            bullet.transform.forward = this.transform.forward;
            bullet.transform.position = _muzzle.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ground"))
        {
            _isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Ground"))
        { 
            _isGrounded = false;
        }
    }
}
