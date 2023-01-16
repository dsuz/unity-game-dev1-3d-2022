using UnityEngine;

/// <summary>
/// 隕石を生成するコンポーネント
/// 管理オブジェクトにアタッチする
/// </summary>
[RequireComponent(typeof(GameManager))]
public class CometGenerator : MonoBehaviour
{
    /// <summary>隕石を生成する場所</summary>
    [SerializeField] Transform[] _spawnPoints;
    /// <summary>隕石として生成するプレハブ</summary>
    [SerializeField] GameObject _cometPrefab;
    /// <summary>生成した隕石はこのオブジェクトの子オブジェクトとする</summary>
    [SerializeField] Transform _prefabGenerationRoot;
    /// <summary>ゲーム中かどうか</summary>
    bool _isInGame;
    /// <summary>隕石の生成間隔を管理するタイマー</summary>
    float _timer;
    /// <summary>次の隕石が生成されるまでの間隔</summary>
    float _interval;

    void Update()
    {
        if (!_isInGame) return;    // ゲーム中でない場合は何もしない
        _timer += Time.deltaTime;  // タイマー加算

        if (_timer > _interval)   // 間隔を越えたら
        {
            _timer = 0f;   // タイマーをリセット
            int i = Random.Range(0, _spawnPoints.Length);  // どの場所に隕石を生成するかランダムに決める
            var go = Instantiate(_cometPrefab, _spawnPoints[i].position, Quaternion.identity);    // 隕石を生成する
            go.transform.SetParent(_prefabGenerationRoot); // ルートの子オブジェクトに設定する
            _interval = Random.Range(0.5f, 1f);    // 次の隕石生成までの間隔をランダムに決める
        }
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void StartGenerate()
    {
        _isInGame = true;
    }

    /// <summary>
    /// ゲーム終了
    /// 隕石の生成を止めて、シーン内の隕石を全て消す
    /// </summary>
    public void StopGenerate()
    {
        _isInGame = false;

        // 隕石のルートオブジェクトの子オブジェクトを全て消す
        foreach (var t in _prefabGenerationRoot.GetComponentsInChildren<Transform>())
        {
            if (t != _prefabGenerationRoot)    // ルートオブジェクトは消さないように
            {
                Destroy(t.gameObject);
            }
        }
    }
}
