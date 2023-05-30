using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public int coin;
    public int stage;

    public float gameTime;
    void Awake()
    {
       
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        AudioManager.instance.BGMSTART();
    }
    private void Update()
    {
        gameTime += Time.deltaTime;
    }
    public void NextStage()
    {
        stage++;
        if(stage == 2) 
            stage= 0;
        SceneManager.LoadScene(stage);

    }
}
