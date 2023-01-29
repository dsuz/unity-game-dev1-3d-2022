// 日本語対応済
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
