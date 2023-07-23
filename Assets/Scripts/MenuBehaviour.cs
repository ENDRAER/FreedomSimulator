using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
    [SerializeField] private PlayMode playMode;
    [SerializeField] private GameObject NoSignalGO;
    [SerializeField] private GameObject PlayButtonLocker;
    [SerializeField] private GameObject TopScoreGO;
    [SerializeField] private TextMeshProUGUI TopScoreText;
    [SerializeField] private GameObject CreditsGO;
    [SerializeField] private Animator CreditsAnimator;


    public void PlayButton()
    {
        PlayButtonLocker.transform.localPosition = new Vector3(0, 0.1f, 0);
        PlayButtonLocker.transform.localEulerAngles = new Vector3();
        NoSignalGO.SetActive(false);
        CreditsGO.SetActive(false);
        TopScoreGO.SetActive(false);

        playMode.StartGame();
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void RecordsButton()
    {
        TopScoreGO.SetActive(!TopScoreGO.activeSelf);
        CreditsGO.SetActive(false);
        TopScoreText.text = PlayerPrefs.GetString("TopScores");
    }

    public void CreditsButton()
    {
        CreditsGO.SetActive(!CreditsGO.activeSelf);
        TopScoreGO.SetActive(false);
        CreditsAnimator.Rebind();
    }

    public void ButtonPressing(Transform trans)
    {
        trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y == 0? -0.06f : 0, trans.localPosition.z);
    }

    public void ButtonLocker(Transform trans)
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
}