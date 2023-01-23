using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;    // IOnEventCallback を使うため
using ExitGames.Client.Photon;  // EventData を使うため
using Photon.Pun;   // PhotonNetwork を使うため
using System;

/// <summary>
/// イベントを受け取るコンポーネント（パターン A）
/// やっていること：
/// 1. MonoBehaviour の代わりに MonoBehaviourPunCallbacks クラスを継承する
/// 2. IOnEventCallback インターフェイスを継承し、IOnEventCallback.OnEvent(EventData) を実装する
/// 3. イベントが Raise されると OnEvent メソッドが呼ばれるので、呼ばれた時の処理を実装する
/// </summary>
public class ReceiveEventA : MonoBehaviourPunCallbacks, IOnEventCallback
{
    /// <summary>ログ出力のための Text</summary>
    [SerializeField] Text _logText; 

    /// <summary>
    /// イベントが Raise されると呼ばれる
    /// </summary>
    /// <param name="e">イベントデータ</param>
    void IOnEventCallback.OnEvent(EventData e)
    {
        if ((int)e.Code < 200)  // 200 以上はシステムで使われているので処理しない
        {
            // イベントで受け取った内容をログに出力する
            string message = $"Event received by { this.GetType().Name }. EventCode: {e.Code.ToString()}, Message: {e.CustomData.ToString()}, From: { e.Sender }";
            Debug.Log(message);
            _logText.text = $"{ DateTime.Now.ToString("G") } {message}\n\n" + _logText.text;
        }
    }
}
