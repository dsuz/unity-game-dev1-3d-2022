using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// ゲームを管理するコンポーネント
/// シーン内で唯一である必要がある
/// </summary>
public class GameManager1 : MonoBehaviour
{
    /// <summary>開始時に実行してほしい処理を登録する</summary>
    [SerializeField] UnityEvent _startEvent;
    /// <summary>クリア時に実行してほしい処理を登録する</summary>
    [SerializeField] UnityEvent _gameClearEvent;
    /// <summary>コイン獲得数を表示するテキスト</summary>
    [SerializeField] Text _coinCountText;
    /// <summary>クリアに必要なコインの数</summary>
    [SerializeField] int _clearCoinCount = 3;
    /// <summary>コイン獲得数</summary>
    int _coinCount;

    void Start()
    {
        _startEvent.Invoke();
        UpdateCoinText(_coinCount);
    }

    /// <summary>
    /// コインを取った時に呼ぶ処理
    /// </summary>
    /// <returns></returns>
    public int GetCoin()
    {
        _coinCount++;
        UpdateCoinText(_coinCount);

        if (_coinCount >= _clearCoinCount)
            _gameClearEvent.Invoke();

        return _coinCount;
    }

    /// <summary>
    /// コイン獲得数の表示を更新する
    /// </summary>
    /// <param name="coinCount"></param>
    void UpdateCoinText(int coinCount)
    {
        _coinCountText.text = coinCount.ToString();
    }
}
