using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    [SerializeField] float _speed = 15f;
    [SerializeField] float _lifeTime = 30f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = this.transform.forward * _speed;  // 「前」に飛ばす
        Destroy(this.gameObject, _lifeTime);
    }
}
