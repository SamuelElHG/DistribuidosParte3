using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemy : MonoBehaviour
{
    [SerializeField] GameObject targetPlayer;

    private void Start()
    {
        transform.position = targetPlayer.transform.position;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPlayer.transform.position, 5f);
    }
}
