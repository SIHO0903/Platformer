using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] Sprite img_Health;
    [SerializeField] Sprite img_Health_off;
    enum HudType { Coin, Stage, Time}
    [SerializeField] HudType type;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        float gameTime = GameManager.instance.gameTime;
        switch (type)
        {
            case HudType.Coin:
                text.text = string.Format($"X {GameManager.instance.coin}");
                break;
            case HudType.Stage:
                text.text = string.Format($"STAGE {GameManager.instance.stage+1}");
                break; 
            case HudType.Time:
                text.text = string.Format($"TIME {Mathf.FloorToInt(gameTime / 60)}:{Mathf.FloorToInt(gameTime % 60)}");
                break;
        }
    }

}
