using UnityEngine;
using Photon.Pun;

public class FistController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _growSpeed = 1f;
    PhotonView _view;
    Animator _anim;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (_view.IsMine)
        {
            Move();
            Heat();
            Grow();
        }   // この条件がポイント。自分が所有者 (owner) の時だけ入力を受け付ける
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(h, v).normalized;
        transform.Translate(dir * _moveSpeed * Time.deltaTime, Space.World);
        if (dir != Vector2.zero)
            transform.up = dir;
    }

    void Heat()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _anim.SetBool("Heat", true);
        }
        
        if (Input.GetButtonUp("Fire1"))
        {
            _anim.SetBool("Heat", false);
        }
    }

    void Grow()
    {
        if (Input.GetButton("Jump"))
        {
            transform.localScale *= (_growSpeed * Time.deltaTime + 1);
        }   // ボタンを押している間少しずつ大きくする
    }
}
