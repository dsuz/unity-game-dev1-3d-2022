using UnityEngine;

/// <summary>
/// Player を操作するコンポーネント
/// WASD で操作する。
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SimplePlayerController : MonoBehaviour
{
    [SerializeField] float _movePower = 5f;
    Rigidbody _rb = default;
    /// <summary>入力された方向</summary>
    Vector3 _dir;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 入力を受け付ける
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 入力された方向を「カメラを基準とした XZ 平面上のベクトル」に変換する
        _dir = new Vector3(h, 0, v);
        _dir = Camera.main.transform.TransformDirection(_dir);
        _dir.y = 0;

        // 動いている方向を向く
        Vector3 dir = _rb.velocity;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            this.transform.forward = dir;
        }
    }

    void FixedUpdate()
    {
        _rb.AddForce(_dir.normalized * _movePower);
    }
}
