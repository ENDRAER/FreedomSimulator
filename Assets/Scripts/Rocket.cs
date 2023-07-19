using System.Collections;
using System.Linq;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Rigidbody goRigidbody;
    [SerializeField] private MeshRenderer goMeshRenderer;
    [SerializeField] private ParticleSystem goParticleSystem;


    private void OnTriggerEnter(Collider other)
    {
        Physics.OverlapSphere(transform.position, 3f).Where(collider => collider.GetComponent<Arabic>() != null).ToList().ForEach(collider => collider.GetComponent<Arabic>().Damage(Mathf.Lerp(0f, 130, 1f / Vector3.Distance(transform.position, collider.transform.position)), transform.position));
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
