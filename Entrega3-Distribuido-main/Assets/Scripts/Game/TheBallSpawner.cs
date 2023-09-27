using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TheBallSpawner : MonoBehaviour
{
    [SerializeField] GameObject theballs;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        transform.DOMoveX(3,3).SetLoops(-1,LoopType.Yoyo);
        StartCoroutine(spawnBalls());
    }

    IEnumerator spawnBalls()
    {
        Instantiate(theballs, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.RandomRange(2, 3));
        StartCoroutine(spawnBalls());
    }
}
