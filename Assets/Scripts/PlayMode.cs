using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayMode : MonoBehaviour
{
    [NonSerialized] public static PlayMode playMode;
    [SerializeField] private Camera planeCamera;
    [SerializeField] private Transform rocketSpawner;
    [SerializeField] private GameObject rocketPF;
    [SerializeField] private GameObject[] donkeysPF;
    [NonSerialized] private GameObject CreatedDonkey;
    [SerializeField] private GameObject arabicPF;
    [SerializeField] private GameObject[] HomesGO;
    [NonSerialized] public List<GameObject> EnemiesOnArea = new List<GameObject>();
    [SerializeField] private Coroutine spawnCoroutine;
    [NonSerialized] public float SpawnSpeed = 3;
    [NonSerialized] public float EnemyHealth = 100;
    [NonSerialized] public float Damage = 120;
    [NonSerialized] public float MaxReloadTime = 1;
    [NonSerialized] public float ReloadTime = 1;
    [NonSerialized] public int Score;
    [NonSerialized] private bool gamePlay;
    [Header("UI")]
    [SerializeField] public GameObject NoSignalGO;
    [SerializeField] public TextMeshProUGUI ScoreText;
    [SerializeField] public GameObject EnemiesCounter;
    [SerializeField] public Image ReloadTimeProgressbar;
    [SerializeField] public GameObject FrozenBounds;
    [SerializeField] public TextMeshProUGUI UpgradeCallerText;
    [SerializeField] public Animator UpgradeCallerAnimator;


    private void Awake()
    {
        playMode = this;
    }

    private void Update()
    {
        if (MaxReloadTime <= ReloadTime)
        {
            ReloadTime = MaxReloadTime;
            ReloadTimeProgressbar.color = Color.green;
            if (Physics.Raycast(planeCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && Input.GetKeyDown(KeyCode.Mouse0) && gamePlay)
            {
                ReloadTime = 0;
                ReloadTimeProgressbar.color = Color.red;
                GameObject rocketGO = Instantiate(rocketPF, rocketSpawner.position, Quaternion.LookRotation(hit.point - rocketSpawner.position, rocketSpawner.right));
                rocketGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 200), ForceMode.Impulse);
                rocketGO.GetComponent<Rocket>().damage = Damage;
            }
        }
        else if (MaxReloadTime > ReloadTime)
            ReloadTime += Time.deltaTime;

        ReloadTimeProgressbar.transform.localPosition = new Vector3(-0.899f, -1.95f, -1.7f + Mathf.Clamp(1.7f / MaxReloadTime * ReloadTime, 0, 1.7f));
    }

    protected IEnumerator EnemySpawnerIE()
    {
        yield return new WaitForSeconds(SpawnSpeed);
        SpawnSpeed /= 1.02f;
        EnemyHealth *= 1.02f;
        if (UnityEngine.Random.Range(0, 100) < 80 || CreatedDonkey != null)
        {
            EnemiesOnArea.Add(Instantiate(arabicPF, HomesGO[UnityEngine.Random.Range(0, HomesGO.Length - 1)].transform.position, new Quaternion()));
            EnemiesOnArea[EnemiesOnArea.Count - 1].GetComponent<Enemy>().health = EnemyHealth;
        }
        else
            EnemiesOnArea.Add(CreatedDonkey = Instantiate(donkeysPF[UnityEngine.Random.Range(0, donkeysPF.Length)], HomesGO[UnityEngine.Random.Range(0, HomesGO.Length - 1)].transform.position, new Quaternion()));

        if (EnemiesOnArea.Count < 10)
            spawnCoroutine = StartCoroutine(EnemySpawnerIE());
        else
            EndGame(); 

        EnemiesCounter.transform.localEulerAngles = new Vector3(0, -90 + (18 * EnemiesOnArea.Count), 0);
    }

    public void ScoreUpdater(int addedScore)
    {
        Score += addedScore;
        ScoreText.text = "";
        for (int i = 3; i != -1; i--)
        {
            if (Score - i >= 0)
                ScoreText.text += Score - i + "\n";
        }
    }

    public void StartGame()
    {
        if (gamePlay)
            return;
        gamePlay = true;
        spawnCoroutine = StartCoroutine(EnemySpawnerIE());
    }

    public void EndGame()
    {
        EnemiesOnArea.ForEach(enemies => Destroy(enemies));
        EnemiesOnArea.Clear();
        Destroy(CreatedDonkey);
        NoSignalGO.SetActive(true);
        gamePlay = false;

        List<int> allRecords = new List<int>();
        string ScoreString = PlayerPrefs.GetString("TopScores");
        if (ScoreString != "")
            ScoreString.Split('\n').Select(a => int.Parse(a)).ToList().ForEach(s => allRecords.Add(s));
        allRecords.Add(Score);
        allRecords = allRecords.OrderByDescending(a => a).ToList();
        PlayerPrefs.SetString("TopScores", string.Join("\n", allRecords.GetRange(0, allRecords.Count < 9 ? allRecords.Count : 9).Select(a => a.ToString()).ToList()));

        SpawnSpeed = 3;
        EnemyHealth = 100;
        Damage = 120;
        MaxReloadTime = 1;
        ReloadTime = 1;
        Score = 0;
        ScoreText.text = "0";
    }

    public IEnumerator FreezeSpawnIE()
    {
        EnemiesOnArea.ForEach(GO => GO.GetComponent<NavMeshAgent>().speed = 0);
        EnemiesOnArea.ForEach(GO => GO.GetComponent<Animator>().speed = 0);
        FrozenBounds.SetActive(true);
        StopCoroutine(spawnCoroutine);
        yield return new WaitForSeconds(3);
        EnemiesOnArea.ForEach(GO => GO.GetComponent<NavMeshAgent>().speed = 5);
        EnemiesOnArea.ForEach(GO => GO.GetComponent<Animator>().speed = 1);
        FrozenBounds.SetActive(false);
        spawnCoroutine = StartCoroutine(EnemySpawnerIE());
    }
}