// 日本語対応済
using UnityEngine;
using Photon.Pun;

public class TankMovement : MonoBehaviour
{
    [SerializeField] float _movePower = 5f;
    [SerializeField] float _rotatePower = 5f; 
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
            _dir = new Vector3(h, 0, v);
        }
    }

    void FixedUpdate()
    {
        if (_view.IsMine)
        {
            _rb.AddRelativeForce(0, 0, _dir.z * _movePower, ForceMode.Force);
            _rb.AddRelativeTorque(0, _dir.x * _rotatePower, 0, ForceMode.Force);
        }
    }
}
