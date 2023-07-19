using System.Collections;
using UnityEngine;

public class PlayMode : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform rocketSpawner;
    [SerializeField] private GameObject rocketPF;
    [SerializeField] private GameObject arabicPF;
    [SerializeField] private GameObject[] HomesGO;


    private void Start()
    {
        StartCoroutine(EnemySpawnerIE());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                StartCoroutine(RocketSpawnIE(hit));
            }
        }
    }

    IEnumerator RocketSpawnIE(RaycastHit hit)
    {
        GameObject RocketGO = Instantiate(rocketPF, rocketSpawner.position, Quaternion.identity);
        RocketGO.transform.LookAt(hit.point, RocketGO.transform.right);
        yield return new WaitForFixedUpdate();
        RocketGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 200), ForceMode.Impulse);
    }

    IEnumerator EnemySpawnerIE()
    {
        yield return new WaitForSeconds(2);
        int randomHouse = UnityEngine.Random.Range(0, HomesGO.Length - 1);
        Instantiate(arabicPF, HomesGO[randomHouse].transform.position, HomesGO[randomHouse].transform.rotation);
        StartCoroutine(EnemySpawnerIE());
    }
}
