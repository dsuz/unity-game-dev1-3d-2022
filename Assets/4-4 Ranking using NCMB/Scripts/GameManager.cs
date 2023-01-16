using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームを管理するマネージャーコンポーネント
/// 管理オブジェクトにアタッチして使う
/// </summary>
[RequireComponent(typeof(CometGenerator))]
public class GameManager : MonoBehaviour
{
    /// <summary>ミスしてもいい回数</summary>
    [SerializeField] int _maxLife = 3;
    /// <summary>スコア表示</summary>
    [SerializeField] Text _scoreText;
    /// <summary>ライフ表示</summary>
    [SerializeField] Text _lifeText;
    /// <summary>ゲーム開始ボタン</summary>
    [SerializeField] Button _startButton;
    /// <summary>ランキングシステムのプレハブ</summary>
    [SerializeField] GameObject _rankingPrefab;
    /// <summary>CometGenerator のオブジェクト</summary>
    CometGenerator _cometGenerator;
    /// <summary>スコア</summary>
    int _score;
    /// <summary>ミスしてもいい回数</summary>
    int _life;

    void Start()
    {
        _cometGenerator = GetComponent<CometGenerator>();
    }

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void StartGame()
    {
        _startButton.gameObject.SetActive(false);  // スタートボタンを消す
        _score = 0;    // スコアをリセットする
        _life = _maxLife; // ライフをリセットする
        AddScore(0);    // 表示をリセットする
        Damage(0);  // 表示をリセットする
        _cometGenerator.StartGenerate();   // 隕石の生成開始
    }

    /// <summary>
    /// 点数を追加する
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        _score += score;
        _scoreText.text = _score.ToString();
    }

    /// <summary>
    /// ライフを減らす
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        _life -= damage;
        _lifeText.text = _life.ToString();

        if (_life < 1)
        {
            GameOver();
        }
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    void GameOver()
    {
        _startButton.gameObject.SetActive(true);   // スタートボタンを表示する
        _cometGenerator.StopGenerate();    // 隕石の生成を止める

        // ランキングシステムを発動させる
        var ranking = Instantiate(_rankingPrefab);
        ranking.GetComponent<RankingManager>().SetScoreOfCurrentPlay(_score);
    }
}
