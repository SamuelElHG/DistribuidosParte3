using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseDBController : MonoBehaviour
{
    [SerializeField] GameObject DB;
    bool a = true;
    private void Update()
    {
        if (a)
        {
            StartCoroutine(activeDB());
            a = false;
        }
    }

    IEnumerator activeDB()
    {
        yield return new WaitForSeconds(0.2f);
        DB.SetActive(true);
    }
}
