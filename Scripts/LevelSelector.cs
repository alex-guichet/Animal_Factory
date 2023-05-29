using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private Image[] stars;
    [SerializeField] private LevelChanger levelChanger;

    private Button _button;

    private void Awake()
    {
        _button = transform.Find("LevelNumber").GetComponent<Button>();
        levelLabel.text = level.ToString();
        CheckInteractivity();
        DisplayStars();
    }

    public void OpenScene()
    {
        levelChanger.GotoLevel("Level " + level);
    }

    public void CheckInteractivity()
    {
        if(level > 1)
        {
            if (PlayerPrefs.HasKey("CountStarsLv" + (level-1).ToString()))
            {
                _button.interactable = true;
            }
        }
    }

    public void DisplayStars()
    {
        string keyName = "CountStarsLv" + level.ToString();

        Color color = Color.white;
        color.a = 1f;

        if (PlayerPrefs.HasKey(keyName))
        {
            switch(PlayerPrefs.GetInt(keyName))
            {
                case 1:
                    stars[0].color = color;
                    break;
                case 2:
                    stars[0].color = color;
                    stars[1].color = color;
                    break;
                case 3:
                    stars[0].color = color;
                    stars[1].color = color;
                    stars[2].color = color;
                    break;
            }
        }
    }
}
