using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public RectTransform WinUI;
    public static GameManager instance;
    int numberOfCrowns;
    private void Awake()
    {

        instance = this;
        WinUI.DOSizeDelta(Vector2.zero, 0);
        WinUI.DOSizeDelta(new Vector2((Screen.height * 2) + 100, (Screen.width * 2) + 100), 1).SetEase(Ease.InBack).OnComplete(() => WinUI.gameObject.SetActive(false));
    }
    public void AddCrown()
    {
        numberOfCrowns++;
    }
    public void RemoveCrown()
    {
        numberOfCrowns--;
        if (numberOfCrowns <= 0)
        {
            WinState();
        }
    }

    void WinState()
    {
        WinUI.gameObject.SetActive(true);
        WinUI.DOSizeDelta(Vector2.zero, 1).SetEase(Ease.OutFlash).OnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
}
