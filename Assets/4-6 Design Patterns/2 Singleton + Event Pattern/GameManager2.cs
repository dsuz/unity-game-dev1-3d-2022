using System;
using UnityEngine;

/// <summary>
/// ゲームを管理するコンポーネント
/// Singleton にする方法は "Level up your code with game programming patterns" でのやり方を参考にした。
/// </summary>
public class GameManager2 : MonoBehaviour
{
    /// <summary>クリアに必要なコインの数</summary>
    [SerializeField] int _clearCoinCount = 3;
    /// <summary>シングルトンのインスタンスを保存しておく static 変数</summary>
    static GameManager2 instance;
    /// <summary>コイン取得時に発生するイベントのデリゲート</summary>
    event Action<int> _onGetCoin;
    /// <summary>ゲームクリア自に発生するイベントのデリゲート</summary>
    event Action _onClear;
    /// <summary>コイン獲得数</summary>
    int _coinCount;

    /// <summary>コイン取得イベント</summary>
    public Action<int> OnGetCoin
    {
        get => _onGetCoin;
        set => _onGetCoin = value;
    }   // ラムダ式でプロパティを記述する

    /// <summary>ゲームクリアイベント</summary>
    public Action OnClear
    {
        get => _onClear;
        set => _onClear = value;
    }   // ラムダ式でプロパティを記述する

    /// <summary>
    /// コインを取った時に呼ぶ処理
    /// </summary>
    /// <returns></returns>
    public int GetCoin()
    {
        _coinCount++;
        _onGetCoin?.Invoke(_coinCount);  // コインの取得を通知する

        if (_coinCount >= _clearCoinCount)
        {
            _onClear?.Invoke(); // クリアを通知する
        }

        return _coinCount;
    }

    #region シングルトンパターンのためのコード
    public static GameManager2 Instance
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
        instance = FindObjectOfType<GameManager2>();

        if (!instance)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<GameManager2>();
            go.name = instance.GetType().Name;
            DontDestroyOnLoad(go);
        }
    }
    #endregion
}
