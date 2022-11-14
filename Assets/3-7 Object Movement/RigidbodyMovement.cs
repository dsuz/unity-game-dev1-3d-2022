using UnityEngine;

/// <summary>
/// Rigidbody コンポーネントを使ってオブジェクトを動かす
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour
{
    /// <summary>動く時に使うパラメーター</summary>
    [SerializeField] float _moveParameter = 3f;
    /// <summary>ジャンプする時に使うパラメーター</summary>
    [SerializeField] float _jumpParameter = 3f;
    /// <summary>動かす時に使う方法</summary>
    [SerializeField] RigidbodyMoveMethod _moveMethod = RigidbodyMoveMethod.AddForce;
    /// <summary>ジャンプの時に使う方法</summary>
    [SerializeField] RigidbodyJumpMethod _jumpMethod = RigidbodyJumpMethod.Velocity;
    /// <summary>最大ジャンプ回数</summary>
    [SerializeField] int _maxJumpCount = 2;
    [HeaderAttribute("Grounded Area")]
    /// <summary>接地判定範囲の中心点（オフセット）</summary>
    [SerializeField] Vector3 _center = default;
    /// <summary>接地判定範囲の半径</summary>
    [SerializeField] float _radius = 1f;
    /// <summary>地面と判定するレイヤー</summary>
    [SerializeField, Tooltip("地面と判定するレイヤー")] LayerMask _groundLayer = ~0;
    /// <summary>ジャンプしている回数</summary>
    int _jumpCount = 0;
    Rigidbody _rb = default;
    /// <summary>入力方向（XZ平面）</summary>
    Vector3 _dir = default;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 入力を受け取り、カメラを基準にした XZ 平面上に変換する
        _dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _dir = Camera.main.transform.TransformDirection(_dir);
        _dir.y = 0;

        if (_moveMethod == RigidbodyMoveMethod.Velocity)
        {
            // 垂直方向の速度を保持しながら、入力した方向へ動かす
            float verticalVelocity = _rb.velocity.y;
            _rb.velocity = _dir.normalized * _moveParameter + Vector3.up * verticalVelocity;
        }

        // ジャンプ処理
        if (Input.GetButtonDown("Jump") && (IsGrounded() || _jumpCount < _maxJumpCount))
        {
             _jumpCount++;

            switch (_jumpMethod)
            {
                case RigidbodyJumpMethod.AddForceImpulse:
                    _rb.AddForce(Vector3.up * _jumpParameter, ForceMode.Impulse);
                    break;
                case RigidbodyJumpMethod.AddForceVelocityChange:
                    _rb.AddForce(Vector3.up * _jumpParameter, ForceMode.VelocityChange);
                    break;
                case RigidbodyJumpMethod.Velocity:
                    _rb.velocity = new Vector3(_rb.velocity.x, _jumpParameter, _rb.velocity.z);
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        if (_moveMethod == RigidbodyMoveMethod.AddForce)
        {
            _rb.AddForce(_dir.normalized * _moveParameter, ForceMode.Force);
        }

        IsGrounded();
    }

    /// <summary>
    /// 接地判定範囲のギズモの描画をしている
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(GetGroundedAreaCenter(), _radius);
    }

    /// <summary>
    /// 接地判定範囲の中心座標を取得する
    /// </summary>
    /// <returns></returns>
    Vector3 GetGroundedAreaCenter()
    {
        return this.transform.position + _center;
    }

    /// <summary>
    /// 接地判定をする
    /// </summary>
    /// <returns>接地している時は true を返す</returns>
    bool IsGrounded()
    {
        if (Physics.OverlapSphere(GetGroundedAreaCenter(), _radius, _groundLayer).Length > 0)
        {
            _jumpCount = 0;
            return true;
        }

        return false;
    }
}

/// <summary>
/// Rigidbody を使った移動をする時に使う方法
/// </summary>
enum RigidbodyMoveMethod
{
    /// <summary>AddForce メソッドを使う</summary>
    AddForce,
    /// <summary>velocity プロパティを使う</summary>
    Velocity,
}

/// <summary>Rigidbody を使ったジャンプをする時に使う方法</summary>
enum RigidbodyJumpMethod
{
    /// <summary>AddForce メソッドを使い、ForceMode.Impulse を指定する</summary>
    AddForceImpulse,
    /// <summary>AddForce メソッドを使い、ForceMode.VelocityChange を指定する</summary>
    AddForceVelocityChange,
    /// <summary>velocity プロパティを使う</summary>
    Velocity,
}