using UnityEngine;

/// <summary>
/// クリックで Ray を飛ばして、Ray を Scene ウィンドウに表示するコンポーネント
/// 適当な GameObject に追加して使う
/// </summary>
public class MarkerController : MonoBehaviour
{
    /// <summary>Ray が何にも当たらなかった時、Scene に表示する Ray の長さ</summary>
    [SerializeField] float _debugRayLength = 100f;
    /// <summary>Ray が何かに当たった時に Scene に表示する Ray の色</summary>
    [SerializeField] Color _debugRayColorOnHit = Color.red;
    /// <summary>ここに GameObject を設定すると、飛ばした Ray が何かに当たった時にそこに m_marker オブジェクトを移動する</summary>
    [SerializeField] GameObject _marker;
    /// <summary>飛ばした Ray が当たった座標に m_marker を移動する際、Ray が当たった座標からどれくらいずらした場所に移動するかを設定する</summary>
    [SerializeField] Vector3 _markerOffset = Vector3.up * 0.01f;

    void Update()
    {
        // クリックで Ray を飛ばす
        if(Input.GetButtonDown("Fire1"))
        {
            // カメラの位置 → マウスでクリックした場所に Ray を飛ばすように設定する
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Ray が当たったかどうかで異なる処理をする（Physics.Raycast() にはたくさんオーバーロードがあるので注意すること）
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Ray が当たった時は、当たった座標まで赤い線を引く
                Debug.DrawLine(ray.origin, hit.point, _debugRayColorOnHit);
                // m_marker がアサインされていたら、それを移動する
                if (_marker)
                {
                    _marker.transform.position = hit.point + _markerOffset;
                }
            }
            else
            {
                // Ray が当たらなかった場合は、Ray の方向に白い線を引く
                Debug.DrawRay(ray.origin, ray.direction * _debugRayLength);
            }
        }
    }
}
