using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;   // PhotonNetwork を使うため
using Photon.Realtime;  // RaiseEventOptions/ReceiverGroup を使うため
using ExitGames.Client.Photon;  // SendOptions を使うため

/// <summary>
/// イベントを raise/fire するためのコンポーネント
/// イベントを起こすには PhotonNetwork.RaiseEvent メソッドを呼び出す。
/// イベントを起こすコンポーネントはネットワークコンポーネントやオブジェクトである必要はない。
/// （MonoBehaviourPunCallbacks を継承したり、Photon View をアタッチする必要はない）
/// </summary>
public class RaiseEvent : MonoBehaviour
{
    [SerializeField] InputField _eventCode;
    [SerializeField] InputField _eventMessage;
    [SerializeField] Dropdown _sendTarget;

    public void Raise()
    {
        byte eventCode = byte.Parse(_eventCode.text);   // イベントコードは 0~199 まで指定できる。200 以上はシステムで使われているので使えない。
        ReceiverGroup target = (ReceiverGroup)_sendTarget.value;    // ドロップダウンの選択肢によって誰がイベントを受け取るか指定する
        Raise(eventCode, _eventMessage.text, target);
    }

    /// <summary>
    /// イベントを発生させる
    /// </summary>
    /// <param name="eventCode">イベントのコード (ID)</param>
    /// <param name="eventMessage">メッセージ</param>
    /// <param name="target">誰にイベントを送るか（全員, マスタークライアント, 自分以外）</param>
    void Raise(byte eventCode, string eventMessage, ReceiverGroup target)
    {
        //イベントとして送る準備をする
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.Receivers = target;
        // イベントを起こす
        PhotonNetwork.RaiseEvent(eventCode, eventMessage, raiseEventOptions, new SendOptions());
    }
}