using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// スイッチの機能を提供するコンポーネント
/// Player タグがついたオブジェクトがトリガーに侵入すると、登録した UnityEvent を一度だけ実行する。
/// </summary>
public class SwitchController : MonoBehaviour
{
    /// <summary>Player が侵入した時に実行すること</summary>
    [SerializeField] UnityEvent _onEnter = default;
    /// <summary>一度動作したかどうか</summary>
    bool _isFinished = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!_isFinished)
            {
                _onEnter.Invoke();
                _isFinished = true;
            }
        }
    }
}
