using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PrincipalMenu : MonoBehaviour
{

    [SerializeField] private GameObject image;
    [SerializeField] private GameObject buttonStart;
    [SerializeField] private GameObject buttonEnd;
    [SerializeField] private GameObject videoStart;
    [SerializeField] private GameObject videoEnd;

    public void OnClickStart()
    {
        //buttonStart.SetActive(false);
        //buttonEnd.SetActive(false);
        //videoStart.SetActive(true);
        //VideoPlayer vp = videoStart.GetComponent<VideoPlayer>();
        //image.SetActive(false);
        //vp.loopPointReached += CheckStart;
        SceneManager.LoadScene("Level");
    }

    public void OnClickExit()
    {
        //buttonStart.SetActive(false);
        //buttonEnd.SetActive(false);
        //videoEnd.SetActive(true);
        //VideoPlayer vp = videoEnd.GetComponent<VideoPlayer>();
        //image.SetActive(false);
        //vp.loopPointReached += CheckEnd;
        Application.Quit();
    }

    void CheckEnd(VideoPlayer vp)
    {
        Application.Quit();
    }

    void CheckStart(VideoPlayer vp)
    {
        SceneManager.LoadScene("Level");
    }

}
