using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject PlayerPrefab;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void SpawPlayer(int id, string userName, Vector3 position, Quaternion rotation)
    {
        GameObject player;
        if (id == Client.instance.myId)
        {
            player = Instantiate(localPlayerPrefab, position, rotation);
        }
        else 
        {
            player = Instantiate(PlayerPrefab, position, rotation);

        }

        player.GetComponent<PlayerManager>().id = id;
        player.GetComponent<PlayerManager>().userName = userName;
        players.Add(id, player.GetComponent<PlayerManager>());
    }
}
