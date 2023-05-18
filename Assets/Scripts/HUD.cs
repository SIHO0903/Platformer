using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    TextMeshProUGUI text;
    enum HudType { Coin, Stage, Time}
    [SerializeField] HudType type;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        float gameTime = GameManager.Instance.gameTime;
        switch (type)
        {
            case HudType.Coin:
                text.text = string.Format($"X {GameManager.Instance.coin}");
                break;
            case HudType.Stage:
                text.text = string.Format($"STAGE {GameManager.Instance.stage+1}");
                break; 
            case HudType.Time:
                text.text = string.Format($"TIME {Mathf.FloorToInt(gameTime / 60)}:{Mathf.FloorToInt(gameTime % 60)}");
                break;
        }
    }
}
