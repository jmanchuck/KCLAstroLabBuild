using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public void Disable()
    {
        GetComponent<Button>().interactable = false;
    }

    public void Enable()
    {
        GetComponent<Button>().interactable = true;
    }

    public void Play()
    {
        GameManager.GetInstance().Unpause();
        Disable();
    }
}
