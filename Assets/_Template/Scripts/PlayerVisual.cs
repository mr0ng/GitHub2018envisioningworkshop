using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
	bool isLocalPlayer = false;

    int _playerColor = 5;
    public int PlayerColor
    {
        get {
            return _playerColor;
        }
        set {
            _playerColor = value;
            if(!isLocalPlayer)
                UpdatePlayerColor(((PlayerColorEnum)_playerColor).ToColor());
        }
    }

	public bool IsLocalPlayer
	{
		get { return isLocalPlayer; }
		set { 
			isLocalPlayer = value;
			if (isLocalPlayer)
				DisableVisuals();
		}
	}
	void Start ()
	{
		// Don't need to draw yourself
		if (isLocalPlayer)
			DisableVisuals();


        //Get a random color.
        //Implement a pool later.
        //if(IsLocalPlayer)
            PlayerColor = (int)(getNonRepeatRandomColor());

	}

    private PlayerColorEnum getNonRepeatRandomColor()
    {
        Color randColor = Random.ColorHSV();
        int count = 0;
        var pickedColorEnum = GetRandomPlayerColor();
        while (count < 20 && otherPlayerUseColorEnum(pickedColorEnum))
        {
            pickedColorEnum = GetRandomPlayerColor();
        }
        return pickedColorEnum;
    }
    private bool otherPlayerUseColorEnum(PlayerColorEnum colorEnum)
    {
        foreach (var player in GetOtherPlayerGameObjectList())
        {
            if (player.GetComponent<PlayerVisual>().PlayerColor == (int)colorEnum)
            {
                return true;
            }
        }
        return false;
    }

	void Update ()
	{
		if (isLocalPlayer)
		{
			Transform t = Camera.main.transform;
			transform.position = t.position;
			transform.rotation = t.rotation;
		}
	}

	void DisableVisuals()
	{
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(false);
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = false;
		}
	}

    void UpdatePlayerColor(Color color)
    {
        MeshRenderer headRenderer = transform.Find("Head").GetComponent<MeshRenderer>();
        headRenderer.material.color = color;
    }




    public static PlayerColorEnum GetRandomPlayerColor()
    {
        var values = System.Enum.GetValues(typeof(PlayerColorEnum));
        System.Random random = new System.Random();
        var randomPlayerColorEnum = (PlayerColorEnum)values.GetValue(random.Next(values.Length));
        return randomPlayerColorEnum;
    }

    public List<GameObject> GetOtherPlayerGameObjectList()
    {
        var players = new List<GameObject>();
        var playerVisuals = FindObjectsOfType<PlayerVisual>();
        foreach (var playerVisual in playerVisuals)
        {
            if(playerVisual != this)
                players.Add(playerVisual.gameObject);
        }
        return players;
    }
}
