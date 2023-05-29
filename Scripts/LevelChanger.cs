using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelChanger : MonoBehaviour
{
    [FormerlySerializedAs("_animator")] [SerializeField] private Animator animator;
    [SerializeField] private ScoreManager scoreManager;

    private string _levelToLoad;

    public void FadeToLevel()
    {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(_levelToLoad);
    }

    public void GotoSelection()
    {
        _levelToLoad = "Level Selection";
        FadeToLevel();
    }

    public void GotoNextLevel()
    {
        _levelToLoad = "Level " +(scoreManager.LevelNumber+1).ToString();
        FadeToLevel();
    }

    public void GotoLevel(string level)
    {
        _levelToLoad = level;
        FadeToLevel();
    }
}
