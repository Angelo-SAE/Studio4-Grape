using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] PlayableDirector playScene;
    [SerializeField] Animator wheelOne;
    [SerializeField] Animator wheelTwo;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            if (playScene != null)
            {
                playScene.Play();
                wheelOne.SetTrigger("StartEngine");
                wheelTwo.SetTrigger("StartEngine");

                playScene.stopped += OnTimelineStopped;
            }
            
            

        }

    }

    private void OnTimelineStopped(PlayableDirector director)
    {

        if (director == playScene)
        {
            SceneManager.LoadScene(1);
        }
    }

    private void OnDestroy()
    {

        if (playScene != null)
        {
            //playScene.stopped -= OnTimelineStopped;
            Application.Quit();
        }
    }

}
