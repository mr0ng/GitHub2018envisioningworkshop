using System;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Sharing/Share State")]
public class ShareState : MonoBehaviour
{
	#region Supporting class Watch
	[Serializable]
	class Watch
	{
		// serializable watch definition
		[SerializeField] Component component = null;
		[SerializeField] string    member    = "";
		[SerializeField] float     frequency = 0;

		// state values
		MemberInfo memberInfo;
		float      timer;
		object     prevValue;
		double     lastNetworkTime;

		private object GetValue()
		{
			// get the value via reflection
			// if it's a field
			FieldInfo field = memberInfo as FieldInfo;
			if (field != null)
				return field.GetValue(component);
			// if it's a property
			PropertyInfo prop = memberInfo as PropertyInfo;
			if (prop != null)
				return prop.GetValue(component);

			return null;
		}
		public  object GetCurrentValue()
		{
			return prevValue;
		}
		public void Initialize()
		{
			// turn serializable data into a MemberInfo we can actually use
			memberInfo = component.GetType().GetMember(member, BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic)[0];
			timer      = Time.time;
			prevValue  = GetValue();
		}
		public bool HasChanged(double aCurrentNetworkTime)
		{
			bool result = false;
			timer = Time.time + frequency;
			
			// Get the current value, and check for change!
			object currValue = GetValue();
			result = prevValue == null ? currValue != null : !prevValue.Equals(currValue);
			if (result)
			{
				lastNetworkTime = aCurrentNetworkTime;
				prevValue       = currValue;
			}

			return result;
		}
		public bool NeedsCheck()
		{
			return timer <= Time.time;
		}
		public void Set(double aDataTime, object aData)
		{
			// if this data change timestamp is older than ours, skip it!
			if (aDataTime < lastNetworkTime)
				return;

			// Set the value via reflection
			FieldInfo field = memberInfo as FieldInfo;
			if (field != null)
				field.SetValue(component, aData);
			PropertyInfo prop = memberInfo as PropertyInfo;
			if (prop != null)
				prop.SetValue(component, aData);

			prevValue = aData;
			timer     = Time.time + frequency;
		}
	}
	#endregion

	#region Fields
	[SerializeField] Watch[] watches     = null;
	bool                     initialSync = false;
	PhotonView               view        = null;
	#endregion

	#region Properties
	private PhotonView View { get {
		// This is delayed so that users can add a PhotonView component during runtime if they want
		if (view == null)
			view = GetComponent<PhotonView>();
		if (view == null)
			Debug.LogErrorFormat(gameObject, "[Sharing] {0} requires a PhotonView for the ShareState component to work!", name);
		return view;
	} }
	private static bool   NetworkReady { get {
		return PhotonNetwork.inRoom;
	} }
	private static double NetworkTime { get { 
		return PhotonNetwork.time;
	}}
	#endregion

	#region Unity Events
	void Awake ()
	{
		for (int i = 0; i < watches.Length; i++)
			watches[i].Initialize();
	}
	void Update ()
	{
		// If we aren't in a network room, network messages will just get lost. Don't do anything. 
		if (!NetworkReady)
			return;

		// Initial sync needs to happen right away, but can't happen until we're in a network room
		if (!initialSync)
		{
			initialSync = true;
			SyncAll();
		}

		// Go through all the watches, and check if they need updated
		for (int i = 0; i < watches.Length; i++)
		{
			Watch watch = watches[i];
			if (watch.NeedsCheck() && 
				watch.HasChanged(NetworkTime))
			{
				DataChange(i, watch.GetCurrentValue());
			}
		}
	}
	#endregion
	
	#region RPCs
	void DataChange(int aWatch, object aData)
	{
		View.RPC("RPCDataChange", PhotonTargets.Others, aWatch, aData);
	}
	[PunRPC]
	void RPCDataChange(int aWatch, object aData, PhotonMessageInfo info)
	{
		watches[aWatch].Set(info.timestamp, aData);
	}

	void SyncAll()
	{
		View.RPC("RPCSyncAll", PhotonTargets.MasterClient);
	}
	[PunRPC]
	void RPCSyncAll()
	{
		for (int i = 0; i < watches.Length; i++)
			DataChange(i, watches[i].GetCurrentValue());
	}
	#endregion
}