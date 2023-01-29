// 日本語対応済
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _lifeTime = 5f;
    Rigidbody _rb;
    PhotonView _view;
    float _timer = 0;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();

        if (_view.IsMine)
        {
            _rb.velocity = transform.forward * _speed;
        }
    }

    void Update()
    {
        if (!_view.IsMine) return;
        _timer += Time.deltaTime;
        
        if (_timer > _lifeTime)
        {
            PhotonNetwork.Destroy(_view);
        }
    }
}
