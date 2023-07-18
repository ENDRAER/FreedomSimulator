using System.Linq;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private CapsuleCollider CapsuleCol;


    private void OnTriggerEnter(Collider other)
    {
        Physics.OverlapSphere(transform.position, 3f).Where(collider => collider.GetComponent<Arabic>() != null).ToList().ForEach(collider => collider.GetComponent<Arabic>().Damage(120, transform.position));
        Destroy(gameObject);
    }
}
