using UnityEngine;

/// <summary>
/// 迎撃ミサイルを制御するコンポーネント
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class ShotController : MonoBehaviour
{
    ParticleSystem _particle;
    GameManager _gameManager;

    void Start()
    {
        // 使用するオブジェクト/コンポーネントの参照を取っておく
        _particle = GetComponent<ParticleSystem>();
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        
        if (!_gameManager)
        {
            Debug.LogError("Not found GameManager. GameManager must be attached to the GameObject with Tag: GameController");
        }
    }

    void Update()
    {
        // パーティクルが再生中じゃない時にクリックしたら弾を発射する
        if (_particle.isStopped && Input.GetButtonDown("Fire1"))
        {
            // クリックした場所に移動して
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos += Vector3.forward;
            this.transform.position = pos;
            // パーティクルを再生する
            _particle.Play();
        }
    }

    /// <summary>
    /// パーティクルのコライダーに衝突があった時に呼ばれる関数
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {
        // 衝突相手が隕石だったら
        if (other.gameObject.tag == "Respawn")
        {
            Destroy(other.gameObject);  // 隕石を破壊し
            _gameManager.AddScore(1);  // スコアを加算する
        }
    }
}
