using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    [SerializeField] Transform[] _spawnPoints = default;
    [SerializeField] GameObject _prefab = default;
    [SerializeField] float _interval = 2f;

    void Start()
    {
        StartCoroutine(GenerateRoutine());
    }
    
    IEnumerator GenerateRoutine()
    {
        while (true)
        {
            int i = Random.Range(0, _spawnPoints.Length);
            var go = Instantiate(_prefab);
            go.transform.position = _spawnPoints[i].position;
            yield return new WaitForSeconds(_interval);
        }
    }
}
