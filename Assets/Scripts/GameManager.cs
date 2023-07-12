using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject playerPrefab;
    public GameObject menuPanel;
    public GameObject playPanel;
    public GameObject levelCompletedPanel;
    public GameObject gameOverPanel;
    public Text scoreText;
    public Text ballsText;
    public Text levelText;
    public Text hihgScoreText;
   
    public GameObject[] levels;
    public static GameManager Instance { get; private set; }


    private int score;

    public int Score
    {
        get { return score; }
        set { score = value; 
        scoreText.text = "Score: " + score;
        }
       
    }

    private int level;

    public int Level 
    {
        get { return level; }
        set { level = value;
            levelText.text = "Level: " + level;
        }
    }

    private int balls;

    public int Balls
    {
        get { return balls; }
        set { balls = value;
            ballsText.text = "Bals: " + balls;
        }
    }


    public enum State { MENU,INIT,PLAY,LEVELCOMPLETED,LOADLEVEL,GAMEOVER }
    State state;

    GameObject currentBall;
    GameObject currentLevel;
    bool isSwitchingState;

    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }

    public void SwitchState(State newState, float delay = 0)
    {
      
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        state = newState;
        BeginState(newState);
        isSwitchingState = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                hihgScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highScore");
                menuPanel.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible = false;
                playPanel.SetActive(true);
                Score = 0;
                Level = 0;
                Balls = 3;
                if (currentLevel != null)
                {
                    Destroy(currentLevel);
                }
                Instantiate(playerPrefab);
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(currentBall);
                Destroy(currentLevel);
                Level++;
                levelCompletedPanel.SetActive(true);
                SwitchState(State.LOADLEVEL, 2f);
                break;
            case State.LOADLEVEL:
                if(Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else
                {
                    currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if(Score > PlayerPrefs.GetInt("highScore"))
                {
                    PlayerPrefs.SetInt("highScore", Score);
                }
                gameOverPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if(currentBall == null)
                {
                    if (Balls > 0)
                    {
                        currentBall = Instantiate(ballPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }
                if (currentLevel != null && currentLevel.transform.childCount == 0 && !isSwitchingState)
                {
                     SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;
            default:
                break;
        }
    }

    void EndState()
    {
        switch (state)
        {
            case State.MENU:
                menuPanel.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                levelCompletedPanel.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                playPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                break;
            default:
                break;
        }
    }


}
