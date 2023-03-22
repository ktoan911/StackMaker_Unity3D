using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int sceneIndex;
    public virtual void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public virtual void NextScene()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex ;
        SceneManager.LoadScene(++sceneIndex, LoadSceneMode.Single);
    }

    public virtual void SelectScene()
    {
        UIManager.instance.endGame.SetActive(false);
        UIManager.instance.selectScene.SetActive(true);
    }
    public virtual void ChangeScene01()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public virtual void ChangeScene02()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}