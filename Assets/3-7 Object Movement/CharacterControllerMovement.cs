using UnityEngine;

/// <summary>
/// Character Controller コンポーネントを使ってオブジェクトを動かす
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    /// <summary>動かすのに使う方法</summary>
    [SerializeField] CharacterControllerMoveMethod _moveMethod = CharacterControllerMoveMethod.Move;
    /// <summary>移動速度</summary>
    [SerializeField] float _speed = 3f;
    CharacterController _controller = default;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 入力を受け取り、カメラを基準にした XZ 平面上に変換する
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;

        switch (_moveMethod)
        {
            case CharacterControllerMoveMethod.Move:
                _controller.Move(dir.normalized * _speed * Time.deltaTime);

                //if (_controller.isGrounded)
                //{
                //    Debug.Log("接地しています");
                //}
                //else
                //{
                //    Debug.Log("接地していません");
                //}

                break;
            case CharacterControllerMoveMethod.SimpleMove:
                _controller.SimpleMove(dir.normalized * _speed);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Character Controller を使った衝突判定のコールバック関数
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log($"{hit.collider.name} に衝突した(OnControllerColliderHit)");
    }

    /// <summary>
    /// Character Controller は Collider ではなく Rigidbody も必要ないので、OnCollisionEnter は呼ばれない
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.gameObject.name} に衝突した(OnCollisionEnter)");
    }
}

/// <summary>Character Controller を使って動かす時にどの方法を使うか</summary>
enum CharacterControllerMoveMethod
{
    /// <summary>Move メソッドを使う</summary>
    Move,
    /// <summary>SimpleMove メソッドを使う</summary>
    SimpleMove,
}
