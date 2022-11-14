using UnityEngine;

/// <summary>
/// Transform コンポーネントを使ってオブジェクトを動かす
/// </summary>
public class TransformMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;
    Vector3 _dir = default;

    void Update()
    {
        // 入力を受け取り、カメラを基準にした XZ 平面上に変換する
        _dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _dir = Camera.main.transform.TransformDirection(_dir);
        _dir.y = 0;

        this.transform.Translate(_dir * _moveSpeed * Time.deltaTime);
    }
}
