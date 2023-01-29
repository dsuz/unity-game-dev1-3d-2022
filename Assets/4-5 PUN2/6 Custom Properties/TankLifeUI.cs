// 日本語対応済
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class TankLifeUI : MonoBehaviourPunCallbacks // プロパティ更新の Callback を受け取る
{
    [SerializeField] Text _lifeText;
    PhotonView _view;

    void Awake()
    {
        _view = GetComponent<PhotonView>();

        // Life 表示をカスタムプロパティの値に更新する
        var life = _view.Owner.CustomProperties["Life"];

        if (life != null)
        {
            _lifeText.text = life.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer.ActorNumber == _view.ControllerActorNr)
        {
            _lifeText.text = changedProps["Life"].ToString();
        }   // 自分のことだったらライフの表示を同期する
    }
}
