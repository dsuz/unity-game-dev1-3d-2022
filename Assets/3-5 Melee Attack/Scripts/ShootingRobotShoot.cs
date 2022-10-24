using System.Collections;
using UnityEngine;

/// <summary>
/// LineRenderer を使ってレーザービームを発射するコンポーネント
/// </summary>
public class ShootingRobotShoot : MonoBehaviour
{
    /// <summary>レーザーの発射位置</summary>
    [SerializeField] Transform _muzzle = default;
    [SerializeField] LineRenderer _line = default;
    [SerializeField] float _lineLength = 5f;
    /// <summary>レーザーが表示される時間（秒）</summary>
    [SerializeField] float _flashInterval = 0.05f;
    /// <summary>作動させるボタン</summary>
    [SerializeField] string _fireButton = "Fire2";

    void Update()
    {
        if (Input.GetButtonDown(_fireButton))
        {
            StartCoroutine(ShootRoutine());
        }
    }

    IEnumerator ShootRoutine()
    {
        // ラインの終端を移動させ、待ち、その後、始点と終点を同じ座標にすることで消す
        _line.SetPosition(0, _muzzle.position);
        Vector3 lineTarget = _muzzle.position + this.transform.forward * _lineLength;
        _line.SetPosition(1, lineTarget);
        yield return new WaitForSeconds(_flashInterval);
        _line.SetPosition(1, _line.GetPosition(0));
    }
}
