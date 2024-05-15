using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject player;
    public void LoadGameScene()
    {
        player.transform.position = new Vector3(-16.49f, 0f, 7.31f);
    }
}
