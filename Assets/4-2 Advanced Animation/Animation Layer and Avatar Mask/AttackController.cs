using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AttackController : MonoBehaviour
{
    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    /// <summary>
    /// トリガーをセットする
    /// </summary>
    /// <param name="name">トリガー名</param>
    public void SetTrigger(string name)
    {
        _anim.SetTrigger(name);
    }
}
