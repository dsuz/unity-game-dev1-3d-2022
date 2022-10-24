using UnityEngine;
using System.Collections;

/// <summary>
/// Rigidbody を押す時に IK を働かせる
/// トリガーを設定し、トリガーに接触した GameObject が Rigidbody である時、それを「押している」と判定する。
/// 「押している」時には右手・左手それぞれの IK ターゲットに合わせる（ウェイトを1にする）。
/// 押している対象がトリガーから出たら、IK を切る（ウェイトを０にする）。
/// </summary>
[RequireComponent(typeof(Animator))]
public class Push : MonoBehaviour
{
    /// <summary>「押す」時の右手の IK ターゲット</summary>
    [SerializeField] Transform _rightHandTarget = default;
    /// <summary>「押す」時の左手の IK ターゲット</summary>
    [SerializeField] Transform _leftHandTarget = default;
    /// <summary>右手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _rightPositionWeight = 0;
    /// <summary>右手の Rotation に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _rightRotationWeight = 0;
    /// <summary>左手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _leftPositionWeight = 0;
    /// <summary>左手の Rotation に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _leftRotationWeight = 0;
    /// <summary>現在押している Rigidbody</summary>
    Rigidbody _pushingTarget = default;
    Animator _anim = default;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        // 右手に対して IK を設定する
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightPositionWeight);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightRotationWeight);
        _anim.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTarget.rotation);
        // 左手に対して IK を設定する
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftPositionWeight);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftRotationWeight);
        _anim.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTarget.rotation);
    }

    /// <summary>
    /// 指定した値にウェイトを変更する
    /// </summary>
    /// <param name="targetWeight"></param>
    void ChangeWeight(float targetWeight)
    {
        _rightPositionWeight = targetWeight;
        _rightRotationWeight = targetWeight;
        _leftPositionWeight = targetWeight;
        _leftRotationWeight = targetWeight;
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (rb)
        {
            _pushingTarget = rb;
            ChangeWeight(1f);
            // 腕を滑らかに上げ下げしたい時は次のように段階的に変化させる
            // ChangeWeight(1f, 0.03f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (_pushingTarget == rb)
        {
            _pushingTarget = null;
            ChangeWeight(0f);
            // 腕を滑らかに上げ下げしたい時は次のように段階的に変化させる
            // ChangeWeight(0f, 0.03f);
        }
    }

    /// <summary>
    /// 指定した値にウェイトを step ずつ変更する
    /// </summary>
    /// <param name="targetWeight"></param>
    /// <param name="step"></param>
    void ChangeWeight(float targetWeight, float step)
    {
        StartCoroutine(ChangeWeightRoutine(targetWeight, step));
    }

    /// <summary>
    /// 指定した値にウェイトを step ずつ変更する
    /// </summary>
    /// <param name="targetWeight"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    IEnumerator ChangeWeightRoutine(float targetWeight, float step)
    {
        if (_rightPositionWeight < targetWeight)
        {
            while (_rightPositionWeight < targetWeight)
            {
                _rightPositionWeight += step;
                _rightRotationWeight = _rightPositionWeight;
                _leftPositionWeight = _rightPositionWeight;
                _leftRotationWeight = _rightPositionWeight;
                yield return null;
            }
        }
        else
        {
            while (_rightPositionWeight > targetWeight)
            {
                _rightPositionWeight -= step;
                _rightRotationWeight = _rightPositionWeight;
                _leftPositionWeight = _rightPositionWeight;
                _leftRotationWeight = _rightPositionWeight;
                yield return null;
            }
        }
    }
}
