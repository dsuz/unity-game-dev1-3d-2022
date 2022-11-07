using UnityEngine;
using UnityEngine.AI;   // Navmesh Agent を使うために必要

/// <summary>
/// Navmesh Agent を使って経路探索を行い、移動するためのコンポーネント
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerControllerAi : MonoBehaviour
{
    /// <summary>移動先となる位置情報</summary>
    [SerializeField] Transform _target = default;
    /// <summary>移動先座標を保存する変数</summary>
    Vector3 _cachedTargetPosition;
    /// <summary>キャラクターなどのアニメーションするオブジェクトを指定する</summary>
    [SerializeField] Animator _animator = default;
    NavMeshAgent _agent = default;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _cachedTargetPosition = _target.position; // 初期位置を保存する（※）
    }

    /*
     * （※）_cachedTargetPosition を使って座標を保存しているのは、以下の Update() 内で「毎フレーム座標をセットする」という処理を避け、負荷を下げるためである。
     * 毎フレーム座標をセットすることで経路の計算を毎フレームしてしまうことを避けるため、「Target が移動した時のみ」目的地をセットして経路の計算を行わせている。
     */

    void Update()
    {
        // m_target が移動したら Navmesh Agent を使って移動させる
        if (Vector3.Distance(_cachedTargetPosition, _target.position) > Mathf.Epsilon) // _target が移動したら
        {
            _cachedTargetPosition = _target.position; // 移動先の座標を保存する
            _agent.SetDestination(_cachedTargetPosition); // Navmesh Agent に目的地をセットする（Vector3 で座標を設定していることに注意。Transform でも GameObject でもなく、Vector3 で目的地を指定する）
        }

        // m_animator がアサインされていたら Animator Controller にパラメーターを設定する
        if (_animator)
        {
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
        }
    }
}
