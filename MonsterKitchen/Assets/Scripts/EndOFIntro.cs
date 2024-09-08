using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndOFIntro : MonoBehaviour
{
    private VideoPlayer vp;
    [SerializeField] private GameObject introVideo;
    [SerializeField] private GameObject imageIntro;

    private void Start()
    {
        vp = GetComponentInChildren<VideoPlayer>();
    }

    private void Update()
    {
        if (vp.isPlaying)
        {
            vp.loopPointReached += CheckEndOfVideo;
        }
    }

    void CheckEndOfVideo(VideoPlayer vp)
    {
        imageIntro.SetActive(true);
        introVideo.SetActive(false);
    }
}
