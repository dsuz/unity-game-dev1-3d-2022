using UnityEngine;

/// <summary>
/// コインの機能を提供する
/// </summary>
public class Coin3 : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Destroy(gameObject);
            GameManager3.Instance.GetCoin();    // シングルトンパターンだと、呼ぶ側は楽である
        }
    }
}
