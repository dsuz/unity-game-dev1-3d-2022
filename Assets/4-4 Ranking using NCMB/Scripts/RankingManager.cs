using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;         // NCMB を使うため

/// <summary>
/// ランキングシステムを管理するクラス
/// </summary>
public class RankingManager : MonoBehaviour
{
    /// <summary>ランキングを表示する Text</summary>
    [SerializeField] Text _rankingText;
    /// <summary>名前を入力するフィールド</summary>
    [SerializeField] InputField _nameInput;
    /// <summary>名前の登録を行うためのオブジェクトが配置されたパネル</summary>
    [SerializeField] RectTransform _entryPanel;
    /// <summary>最初にrankingを閉じさせない秒数</summary>
    [SerializeField] float _gracePeriod = 10f;
    /// <summary>ランキング情報の配列</summary>
    List<NCMBObject> _ranking;
    /// <summary>今回のスコア</summary>
    int _score;
    float _timer;
    /// <summary>画面を閉じてもよいか</summary>
    bool _closable = false;

    void Update()
    {
        // しばらくはクリックしても閉じさせないように制御する
        if (!_closable)
        {
            _timer += Time.deltaTime;

            if (_timer > _gracePeriod)
            {
                _closable = true;
            }
        }
    }

    /// <summary>
    /// ランキングシステムを閉じる
    /// </summary>
    public void CloseRanking()
    {
        if (_closable && !_entryPanel.gameObject.activeSelf)    // エントリーが表示されている間は閉じさせない
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// ランキングを取得する
    /// ランキングをサーバーから取ってきて、10 位以内に入っていたら名前を入力する画面を表示する
    /// </summary>
    /// <param name="score">今回のスコア。0 の場合はランキング入力画面は出ない</param>
    public void GetRanking(int score)
    {
        _score = score;
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
        query.OrderByDescending("Score");
        query.Limit = 10;   // 上位10件を取得する

        // 検索する https://mbaas.nifcloud.com/doc/current/datastore/ranking_unity.html#%E3%83%A9%E3%83%B3%E3%82%AD%E3%83%B3%E3%82%B0%E3%81%AE%E5%8F%96%E5%BE%97
        query.FindAsync((objList, e) =>
        {
            if (e != null)
            {
                Debug.LogError(e.ToString());
            }   // エラーの場合
            else
            {
                // 結果を保存する
                _ranking = objList;
                // ランキングを表示する
                MakeRankingText();

                // ランキングの一番下より点数が大きい場合は
                if ((score > 0 && _ranking.Count < 10) || score > int.Parse(_ranking[_ranking.Count - 1]["Score"].ToString()) || _ranking.Count == 0)
                {
                    _entryPanel.gameObject.SetActive(true);    // エントリーパネルを表示する
                }
            }   // 正常終了（エラーではない）場合
        });
    }

    /// <summary>
    /// ランキング情報の配列から、ランキング情報のテキストを作って表示する
    /// </summary>
    void MakeRankingText()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        for (int i = 0; i < _ranking.Count; i++)
        {
            builder.Append("<color=" + (i % 2 == 0 ? "yellow>" : "cyan>"));
            builder.Append((i + 1).ToString().PadLeft(2));  // 桁を揃える
            builder.Append(" : ");
            string name = _ranking[i]["Name"].ToString();
            name = name.Length > 10 ? name.Substring(0, 10) : name.PadRight(10);    // 名前が10文字以上ならば切り捨てる。10文字未満ならば右側をスペースで埋める（パディング）
            builder.Append(name);
            builder.Append(" : ");
            builder.Append(_ranking[i]["Score"].ToString().PadLeft(4));    // 桁を揃える
            builder.AppendLine("</color>");
        }

        Debug.Log("Ranking Text:\r\n" + builder.ToString());    // デバッグ用にログを出す
        _rankingText.text = builder.ToString();
    }

    /// <summary>
    /// 今回のスコアを登録する。ランキングシステムを呼び出したら、まずこの関数を呼び出す。
    /// これを行うと、ランキングの取得や表示が始まる
    /// </summary>
    /// <param name="score"></param>
    public void SetScoreOfCurrentPlay(int score)
    {
        _rankingText.text = "";    // 表示をクリアする
        GetRanking(score);
    }

    /// <summary>
    /// ハイスコアの名前登録を行う
    /// </summary>
    public void Entry()
    {
        // 保存するためのデータを作る
        NCMBObject obj = new NCMBObject("HighScore");
        obj["Name"] = _nameInput.text;
        obj["Score"] = _score;
        // データを保存する https://mbaas.nifcloud.com/doc/current/datastore/ranking_unity.html#%E3%82%B9%E3%82%B3%E3%82%A2%E3%81%AE%E4%BF%9D%E5%AD%98
        obj.SaveAsync(e =>
        {
            if (e != null)
            {
                Debug.LogError(e.ToString());
            }   // エラーの場合
            else
            {
                // エントリー画面を消してランキングをリロードする
                _entryPanel.gameObject.SetActive(false);   // エントリー画面を消す
                GetRanking(0);  // ランキングをリロードする
            }   // 正常終了（エラーではない）場合
        });
    }
}
