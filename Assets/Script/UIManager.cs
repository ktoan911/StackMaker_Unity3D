using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Text textBrickCount;

    public GameObject gameOver;

    [SerializeField] private GameObject startPlayGame;

    private bool isStart;
    public bool IsStart { get => isStart; }

    public GameObject endGame;

    public GameObject selectScene;

    private void Awake()
    {
        UIManager.instance = this;

        gameOver.SetActive(false);

        startPlayGame.SetActive(true);

        endGame.SetActive(false) ;

        selectScene.SetActive(false);

        isStart = false;
    }

    public void SetBrickCount(int count)
    {
        textBrickCount.text = count.ToString();
    }

    public void StartGame()
    {
        startPlayGame.SetActive(false);
        isStart = true;
    }


}
