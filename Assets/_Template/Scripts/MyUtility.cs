using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUtility : MonoBehaviour {

    public static GameObject FindLocalPlayer()
    {
        foreach (var player in PhotonNetwork.playerList)
        {
            if (player.IsLocal)
            {
                return findPlayerGameObject(player.ID);
            }
        }
        return null;
    }

    private static GameObject findPlayerGameObject(int actorID)
    {
        var playerVisuals = FindObjectsOfType<PlayerVisual>();
        foreach (var playerVisual in playerVisuals)
        {
            var playerView = playerVisual.gameObject.GetComponent<PhotonView>();
            if (playerView.ownerId == actorID)
            {
                return playerVisual.gameObject;
            }
        }
        return null;
    }
}
