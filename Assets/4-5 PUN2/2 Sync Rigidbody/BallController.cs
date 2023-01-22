// 日本語対応済
using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviour
{
    [SerializeField] float _movePower = 1f;
    Rigidbody _rb;
    PhotonView _view;
    Vector3 _dir;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (_view.IsMine)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            _dir = new Vector3(h, 0, v).normalized;
        }
    }

    void FixedUpdate()
    {
        if (_view.IsMine)
        {
            _rb.AddForce(_dir * _movePower, ForceMode.Force);
        }
    }
}
