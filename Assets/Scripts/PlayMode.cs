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
    [NonSerialized] public float MaxReloadTime = 1;
    [NonSerialized] public float ReloadTime = 1;
    [NonSerialized] public int Score;
    [NonSerialized] private bool gamePlay;
    [Header("Interface")]
    [SerializeField] public TextMeshProUGUI ScoreText;
    [SerializeField] public Image ReloadTimeProgressbar;
    [SerializeField] public GameObject FrozenBounds;
    [SerializeField] public GameObject EnemiesCounter;
    [SerializeField] public GameObject NoSignalGO;
    [SerializeField] public GameObject PlayButtonLocker;
    [SerializeField] public GameObject TopScoreGO;
    [SerializeField] public TextMeshProUGUI TopScoreText;
    [SerializeField] public GameObject CreditsGO;
    [SerializeField] public Animator CreditsAnimator;


    private void Awake()
    {
        MainBridge = this;
    }
    
    private void Update()
    {
        if (MaxReloadTime <= ReloadTime)
        {
            ReloadTimeProgressbar.color = Color.green;
            if (Physics.Raycast(planeCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && Input.GetKeyDown(KeyCode.Mouse0) && gamePlay)
            {
                ReloadTime = 0;
                ReloadTimeProgressbar.color = Color.red;
                Instantiate(rocketPF, rocketSpawner.position, Quaternion.LookRotation(hit.point - rocketSpawner.position, rocketSpawner.right))
                    .GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 200), ForceMode.Impulse);
            }
        }
        else if (MaxReloadTime > ReloadTime)
        {
            ReloadTimeProgressbar.transform.localPosition = new Vector3(-0.899f, -1.95f, -1.7f + Mathf.Clamp(1.7f / MaxReloadTime * ReloadTime, 0, 1.7f));
            ReloadTime += Time.deltaTime;
        }
    }

    IEnumerator EnemySpawnerIE()
    {
        yield return new WaitForSeconds(1);
        if(UnityEngine.Random.Range(0, 100) < 95 || CreatedDonkey != null)
            EnemiesOnArea.Add(Instantiate(arabicPF, HomesGO[UnityEngine.Random.Range(0, HomesGO.Length - 1)].transform.position, new Quaternion()));
        else
            CreatedDonkey = Instantiate(donkeysPF[UnityEngine.Random.Range(0, donkeysPF.Length)], HomesGO[UnityEngine.Random.Range(0, HomesGO.Length - 1)].transform.position, new Quaternion());

        if (EnemiesOnArea.Count == 10)
            Defeat();
        else
            spawnCoroutine = StartCoroutine(EnemySpawnerIE());
        EnemiesCounter.transform.localEulerAngles = new Vector3(0, -90 + (18 * EnemiesOnArea.Count), 0);
    }

    public void Defeat()
    {
        EnemiesOnArea.ForEach(enemies => Destroy(enemies));
        EnemiesOnArea.Clear();
        Destroy(CreatedDonkey);
        NoSignalGO.SetActive(true);
        gamePlay = false;

        List<int> allRecords = new List<int>();
        PlayerPrefs.GetString("TopScores").Split('\n').Select(a => int.Parse(a)).ToList().ForEach(s => allRecords.Add(s));
        allRecords.Add(Score);
        allRecords.OrderByDescending(a => a).ToList();
        PlayerPrefs.SetString("TopScores", string.Join("\n", allRecords.GetRange(0, allRecords.Count < 9? allRecords.Count : 9).Select(a => a.ToString()).ToList()));
        Score = 0;
        ScoreText.text = "0";
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
        FrozenBounds.SetActive(true);
        yield return new WaitForSeconds(3);
        EnemiesOnArea.ForEach(GO => GO.GetComponent<NavMeshAgent>().speed = 5);
        EnemiesOnArea.ForEach(GO => GO.GetComponent<Animator>().speed = 1);
        spawnCoroutine = StartCoroutine(EnemySpawnerIE());
        FrozenBounds.SetActive(false);
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

    public void PlayButtonDown(Transform trans)
    {
        if (gamePlay)
            return;
        trans.localPosition = new Vector3(trans.localPosition.x, -0.06f, trans.localPosition.z); 
        PlayButtonLocker.transform.localPosition = new Vector3(0, 0.1f, 0);
        PlayButtonLocker.transform.localEulerAngles = new Vector3();
        NoSignalGO.SetActive(false);
        CreditsGO.SetActive(false);
        TopScoreGO.SetActive(false);
        gamePlay = trans;
        spawnCoroutine = StartCoroutine(EnemySpawnerIE());
    }

    public void ExitButtonDown(Transform trans)
    {
        trans.localPosition = new Vector3(trans.localPosition.x, -0.06f, trans.localPosition.z);
        Application.Quit();
    }

    public void RecordsButtonDown(Transform trans)
    {
        if (gamePlay)
            return;
        trans.localPosition = new Vector3(trans.localPosition.x, -0.06f, trans.localPosition.z);
        TopScoreGO.SetActive(!TopScoreGO.activeSelf);
        CreditsGO.SetActive(false);
        TopScoreText.text = PlayerPrefs.GetString("TopScores");
    }

    public void CreditsButtonDown(Transform trans)
    {
        if (gamePlay)
            return;
        trans.localPosition = new Vector3(trans.localPosition.x, -0.06f, trans.localPosition.z);
        CreditsGO.SetActive(!CreditsGO.activeSelf);
        TopScoreGO.SetActive(false);
        CreditsAnimator.Rebind();
    }

    public void ButtonUp(Transform trans)
    {
        trans.localPosition = new Vector3(trans.localPosition.x, 0, trans.localPosition.z);
    }
    public void ButtonLockerDown(Transform trans)
    {
        if (trans.localPosition.x == 0)
        {
            trans.localPosition = new Vector3(-0.2f, 0.12f, 0);
            trans.localEulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            trans.localPosition = new Vector3(0, 0.1f, 0);
            trans.localEulerAngles = new Vector3();
        }
    }
    #endregion
}
