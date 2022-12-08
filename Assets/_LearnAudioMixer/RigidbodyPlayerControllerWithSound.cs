using UnityEngine;

/// <summary>
/// Rigidbody を使ってプレイヤーを動かすコンポーネント
/// 入力を受け取り、それに従ってオブジェクトを動かす
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyPlayerControllerWithSound : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("移動に使う力")][SerializeField] float _movePower = 5f;
    [Tooltip("最大移動速度")][SerializeField] float _maxSpeed = 5f;
    [Tooltip("方向転換の速さ")][SerializeField] float _turnSpeed = 3f;
    [Tooltip("ジャンプ力")][SerializeField] float _jumpPower = 5f;
    [Tooltip("地面と判定するレイヤーを設定する")][SerializeField] LayerMask _groundLayer;
    [Tooltip("接地判定の開始地点に対する Pivot からのオフセット")][SerializeField] Vector3 _groundCheckStartOffset = Vector3.zero;
    [Tooltip("接地判定の終点に対する Pivot からのオフセット")][SerializeField] Vector3 _groundCheckEndOffset = Vector3.zero;
    [Header("Sound Effects")]
    [Tooltip("効果音を再生するための AudioSource")][SerializeField] AudioSource _audioSource;
    [Tooltip("ジャンプした時の効果音")][SerializeField] AudioClip _jump;
    [Tooltip("足音の効果音")][SerializeField] AudioClip _footStep;
    [Tooltip("攻撃時の効果音")][SerializeField] AudioClip _fire1;
    Rigidbody _rb;
    Animator _anim;
    /// <summary>接地フラグ</summary>
    bool _isGrounded = true;
    /// <summary>移動方向の入力値</summary>
    float _h, _v;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 方向の入力を取得し、方向を求める
        _v = Input.GetAxisRaw("Vertical");
        _h = Input.GetAxisRaw("Horizontal");

        // ジャンプの入力を取得し、接地している時に押されていたらジャンプする
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _anim.SetTrigger("Attack");
        }
    }

    void FixedUpdate()
    {
        // 接地判定
        Vector3 start = _groundCheckStartOffset + transform.position;
        Vector3 end = _groundCheckEndOffset + transform.position;
        Debug.DrawLine(start, end);
        _isGrounded = Physics.Linecast(start, end, _groundLayer);

        // 移動
        if (_isGrounded)    // 空中では移動できない
        {
            Vector3 dir = new Vector3(_h, 0, _v);
            Vector3 velo = _rb.velocity;

            // 入力がない場合はすぐに停める
            if (dir == Vector3.zero)
            {
                _rb.velocity = new Vector3(0, velo.y, 0);
                return;
            }

            velo.y = 0;

            if (velo.magnitude < _maxSpeed) // 最大移動速度を超えている時は力を加えない
            {
                // カメラを基準に入力が上下=奥/手前, 左右=左右に力を加える
                dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
                dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする
                _rb.AddForce(dir * _movePower);

                // 入力方向に滑らかに回転させる
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.fixedDeltaTime * _turnSpeed);
            }
        }
    }

    void LateUpdate()
    {
        // アニメーションを設定する
        Vector3 velo = _rb.velocity;
        velo.y = 0;
        _anim.SetFloat("Speed", velo.magnitude);    // XZ 平面に対する移動速度を Animator Controller に渡す
        _anim.SetBool("IsGrounded", _isGrounded);   // 接地状況を渡す

        // アニメーションの全身⇔上半身のみを切り替える
        if (velo.sqrMagnitude > 0.1f || !_isGrounded)
        {
            // 空中にいるか、または移動している時は上半身のみをアニメーションさせる
            _anim.SetLayerWeight(1, 1);
            _anim.SetLayerWeight(2, 0);
        }
        else
        {
            // 地上にいて止まっている時は全身をアニメーションさせる
            _anim.SetLayerWeight(1, 0);
            _anim.SetLayerWeight(2, 1);
        }
    }

    /// <summary>
    /// ジャンプする。
    /// </summary>
    void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
        _audioSource.PlayOneShot(_jump);
    }

    /// <summary>
    /// 攻撃時に Animation から呼び出される
    /// </summary>
    void Attack()
    {
        _audioSource.PlayOneShot(_fire1);
    }

    /// <summary>
    /// 地面に足がついた時に Animation から呼び出される
    /// </summary>
    void PlayFootStep()
    {
        _audioSource.PlayOneShot(_footStep);
    }
}
