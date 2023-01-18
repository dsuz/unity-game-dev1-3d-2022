using UnityEngine;

/// <summary>
/// 弾を飛ばすコンポーネント
/// 弾のオブジェクトにアタッチして使う
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet3D : MonoBehaviour
{
    [Tooltip("弾が飛ぶ速度")]
    [SerializeField] float _speed = 20f;
    [Tooltip("弾が発射されてから消えるまでの時間（秒）")]
    [SerializeField] float _lifeTime = 30f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = this.transform.forward * _speed;  // 前方に飛ばす
        Destroy(this.gameObject, _lifeTime);    // 消えるまでの時間を設定する
    }
}
