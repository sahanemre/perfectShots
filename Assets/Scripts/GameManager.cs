using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Canvas;

    private AudioSource audioSource;
    public AudioClip basketBallBounce;
    public AudioClip basketSound, winSound,ballThrow,coinSound;

    private BallController ballController;
    private GameObject hoop;
    public ParticleSystem particleSystem;

    public float power;

    private int levelNo;
    private GameObject currentLevelObj;
    private int levelIndexNo = 1;

    public bool DestroyFingerAnim = false;

    public TextMeshProUGUI levelTimeText;
    public float levelTime;
    public bool isTimeStart;
    public bool isLevelFinished;

    public RectTransform nextButton;
    public Ease ease;
    public RectTransform restartButton;

    public LineRenderer lr;
    private float powerBarForce;
    public TextMeshProUGUI powerBarText;

    public TextMeshProUGUI coinText;
    private int totalCoinScore;

    //ScoreBoard
    public TextMeshProUGUI You,Player1, Player2, Player3, Player4,name1,name2,name3,name4;
    public RectTransform th1,th2,th3,th4,th5;
    private float YourTime,Player1Time, Player2Time, Player3Time, Player4Time;
    public RectTransform ScoreBoard;
    public GameObject ScoreBoardGO;

    private List<string> nameList;
    
    void Awake()
    {
        Instance = this;
        levelIndexNo = PlayerPrefs.GetInt("levelIndexNo", levelIndexNo);
        LoadLevel(/*levelIndexNo*/);
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        hoop = GameObject.Find("NextLevelCollider");

    }

    private void Start()
    {
        totalCoinScore = PlayerPrefs.GetInt("totalScore", (int)totalCoinScore);
        coinText.text = totalCoinScore.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(ballController.force);
        //power = ballController.force;

        if (Input.GetMouseButtonDown(0))
        {
            DestroyFingerAnim = true;
            GameObject finger = GameObject.Find("FingerAnim");
            Destroy(finger);

            ScoreBoardGO.SetActive(false);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DestroyFingerAnim = false;
            
        }

        if (isTimeStart)
        {
            levelTime += Time.deltaTime;
            levelTimeText.text = levelTime.ToString("F2");
        }
        else //NextLevel
        {
            levelTimeText.text = levelTime.ToString("F2");
            YourTime = levelTime;
            You.text = YourTime.ToString("F2");

            if (isLevelFinished)
            {
                StartCoroutine(ScoreBoardCoroutine());
                isLevelFinished = false;
            }
        }

        powerBarForce = ballController.GetComponent<BallController>().force;
        lr.SetPosition(1, new Vector3(0, powerBarForce * 1.435f, 0));
        powerBarText.text = Mathf.Round(10 * powerBarForce).ToString();

        //Debug.Log(powerBarForce);
        
        
    }

    public void lrBool(bool isActive)
    {
        if (isActive)
        {
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }
    }

    public void restartButtonAnim(bool isActive)
    {
        if (isActive)
        {
            restartButton
                .DOAnchorPos(new Vector2(-66, -344), 1)
                .SetEase(ease);
        }
        else
        {
            restartButton
                .DOAnchorPos(new Vector2(66, -344), 1)
                .SetEase(ease);
        }
    }

    public void updateCoinScore(int updateCoinAmount)
    {
        totalCoinScore += updateCoinAmount;
        coinText.text = totalCoinScore.ToString();

        PlayerPrefs.SetInt("totalScore", (int)totalCoinScore);

    }

    #region SoundandParticleManager
    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    public void playBasketBallBounceClip()
    {
        PlaySFX(basketBallBounce);
    }

    public void playBasketClip()
    {
        PlaySFX(basketSound);
    }

    public void playWinClip()
    {
        PlaySFX(winSound);
        particleSystem.Play();
        Handheld.Vibrate();
    }

    public void playThrowClip()
    {
        PlaySFX(ballThrow);
    }

    public void confetiStop()
    {
        particleSystem.Stop();
    }

    public void coinClip()
    {
        PlaySFX(coinSound);
    }



    #endregion

    #region CameraManager

    //public void CameraAngle(bool Main,bool Action)
    //{
    //    MainCam.SetActive(Main);
    //}

    //IEnumerator introCoroutine()
    //{


    //    yield return new WaitForSeconds(2.6f);
    //    HowToPlayImage(true);
    //    HowToPlayAnimation();

    //}

    #endregion

    #region ScoreBoardManager

    private void AddNameInList()
    {
        nameList = new List<string>()
        {
            "Piggy",
            "Dapper",
            "Sugar",
            "Mitzi",
            "Bud",
            "Dream",
            "Gator",
            "Dynamite",
            "Thunder",
            "Flame",
            "Blush",
            "Gonzo",
            "Mac",
            "Snowflak",
            "Storm",
            "Hammer",
            "Gentle",
            "Genius",
            "Black",
            "Rain"
        };

        int a = Random.Range(0, 4);
        int b = Random.Range(5, 9);
        int c = Random.Range(10, 14);
        int d = Random.Range(15, 19);

        name1.text = nameList[a].ToString();
        name2.text = nameList[b].ToString();
        name3.text = nameList[c].ToString();
        name4.text = nameList[d].ToString();


    }
    private void PlayersCalculateTime()
    {
        
        Player1Time = Random.Range(20f, 30f);
        Player2Time = Random.Range(30f, 50f);
        Player3Time = Random.Range(50f, 80f);
        Player4Time = Random.Range(80f, 110f);

        //AddListOfTime(YourTime);
        //AddListOfTime(Player1Time);
        //AddListOfTime(Player2Time);
        //AddListOfTime(Player3Time);
        //AddListOfTime(Player4Time);

        PlayersTimeText();
    }



    private void PlayersTimeText()
    {
        Player1.text = Player1Time.ToString("F2");
        Player2.text = Player2Time.ToString("F2");
        Player3.text = Player3Time.ToString("F2");
        Player4.text = Player4Time.ToString("F2");
    }

    private void ScoreBoardPosition()
    {
        

         if (YourTime < Player1Time)
            {
                th1
                .DOAnchorPos(new Vector3(0, 130, 0), 1)
                .SetEase(ease);
                th2
                .DOAnchorPos(new Vector3(0, 50, 0), 1);

                th3
                .DOAnchorPos(new Vector3(0, -30, 0), 1);
                th4
                .DOAnchorPos(new Vector3(0, -110, 0), 1);
                th5
                .DOAnchorPos(new Vector3(0, -190, 0), 1);

        }
            else if (YourTime > Player1Time && YourTime < Player2Time)
            {
                th1
                .DOAnchorPos(new Vector3(0, 50, 0), 1)
                .SetEase(ease);

                th2
                .DOAnchorPos(new Vector3(0, 130, 0), 1);
                th3
                .DOAnchorPos(new Vector3(0, -30, 0), 1);
        }
            else if (YourTime > Player2Time && YourTime < Player3Time)
            {
                th1
                .DOAnchorPos(new Vector3(0, -30, 0), 1)
                .SetEase(ease);

                th2
                .DOAnchorPos(new Vector3(0, 130, 0), 1);

                th3
                .DOAnchorPos(new Vector3(0, 50, 0), 1);
                th4
                .DOAnchorPos(new Vector3(0, -110, 0), 1);

        }
            else if (YourTime > Player3Time && YourTime < Player4Time)
            {
                th1
                .DOAnchorPos(new Vector3(0, -110, 0), 1)
                .SetEase(ease);

                th2
                .DOAnchorPos(new Vector3(0, 130, 0), 1);

                th3
                .DOAnchorPos(new Vector3(0, 50, 0), 1);
                th4
                .DOAnchorPos(new Vector3(0, -30, 0), 1);
                th5
                .DOAnchorPos(new Vector3(0, -190, 0), 1);

        }
            else if (YourTime > Player4Time)
            {
                th1
                .DOAnchorPos(new Vector3(0, -190, 0), 1)
                .SetEase(ease);

                th2
                .DOAnchorPos(new Vector3(0, 130, 0), 1);

                th3
                .DOAnchorPos(new Vector3(0, 50, 0), 1);
                th4
                .DOAnchorPos(new Vector3(0, -30, 0), 1);
                th5
                .DOAnchorPos(new Vector3(0, -110, 0), 1);

            }
        //else
        //{

        //    th1
        //       .DOAnchorPos(new Vector3(0, -190, 0), 1)
        //       .SetEase(ease);

        //    th2
        //    .DOAnchorPos(new Vector3(0, 130, 0), 1);

        //    th3
        //    .DOAnchorPos(new Vector3(0, 50, 0), 1);
        //    th4
        //    .DOAnchorPos(new Vector3(0, -30, 0), 1);
        //    th4
        //    .DOAnchorPos(new Vector3(0, -110, 0), 1);
        //}
        
    }

    //public void ScoreBoardAnimation()
    //{
    //    ScoreBoard
    //        .DOAnchorPos(new Vector3(-300, 164, 0), 1f)
    //        .SetEase(ease);
            
    //}
    //private void ScoreBoardAnimationReverse()
    //{
    //    ScoreBoard
    //        .DOAnchorPos(new Vector3(296, 164, 0), 0.5f)
    //        .SetEase(ease);

    //}


    #endregion

    public void LoadLevel(/*int levelIndex*/)
    {
        AddNameInList();

        if (currentLevelObj != null)
        {
            Destroy(currentLevelObj);
        }
        currentLevelObj = Instantiate(Resources.Load("Level " + levelIndexNo)) as GameObject;

        PlayerPrefs.SetInt("levelIndexNo", (int)levelIndexNo);
        levelIndexNo++;

        ballController = currentLevelObj.GetComponentInChildren<BallController>();

        levelTime = 0;

        lr.SetPosition(0, new Vector3(0, 0, 0));
        powerBarText.text = powerBarForce.ToString("F1");

        PlayersCalculateTime();

    }
     
    public void GoNextLevel()
    {
        nextButtonAnimReverse();

        LoadLevel(/*levelIndexNo++*/);

    }

    public void GoRestartLevel()
    {
        //restartButtonAnim(false);
        ballController.AfterRestartCollider();

    }

    public void nextButtonAnim()
    {
        nextButton
            .DOAnchorPos(new Vector2(0, -200), 1)
            .SetEase(ease);
            

    }

    private void nextButtonAnimReverse()
    {
        //ScoreBoardGO.SetActive(false);
        confetiStop();

        nextButton
            .DOAnchorPos(new Vector2(0, -637), 0.5f)
            .SetEase(ease);
            //.OnComplete(() => ScoreBoardGO.SetActive(false));
    }

  

    IEnumerator ScoreBoardCoroutine()
    {
        ScoreBoardGO.SetActive(true);

        yield return new WaitForSeconds(1f);

        ScoreBoardPosition();

        //yield return new WaitForSeconds(3f);

        //ScoreBoardGO.SetActive(false);
    }

    IEnumerator seco(float sec)
    {
        yield return new WaitForSeconds(sec);
    }

}
