using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon;

public class StageManager : MonoBehaviour
{
    int _stageOwnerActorID = -1;
    public int StageOwnerActorID
    {
        get { return _stageOwnerActorID; }  
        set {
            _stageOwnerActorID = value;
            SetStageOwner(_stageOwnerActorID);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    var player = findPlayerGameObject(PhotonNetwork.playerList[0].ID);
        //    Debug.Log(player.name);
        //    Debug.Log(player.GetComponent<PlayerVisual>().PlayerColor);
        //}
	}


    [SerializeField] MeshRenderer stageRenderer;
    [SerializeField] MeshRenderer modelBottomRenderer;
    void SetStageOwner(int ownerActorID)
    {
        Debug.Log("Settign owner to be " + ownerActorID);
        Color playerColor = Color.black;
        if (ownerActorID != -1)
        {
            var player = PhotonPlayer.Find(ownerActorID);
            playerColor = ((PlayerColorEnum)(findPlayerGameObject(ownerActorID).GetComponent<PlayerVisual>().PlayerColor)).ToColor();
        }

        //Update Visuals
        stageRenderer.material.color = playerColor;
        modelBottomRenderer.material.color = playerColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On Trigger enter");
        //No one owns the stage.
        if (_stageOwnerActorID == -1)
        {
            //has player visual means the GameObject is a player
            if (other.gameObject.GetComponent<PlayerVisual>() != null)
            {
                assignPlayerWithStageAuthority(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("On Trigger exit");
        StageOwnerActorID = -1;
    }

    private void assignPlayerWithStageAuthority(GameObject playerGameObject)
    {
        StageOwnerActorID = playerGameObject.GetComponent<PhotonView>().owner.ID;
    }

    private GameObject findPlayerGameObject(int actorID)
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
