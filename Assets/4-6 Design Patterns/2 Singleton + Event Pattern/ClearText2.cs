using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// クリアーの表示を制御するコンポーネント
/// </summary>
[RequireComponent(typeof(Text))]
public class ClearText2 : MonoBehaviour
{
    Text _text;

    void Start()
    {
        // 文字を消す（GameObject を非アクティブにするとイベントを受け取れないので Text を無効にする）
        _text = GetComponent<Text>();

        if (_text)
            _text.enabled = false;
    }

    void OnEnable()
    {
        GameManager2.Instance.OnClear += ShowText;
    }

    void OnDisable()
    {
        GameManager2.Instance.OnClear -= ShowText;
    }

    void ShowText()
    {
        _text.enabled = true;
    }
}
