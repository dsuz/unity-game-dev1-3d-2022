using UnityEngine;

/// <summary>
/// ゲームを管理するコンポーネント
/// Singleton にする方法は "Level up your code with game programming patterns" でのやり方を参考にした。
/// </summary>
public class GameManager3 : MonoBehaviour
{
    /// <summary>クリアに必要なコインの数</summary>
    [SerializeField] int _clearCoinCount = 3;
    /// <summary>シングルトンのインスタンスを保存しておく static 変数</summary>
    static GameManager3 instance;
    /// <summary>コイン獲得数</summary>
    int _coinCount;
    /// <summary>クリアに必要なコインの数</summary>
    public int ClearCoinCount => _clearCoinCount;
    /// <summary>コイン獲得数</summary>
    public int CoinCount => _coinCount;

    /// <summary>
    /// コインを取った時に呼ぶ処理
    /// </summary>
    /// <returns></returns>
    public int GetCoin()
    {
        return ++_coinCount;
    }

    #region シングルトンパターンのためのコード
    public static GameManager3 Instance
    {
        get
        {
            if (!instance)
            {
                SetupInstance();
            }

            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    static void SetupInstance()
    {
        instance = FindObjectOfType<GameManager3>();

        if (!instance)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<GameManager3>();
            go.name = instance.GetType().Name;
            DontDestroyOnLoad(go);
        }
    }
    #endregion
}
