using System.Collections;
using UnityEngine;

/// <summary>
/// 操作しているロボットにパンチ（モーションのある攻撃）をさせるためのコンポーネント
/// </summary>
[RequireComponent(typeof(Animator), typeof(ShootingRobotMove))]
public class ShootingRobotPunch : MonoBehaviour
{
    /// <summary>攻撃（パンチ）ボタン</summary>
    [SerializeField] string _attackButton = "Fire1";
    /// <summary>攻撃時に移動不可にする秒数</summary>
    [SerializeField] float _freezeSecondsOnAttack = 0.3f;
    /// <summary>攻撃判定となるトリガー</summary>
    [SerializeField] Collider _attackTrigger;
    /// <summary>攻撃が当たった時に加える力</summary>
    [SerializeField] float _attackPower = 20f;
    Animator _anim;
    ShootingRobotMove _move;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _move = GetComponent<ShootingRobotMove>();
    }

    void Update()
    {
        if (Input.GetButtonDown(_attackButton))
        {
            _move?.Freeze(_freezeSecondsOnAttack, () => _anim.SetBool("Attack", false));
            _anim?.SetBool("Attack", true);
            // 上のやり方は若干難しい（攻撃時の Freeze をより厳密に行っている）が、より簡単な Trigger で遷移させたい場合は以下のようにする
            //_move.Freeze(_freezeSecondsOnAttack);
            //_anim.SetTrigger("AttackTrigger");
        }
    }

    /// <summary>
    /// 攻撃判定を起こす
    /// Animation Event から呼ばれる
    /// </summary>
    void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        // トリガーをアクティブにして、物理演算を待って非アクティブに戻す
        _attackTrigger.gameObject.SetActive(true);
        yield return new WaitForFixedUpdate();
        _attackTrigger.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // 接触したオブジェクトが Rigidbody を持っていたら力を加える
        other.GetComponent<Rigidbody>()?.AddForce(transform.forward * _attackPower, ForceMode.Impulse);
    }
}
