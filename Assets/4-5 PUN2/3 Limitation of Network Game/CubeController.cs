using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using System.Linq;
using System.Text;
using System;

public class CubeController : MonoBehaviour
{
    [SerializeField] float _speed = 1.0f;
    [SerializeField] CinemachineVirtualCameraBase _vCam;
    [SerializeField] Text _rankingText; 
    Rigidbody _rb;
    PhotonView _view;
    float _random;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _view = GetComponent<PhotonView>();
        _random = UnityEngine.Random.value;

        if (_view.IsMine)
        {
            _vCam.MoveToTopOfPrioritySubqueue();
        }
        else
        {
            _vCam.enabled = false;
            _rankingText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!_view.IsMine) return;

        // クリックしたら発信する
        if (Input.GetButtonDown("Fire1"))
            _rb.velocity = transform.forward * _speed;

        // ランダムにスピードを変える
        if (_rb.velocity.magnitude > Mathf.Epsilon)
            _rb.velocity = transform.forward * Mathf.PerlinNoise(Time.time * _random, 0) * _speed;
    }

    void FixedUpdate()
    {
        if (!_view.IsMine) return;
        
        // 順位表を作る
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        var orderedViews = players.Select(p => p.GetComponent<PhotonView>())
            .OrderByDescending(p => p.transform.position.z).ToArray();
        StringBuilder builder = new StringBuilder();
        Array.ForEach(orderedViews, p => builder.AppendLine("Player " + p.OwnerActorNr.ToString()));
        _rankingText.text = builder.ToString();
    }
}
