using UnityEngine;

/// <summary>
/// 隕石をコントロールするコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class CometController : MonoBehaviour
{
    Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddTorque(Random.Range(-10f, 10f));  // ランダムな強さで回転させる
        _rb.gravityScale = Random.Range(0.01f, 0.03f); // 落ちる加速度をランダムに変える
    }

    void Update()
    {
        // 落ちていって下の方に行ってしまったものは破棄する
        if (this.transform.position.y < -10f)
        {
            Destroy(this.gameObject);
        }
    }
}
