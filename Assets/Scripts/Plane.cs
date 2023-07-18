using System.Collections;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] private Camera _Camera;
    [SerializeField] private Transform RocketSpawner;
    [SerializeField] private GameObject RocketPF;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                StartCoroutine(RocketSpawn(hit));
            }
        }
    }

    IEnumerator RocketSpawn(RaycastHit hit)
    {
        GameObject RocketGO = Instantiate(RocketPF, RocketSpawner.position, Quaternion.identity);
        RocketGO.transform.LookAt(hit.point, RocketGO.transform.right);
        yield return new WaitForFixedUpdate();
        RocketGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 200), ForceMode.Impulse);
    }
}
