using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMode : MonoBehaviour
{
    [NonSerialized] public static PlayMode MainBridge;
    [SerializeField] private Camera planeCamera;
    [SerializeField] private Transform rocketSpawner;
    [SerializeField] private GameObject rocketPF;
    [SerializeField] private GameObject[] donkeysPF;
    [SerializeField] private GameObject arabicPF;
    [SerializeField] private GameObject[] HomesGO;
    [NonSerialized] public List<GameObject> EnemiesOnArea = new List<GameObject>();


    private void Start()
    {
        MainBridge = this;
        StartCoroutine(EnemySpawnerIE());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = planeCamera.ScreenPointToRay(Input.mousePosition);
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
        if(UnityEngine.Random.Range(0, 100) < 95)
            EnemiesOnArea.Add(Instantiate(arabicPF, HomesGO[randomHouse].transform.position, HomesGO[randomHouse].transform.rotation));
        else
            Instantiate(donkeysPF[UnityEngine.Random.Range(0, donkeysPF.Length - 1)], HomesGO[randomHouse].transform.position, HomesGO[randomHouse].transform.rotation);
        StartCoroutine(EnemySpawnerIE()); 
    }
}
