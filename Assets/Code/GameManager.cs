﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager i;
    public GameObject hidenpart;
    public GameObject line;
    public Points points;
    public int count;
    public bool win = false;
    public Animator animator;
    public GameObject effect1;
    public GameObject effect2;
    public GameObject tick;

    public float time;
    public TextMeshProUGUI cooldown;
    public bool timer = true;
    public UiPanelDotween ui;
    public Sprite lose;
    public Sprite replay;
    public Button button;
    public SceneFader sceneFader;
    public Level level;
    private void Awake()
    {
        i = this;
        if (line != null)
        {
            Instantiate(line, Vector2.zero, Quaternion.identity);
        }
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        LoadLevel(1);
        points =GameObject.FindObjectOfType<Points>();
        hidenpart = GameObject.Find("HidenPart");
    }
    void Start()
    {
        count = points.countPoints;
        hidenpart.SetActive(false);
        animator = GameObject.Find("Full").GetComponent<Animator>();
        animator.enabled = false;
        tick.SetActive(false);
        button.onClick.AddListener(Replay);

    }
    void LoadLevel(int index)
    {
        GameObject lv = Instantiate(level.listLevel[index], new Vector2(0,0), Quaternion.identity); ;
    }
    private void Update()
    {
        if (win)
        {
            hidenpart.SetActive(true);
            animator.enabled = true;
            tick.SetActive(true);
            for (int i = 0; i < points.transform.childCount; i++)
            {
                GameObject child = points.transform.GetChild(i).gameObject;

                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                    spriteRenderer.enabled = false;
            }
            Invoke(nameof(Effect), 0.5f);
            Invoke(nameof(UiWin), 1f);
            win = false;
            button.onClick.RemoveAllListeners(); 
            button.onClick.AddListener(NextLevel);
        }


        if (!win&& time > 0)
        {
            time -= Time.deltaTime;
            cooldown.text = Mathf.CeilToInt(time).ToString();
            if (time<=0)
            {
                ui.PanelFadeIn();

                GameObject.Find("Board").GetComponent<Image>().sprite=lose;
                GameObject.Find("Next").GetComponent<Image>().sprite = replay;
                Debug.Log("Lose");
                button.onClick.RemoveAllListeners(); 
                button.onClick.AddListener(Replay);
                timer = false;
            }
        }
    }

    void Effect()
    {
        GameObject eff = Instantiate(effect1, Vector2.zero, Quaternion.identity);
        GameObject eff2 = Instantiate(effect2, Vector2.zero, Quaternion.identity);
    }
    void UiWin()
    {
        ui.PanelFadeIn();
    }
    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
    public void Replay()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nextSceneIndex);
    }
    public void BackHome()
    {
        sceneFader.FadeTo("HomeScene");
    }
    public void Hint()
    {
        points.Hint();
    }

}
