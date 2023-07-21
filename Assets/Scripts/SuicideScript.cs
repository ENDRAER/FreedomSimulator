using System.Collections;
using UnityEngine;

public class SuicideScript : MonoBehaviour
{
    [SerializeField] private float seconds;

    private void Start()
    {
        StartCoroutine(KillUrSelf());
    }

    private IEnumerator KillUrSelf()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
