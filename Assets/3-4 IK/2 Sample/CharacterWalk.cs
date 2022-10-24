using UnityEngine;

/// <summary>
/// キャラクターを歩かせる
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CharacterWalk : MonoBehaviour
{
    /// <summary>移動する時の力</summary>
    [SerializeField] float _movePower = 10f;
    Rigidbody _rb = default;
    /// <summary>入力された方向</summary>
    Vector3 _dir = default;
    Animator _anim = default;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 入力を受け付ける
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        _dir = new Vector3(h, 0, v);

        // キャラクターの向きを制御する
        if (_dir != Vector3.zero)
        {
            this.transform.forward = _dir;
        }
    }

    void FixedUpdate()
    {
        // 入力に応じて力を加えて移動させる
        Vector3 dir = Camera.main.transform.TransformDirection(_dir);
        dir.y = 0;
        _rb.AddForce(dir.normalized * _movePower, ForceMode.Force);
    }

    void LateUpdate()
    {
        // アニメーションを制御する
        if (_anim)
        {
            Vector3 velocity = _rb.velocity;
            velocity.y = 0;
            _anim.SetFloat("Speed", velocity.magnitude);
        }
    }
}
