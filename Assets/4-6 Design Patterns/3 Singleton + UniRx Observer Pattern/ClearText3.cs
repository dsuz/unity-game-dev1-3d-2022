using UniRx;
using UnityEngine;

/// <summary>
/// クリアーの表示を制御するコンポーネント
/// </summary>
public class ClearText3 : MonoBehaviour
{
    void Start()
    {
        // 文字を消す
        gameObject.SetActive(false);

        // コインの数がクリアに必要な数になったらテキストを表示する
        GameManager3.Instance.ObserveEveryValueChanged(gm => gm.CoinCount)          // ここでプロパティの監視を設定している
            .Where(coinCount => coinCount >= GameManager3.Instance.ClearCoinCount)  // ここで条件を指定する
            .Subscribe(_ => gameObject.SetActive(true));  // ここで条件を満たした時に実行する処理を登録している
    }
}
