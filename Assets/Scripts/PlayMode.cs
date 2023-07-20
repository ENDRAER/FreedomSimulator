using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PlayMode : MonoBehaviour
{
    [NonSerialized] public static PlayMode MainBridge;
    [SerializeField] private Camera planeCamera;
    [SerializeField] private Transform rocketSpawner;
    [SerializeField] private GameObject rocketPF;
    [SerializeField] private GameObject[] donkeysPF;
    [NonSerialized] private GameObject CreatedDonkey;
    [SerializeField] private GameObject arabicPF;
    [SerializeField] private GameObject[] HomesGO;
    [NonSerialized] public List<GameObject> EnemiesOnArea = new List<GameObject>();
    [SerializeField] private Coroutine spawnCoroutine;
    [Header("Interface")]
    [NonSerialized] public int Score;
    [SerializeField] public TextMeshPro ScoreText;


    private void Start()
    {
        MainBridge = this;
        spawnCoroutine = StartCoroutine(EnemySpawnerIE());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = planeCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Instantiate(rocketPF, rocketSpawner.position, Quaternion.LookRotation(hit.point - rocketSpawner.position, rocketSpawner.right))
                    .GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 200), ForceMode.Impulse);
            }
        }
    }

    IEnumerator EnemySpawnerIE()
    {
        yield return new WaitForSeconds(1);
        int randomHouse = UnityEngine.Random.Range(0, HomesGO.Length - 1);
        if(UnityEngine.Random.Range(0, 100) < 95 || CreatedDonkey != null)
            EnemiesOnArea.Add(Instantiate(arabicPF, HomesGO[randomHouse].transform.position, HomesGO[randomHouse].transform.rotation));
        else
            CreatedDonkey = Instantiate(donkeysPF[UnityEngine.Random.Range(0, donkeysPF.Length)], HomesGO[randomHouse].transform.position, HomesGO[randomHouse].transform.rotation);
        spawnCoroutine = StartCoroutine(EnemySpawnerIE()); 
    }

    public void FreezeStarter()
    {
        StartCoroutine(FreezeSpawnIE());
    }
    public IEnumerator FreezeSpawnIE()
    {
        StopCoroutine(spawnCoroutine);
        EnemiesOnArea.ForEach(GO => GO.GetComponent<NavMeshAgent>().speed = 0);
        EnemiesOnArea.ForEach(GO => GO.GetComponent<Animator>().speed = 0);

        yield return new WaitForSeconds(3);

        spawnCoroutine = StartCoroutine(EnemySpawnerIE());
        EnemiesOnArea.ForEach(GO => GO.GetComponent<NavMeshAgent>().speed = 5);
        EnemiesOnArea.ForEach(GO => GO.GetComponent<Animator>().speed = 1);
    }


    #region Interface
    public void ScoreUpdater(int addedScore)
    {
        Score += addedScore;
        ScoreText.text = "";
        for (int i = 3; i != -1; i--)
        {
            if(Score - i >= 0)
                ScoreText.text += Score - i + "\n";
        }
    }


    public void RecordsButtonDown(Transform trans)
    {
        trans.localPosition = new Vector3(0, -0.06f, 0);
    }
    public void ButtonUp(Transform trans)
    {
        trans.localPosition = new Vector3();
    }
    #endregion
}
