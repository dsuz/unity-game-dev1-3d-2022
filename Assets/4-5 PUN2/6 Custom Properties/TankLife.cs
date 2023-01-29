// 日本語対応済
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;

public class TankLife : MonoBehaviour
{
    [SerializeField] int _initialLife = 3;
    int _life = 0;  // Life は Owner のオブジェクトでのみ管理する
    Hashtable hashtable = new Hashtable();    // System.Collections.Hashtable ではないことに注意
    PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();

        if (_view.IsMine)
        {
            _life = _initialLife;
            UpdateLife(_life);
        }
    }

    void UpdateLife(int newLife)
    {
        _life = newLife;
        hashtable["Life"] = newLife;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);   // カスタムプロパティを設定・更新する
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet") && _view.IsMine)
        {
            UpdateLife(_life - 1);  // ライフを一つ減らす
            
            if (_life < 1)
            {
                PhotonNetwork.Destroy(_view);
            }
        }
    }
}
