using UnityEngine;

/// <summary>
/// Rigidbody を使ってプレイヤーを動かすコンポーネント
/// 入力を受け取り、それに従ってオブジェクトを動かす。
/// カメラの向きに応じて相対的に移動する。
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerNinjaController : MonoBehaviour
{
    /// <summary>動く速さ</summary>
    [SerializeField] float _movingSpeed = 5f;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float _turnSpeed = 3f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float _jumpSpeed = 5f;
    /// <summary>接地判定の際、コライダーの中心 (Center) からどれくらいの距離を「接地している」と判定するかの長さ</summary>
    [SerializeField] float _isGroundedLength = 1.1f;
    /// <summary>地面を表す Layer</summary>
    [SerializeField] LayerMask _groundLayer = ~0;
    /// <summary>何回まで接地せずにジャンプできるか。２段ジャンプする時は 2 に設定する</summary>
    [SerializeField] int _maxJumpCount = 2;
    /// <summary>弾を発射する地点となるオブジェクト</summary>
    [SerializeField] Transform _muzzle = null;
    /// <summary>弾のプレハブ</summary>
    [SerializeField] GameObject _bulletPrefab = null;
    /// <summary>ジャンプした時に鳴らす効果音</summary>
    [SerializeField] AudioClip _jumpSfx = null;

    /// <summary>ジャンプした回数。接地状態からジャンプした時に 1 になる。</summary>
    int _jumpCount = 0;
    /// <summary>敵を検出するコンポーネント</summary>
    EnemyDetector _enemyDetector = null;
    /// <summary>接地フラグ</summary>
    bool _isGrounded = true;
    Rigidbody _rb = null;
    Animator _anim = null;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _enemyDetector = GetComponent<EnemyDetector>();
    }

    void Update()
    {
        // 方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // 入力方向のベクトルを組み立てる
        Vector3 dir = Vector3.forward * v + Vector3.right * h;

        if (dir == Vector3.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を保持するだけ
            _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
        }
        else
        {
            // カメラを基準に入力が上下=奥/手前, 左右=左右にキャラクターを向ける
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);  // Slerp を使うのがポイント

            Vector3 velo = dir.normalized * _movingSpeed; // 入力した方向に移動する
            velo.y = _rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
            _rb.velocity = velo;   // 計算した速度ベクトルをセットする
        }

        // ジャンプの入力を取得し、接地している場合はジャンプする
        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded)
            {
                Jump();
            }   // 接地している時
            else if (_jumpCount < _maxJumpCount)
            {
                _jumpCount++;
                _anim.SetTrigger("JumpTrigger");
                Jump();
            }   // 接地しておらず、最大ジャンプ回数に達していない時
        }

        // 接地している時のみ攻撃可能
        if (Input.GetButtonDown("Fire1") && _isGrounded)
        {
            Attack();
        }
    }

    void LateUpdate()
    {
        // アニメーションを操作する
        Vector3 velocity = _rb.velocity;
        velocity.y = 0; // 上下方向の速度は無視する
        _anim.SetFloat("Speed", velocity.magnitude);
        _anim.SetBool("IsGrounded", _isGrounded);
    }

    /// <summary>
    /// ジャンプする時に呼び出す
    /// </summary>
    void Jump()
    {
        AudioSource.PlayClipAtPoint(_jumpSfx, this.transform.position);
        // AddForce でなくスピードを変える（AddForce だと、降下中にジャンプした時にジャンプが弱くなる）
        Vector3 velocity = _rb.velocity;
        velocity.y = _jumpSpeed;
        _rb.velocity = velocity;
    }

    /// <summary>
    /// 攻撃する時に呼び出す
    /// </summary>
    void Attack()
    {
        // ロックオンしている敵がいる場合
        if (_enemyDetector.Target)
        {
            Debug.Log("ロックオンしている敵がいます");
            // ロックオンしている敵の方を向く
            Vector3 dir = _enemyDetector.Target.transform.position - transform.position;
            dir.y = 0;  // 上や下を向かないように調整する
            transform.forward = dir;
        }

        // 攻撃アニメーションを再生し、弾を発射する
        _anim.SetTrigger("AttackTrigger");
        Instantiate(_bulletPrefab, _muzzle.position, _muzzle.rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        _isGrounded = true;
        _jumpCount = 0; // 着地した時にカウンタをリセットする
    }

    void OnTriggerExit(Collider other)
    {
        _isGrounded = false;
        _jumpCount = 1; // 初回ジャンプまたは足場から降りた時にカウンタを１にする
        _anim.SetTrigger("JumpTrigger");    // ジャンプした時や足場から降りた時にモーションを再生する
    }
}