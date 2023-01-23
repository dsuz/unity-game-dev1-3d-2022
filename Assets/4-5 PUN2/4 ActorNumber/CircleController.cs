using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CircleController : MonoBehaviour
{
    [SerializeField] float _speed = 1.0f;
    [SerializeField] Text _number;
    PhotonView _view;
    Rigidbody2D _rb;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody2D>();
        // Text に ActorNumber を表示する。この処理はネットワーク越しには行わない。
        int actorNumber = _view.OwnerActorNr;    // ActorNumber は入室順に振られる。1スタートであることに注意すること。
        _number.text = actorNumber.ToString();

        // ActorNumber は以下のようなやり方でも取得できる。
        //int actNum = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    void Update()
    {
        if (!_view.IsMine) return;

        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _rb.velocity = dir * _speed;
    }
}
