using System.Linq;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private CapsuleCollider CapsuleCol;


    private void OnTriggerEnter(Collider other)
    {
        Destroy(CapsuleCol);
        print(Physics.OverlapSphere(transform.position, 1.3f, 6).Length);
        Physics.OverlapSphere(transform.position, 1.3f, 0).ToList().ForEach(_collider => Destroy(_collider.gameObject));
        Destroy(gameObject);
    }
}
