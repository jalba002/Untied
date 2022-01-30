using System;
using System.Collections;
using System.Collections.Generic;
using com.kpg.ggj2022.player;
using UnityEngine;

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

    private IEnumerator playerRespawner;

    public void Awake()
    {
        GM = this;
        player = FindObjectOfType<PlayerController>();
    }

    public void RestartAllElements()
    {
        var all = FindObjectsOfType<RestartableObject>();
        foreach (var item in all)
        {
            try
            {
                item.Restart();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                continue;
            }
        }
    }

    public void TeleportPlayer(Vector2 position)
    {
        player.gameObject.transform.position = position;
    }
    public void TeleportPlayer(Transform t)
    {
        player.gameObject.transform.position = new Vector2(t.position.x, t.position.y);
    }

    public void RespawnPlayer()
    {
        if (playerRespawner != null) return;
        FindObjectOfType<BoundsChanger>().SetDefault();
        if (HUDManager.Instance != null)
            playerRespawner = RespawnPlayer(player.GetStartingPos());
        else
            playerRespawner = RespawnPlayerNoAnim(player.GetStartingPos());

        try
        {
            RestartAllElements();
        }
        catch (Exception e)
        {

        }
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

    private IEnumerator RespawnPlayerNoAnim(Vector3 pos)
    {
        yield return null;
        TeleportPlayer(pos);
        player.ToggleControls(true);
        player.Respawn();
        playerRespawner = null;
    }

    private IEnumerator RespawnPlayer(Vector2 pos)
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

    public GameObject GetPlayerGO()
    {
        return player.gameObject;
    }
}