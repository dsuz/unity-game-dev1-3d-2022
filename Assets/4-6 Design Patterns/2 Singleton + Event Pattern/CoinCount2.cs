using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コイン獲得数表示を制御するコンポーネント
/// </summary>
[RequireComponent(typeof(Text))]
public class CoinCount2 : MonoBehaviour
{
    Text _text;

    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = "0";
    }

    void OnEnable()
    {
        GameManager2.Instance.OnGetCoin += UpdateCounter;
    }

    void OnDisable()
    {
        GameManager2.Instance.OnGetCoin -= UpdateCounter;
    }

    void UpdateCounter(int coinCount)
    {
        _text.text = coinCount.ToString();
    }
}
