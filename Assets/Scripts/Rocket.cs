using System;
using System.Linq;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject explosionPF;
    [NonSerialized] private bool Activated;


    private void OnTriggerEnter(Collider other)
    {
        if (Activated) return;
        Activated = true;
        Physics.OverlapSphere(transform.position, 3f).Where(collider => collider.GetComponent<Enemy>() != null).ToList().
            ForEach(collider => collider.GetComponent<Enemy>().Damage(Mathf.Lerp(0f, 130, 1f / Vector3.Distance(transform.position, collider.transform.position)), transform.position));

        Instantiate(explosionPF, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), new Quaternion());
        Destroy(gameObject);
    }
}
