using UnityEngine;
using UnityEngine.UI;

public class PlayPauseButton : MonoBehaviour
{
    public Sprite playSprite;
    public Sprite pauseSprite;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void UpdateImage()
    {
        GameManager.GetInstance().TogglePause();
        UpdateToPauseState();
    }

    public void UpdateToPauseState()
    {
        if (GameManager.paused)
        {
            button.image.sprite = playSprite;
        }

        else
        {
            button.image.sprite = pauseSprite;
        }
    }
}
