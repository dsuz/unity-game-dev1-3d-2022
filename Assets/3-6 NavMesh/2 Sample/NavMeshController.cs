using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    [SerializeField] float _retargetInterval = 1.2f;
    float _timer = 0;
    NavMeshAgent _agent = default;
    Transform _target = default;
    Animator _anim = default;
    // IK 関連
    [SerializeField, Range(0f, 1f)] float _weight = 0;
    [SerializeField, Range(0f, 1f)] float _bodyWeight = 0;
    [SerializeField, Range(0f, 1f)] float _headWeight = 0;
    [SerializeField, Range(0f, 1f)] float _eyesWeight = 0;
    [SerializeField, Range(0f, 1f)] float _clampWeight = 0;

    void Start()
    {
        _timer = _retargetInterval;
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _retargetInterval)
        {
            if (_target)
            {
                _agent.SetDestination(_target.position);
                _timer = 0;
            }
            else
            {
                _target = GameObject.FindGameObjectWithTag("Player")?.transform;
            }
        }    
    }

    void LateUpdate()
    {
        if (_anim)
        {
            _anim.SetFloat("Speed", _agent.velocity.magnitude);
            _anim.SetBool("IsJumping", _agent.isOnOffMeshLink);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (_target)
        {
            var lookAtTarget = _target.Find("Root/Ribs/Neck/Head");

            if (lookAtTarget)
            {
                _anim.SetLookAtWeight(_weight, _bodyWeight, _headWeight, _eyesWeight, _clampWeight);
                _anim.SetLookAtPosition(lookAtTarget.transform.position);
            }
        }
    }
}
