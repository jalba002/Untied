using System.Collections;
using System.Collections.Generic;
using com.kpg.ggj2022.player;
using UnityEngine;

// This class is a literal god.
public class GameManager : MonoBehaviour
{
    private static GameManager _gameManager;

    public static GameManager GM
    {
        get => _gameManager;

        set
        {
            if (_gameManager != null)
                Destroy(value.gameObject);
            else
            {
                _gameManager = value;
            }
        }
    }

    public PlayerController player;

    // PauseManager _pauseManager;
    private IEnumerator playerRespawner;

    public void Awake()
    {
        GM = this;
        // transform.parent = null;
        // DontDestroyOnLoad(this.gameObject);
        // gameObject.name = "[LITERAL GOD]";
        // _pauseManager = FindObjectOfType<PauseManager>();
    }

    private void Start()
    {
        // PlayGame();
    }

    void TeleportController(PlayerController pc, Vector3 position)
    {
        //pc.Controller.enabled = false;
        pc.gameObject.transform.position = position;
        //pc.Controller.enabled = true;
    }

    public void TeleportPlayer(Vector3 pos)
    {
        TeleportController(player, pos);
        //m_Camera.GetComponent<FollowCameraController>().ForceNewPos();
    }
    
    public void RespawnPlayer()
    {
        if (playerRespawner != null) return;
        if(HUDManager.Instance != null)
            playerRespawner = RespawnPlayer(player.GetStartingPos());
        else
            playerRespawner = RespawnPlayerNoAnim(player.GetStartingPos());
        
        StartCoroutine(playerRespawner);
    }

    public void RestartGame()
    {
        if (playerRespawner != null) return;
        if (HUDManager.Instance != null)
            playerRespawner = RestartGame(player.GetStartingPos());
        else
            playerRespawner = RestartGameNoAnim(player.GetStartingPos());

        StartCoroutine(playerRespawner);
    }

    private void RepositionAllElements()
    {
        var stuff = FindObjectsOfType<MonoBehaviour>();

    }
    
    private IEnumerator RespawnPlayerNoAnim(Vector3 pos)
    {
        yield return null;
        TeleportPlayer(pos);
        player.ToggleControls(true);
        playerRespawner = null;
    }

    private IEnumerator RespawnPlayer(Vector3 pos)
    {
        // Camera fadeblack

        yield return new WaitForSecondsRealtime(HUDManager.Instance.FadeToBlack());
        TeleportPlayer(pos);
        player.ToggleControls(true);
        // player.GetComponent<PlayerAnimatorManager>().Restart();
        // player.GetComponent<PlayerHealthManager>().Respawn();
        yield return new WaitForSecondsRealtime(HUDManager.Instance.FadeToWhite());
        // Camera fadewhite
        playerRespawner = null;
    }

    private IEnumerator RestartGame(Vector3 pos)
    {
        // Camera fadeblack

        yield return new WaitForSecondsRealtime(HUDManager.Instance.FadeToBlack());
        TeleportPlayer(pos);
        player.ToggleControls(true);
        // player.GetComponent<PlayerAnimatorManager>().Restart();
        // player.GetComponent<PlayerHealthManager>().Respawn();
        yield return new WaitForSecondsRealtime(HUDManager.Instance.FadeToWhite());
        // Camera fadewhite
        playerRespawner = null;
    }

    private IEnumerator RestartGameNoAnim(Vector3 pos)
    {
        // Camera fadeblack

        yield return new WaitForSeconds(1f);
        TeleportPlayer(pos);
        player.ToggleControls(true);
        player.StopMovement();


        // player.GetComponent<PlayerAnimatorManager>().Restart();
        // player.GetComponent<PlayerHealthManager>().Respawn();
        // Camera fadewhite
        playerRespawner = null;
    }


    // public void TeleportPlayerToWaitingRoom(PlayerController pc)
    // {
    //     TeleportController(pc, waitingRoom.position);
    // }

    // public void TogglePlayerControl(bool enable)
    // {
    //     Debug.Log("Player has " + (enable ? "YES" : "NO") + " control.");
    //     foreach (var item in m_Players)
    //     {
    //         item.ToggleInput(enable);
    //     }
    // }

    // [Button("Set camera spot")]
    // public void SetCameraSpot(CameraSpot cameraSpot)
    // {
    //     m_Camera.transform.position = cameraSpot.transform.position;
    //     m_Camera.transform.rotation = cameraSpot.transform.rotation;
    //     m_Camera.fieldOfView = cameraSpot.FOV;
    // }

    //public void Pause()
    //{
    //    // Get the pause menu and toggle it.
    //    // If the pause menu is null ignore it.
    //    if (_pauseManager == null) return;

    //    _pauseManager.PauseOn();

    //}

    public GameObject GetPlayerGO()
    {
        return player.gameObject;
    }

    //  public void UpdateOnSceneLoad()
    //  {
    //      player = FindObjectOfType<PlayerController>();
    //      m_Camera = FindObjectOfType<Camera>();
    //      _pauseManager = FindObjectOfType<PauseManager>();
    //  }
}