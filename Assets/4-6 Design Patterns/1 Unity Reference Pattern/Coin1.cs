using UnityEngine;

/// <summary>
/// コインの機能を提供する
/// </summary>
public class Coin1 : MonoBehaviour
{
    GameManager1 gameManager;

    void Start()
    {
        // 各コインから GameManager を参照する
        GameObject go = GameObject.FindGameObjectWithTag("GameController");

        if (go)
            gameManager = go.GetComponent<GameManager1>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Destroy(gameObject);
            gameManager?.GetCoin();
        }
    }
}
