using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Rigidbody goRigidbody;
    [SerializeField] private MeshRenderer goMeshRenderer;
    [SerializeField] private ParticleSystem goParticleSystem;
    [NonSerialized] private bool Activated;


    private void OnTriggerEnter(Collider other)
    {
        if (Activated) return;
        Activated = true;
        print(1);
        Physics.OverlapSphere(transform.position, 3f).Where(collider => collider.GetComponent<Enemy>() != null).ToList().
            ForEach(collider => collider.GetComponent<Enemy>().Damage(Mathf.Lerp(0f, 130, 1f / Vector3.Distance(transform.position, collider.transform.position)), transform.position));

        Destroy(goRigidbody);
        Destroy(goMeshRenderer);
        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        transform.rotation = new Quaternion();
        goParticleSystem.Play();
        StartCoroutine(UShouldKillUrSelfNOW());
    }

    private IEnumerator UShouldKillUrSelfNOW()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
