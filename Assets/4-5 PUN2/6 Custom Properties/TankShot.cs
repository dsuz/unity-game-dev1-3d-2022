// 日本語対応済
using Photon.Pun;
using UnityEngine;

public class TankShot : MonoBehaviour
{
    [SerializeField] Transform _muzzle;
    [SerializeField] string _bulletPrefabNane;
    PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!_view.IsMine) return;

        if (Input.GetButtonDown("Fire1"))
        {
            PhotonNetwork.Instantiate(_bulletPrefabNane, _muzzle.position, _muzzle.rotation);
        }
    }
}
