using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ヒューマノイドを動かすためのコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ShootingRobotMove : MonoBehaviour
{
    [SerializeField] float _movePower = 5f;
    Rigidbody _rb = default;
    Animator _anim = default;
    Vector3 _dir = default;
    /// <summary>フリーズフラグ. これが true の時は動きが止まる</summary>
    bool _freeze = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        _dir = Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward;
    }

    void FixedUpdate()
    {
        if (!_freeze)
        {
            _rb.AddForce(_dir.normalized * _movePower);
        }
    }

    void LateUpdate()
    {
        if (_dir != Vector3.zero)
        {
            this.transform.forward = _dir;
        }

        Vector3 forward = _rb.velocity;
        forward.y = 0;

        if (_anim)
        {
            _anim.SetFloat("Speed", forward.magnitude);
        }
    }

    /// <summary>
    /// 動けなくする
    /// </summary>
    /// <param name="duration">動けなくなる時間（秒）</param>
    public void Freeze(float duration)
    {
        StartCoroutine(FreezeRoutine(duration, null));
    }

    /// <summary>
    /// 動けなくする
    /// </summary>
    /// <param name="duration">動けなくなる時間（秒）</param>
    /// <param name="callback">動けるようになった時に呼ばれるコールバック関数</param>
    public void Freeze(float duration, Action callback)
    {
        StartCoroutine(FreezeRoutine(duration, callback));
    }

    IEnumerator FreezeRoutine(float duration, Action callback)
    {
        // フリーズフラグを立てて、動きを止める
        _freeze = true;
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(duration);  // 待つ
        // フリーズを解除する
        _freeze = false;
        callback();
    }
}
