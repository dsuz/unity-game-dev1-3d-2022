using UnityEngine;

/// <summary>
/// コインの機能を提供する
/// </summary>
public class Coin2 : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Destroy(gameObject);
            GameManager2.Instance.GetCoin();    // シングルトンパターンだと、呼ぶ側は楽である
        }
    }
}
