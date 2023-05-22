using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;

    public int coin;
    public int stage;

    public float gameTime;
    void Awake()
    {
        Instance= this;
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
