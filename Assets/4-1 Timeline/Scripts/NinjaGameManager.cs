using UnityEngine;
using UnityEngine.Playables;    // Timeline をスクリプトからコントロールするために必要
using Cinemachine;

/// <summary>
/// ゲーム全体を管理するコンポーネント
/// カットシーンの再生をしてからゲームを開始する
/// </summary>
public class NinjaGameManager : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab = null;
    /// <summary>ゲーム開始時に再生する PlayableDirector</summary>
    [SerializeField] PlayableDirector _openingCutScene = null;
    /// <summary>ゲームの状態</summary>
    GameState _state = GameState.None;

    void Update()
    {
        switch (_state)
        {
            // オープニングを再生する
            case GameState.None:
                if (_openingCutScene)
                {
                    _openingCutScene.Play();
                }
                _state = GameState.Opening;
                break;
            // オープニングの再生が終わったらゲームを開始する
            case GameState.Opening:
                if (_openingCutScene && _openingCutScene.state != PlayState.Playing)
                {
                    _openingCutScene.gameObject.SetActive(false);
                    StartGame();
                }
                else if (!_openingCutScene)
                {
                    StartGame();
                }
                break;
        }
    }

    void StartGame()
    {
        _state = GameState.InGame;
        var p = Instantiate(_playerPrefab, Vector3.zero, _playerPrefab.transform.rotation);
        // vCam で生成したプレイヤーを追う
        var vCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        vCam.LookAt = p.transform;
        vCam.Follow = p.transform;
    }
}

/// <summary>
/// ゲームの状態を表す
/// </summary>
public enum GameState
{
    /// <summary>ゲーム起動時など</summary>
    None,
    /// <summary>オープニングカットシーン再生中</summary>
    Opening,
    /// <summary>ゲーム中</summary>
    InGame,
}