using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class ShareStateTypes {

	#region Public Methods
	/// <summary>
	/// Tells if the current networking library can serialize a Type of data. This checks any
	/// builtin types, as well as any custom types that may be registered. Will need to be periodically
	/// reviewed/updated as the networking libraries improve.
	/// </summary>
	/// <param name="aType">Reflected Type of the Variable we want to know about it.</param>
	/// <returns>Can the network serialize/deserialize it?</returns>
	public static bool CanSerialize(Type aType)
	{
		// Builtin Photon types (as of Photon Classic, 11/16/2018)
		if (aType == typeof(byte) || 
			aType == typeof(string) || 
			aType == typeof(bool) || 
			aType == typeof(short) || 
			aType == typeof(int) || 
			aType == typeof(long) || 
			aType == typeof(float) || 
			aType == typeof(double) || 
			aType == typeof(ExitGames.Client.Photon.Hashtable) || 
			aType == typeof(byte[]) || 
			aType == typeof(Vector2) || 
			aType == typeof(Vector3) || 
			aType == typeof(Quaternion) || 
			aType == typeof(PhotonPlayer))
		{
			return true;
		} 
		else if ( // Our custom types here
			aType == typeof(Color) || 
			aType == typeof(Vector4))
		{
			return true;
		}
		return false;
	}
	/// <summary>
	/// Registers custom serialization types with the active networking library. Should be called
	/// before data starts flying around the network!
	/// </summary>
	public static void RegisterCustomTypes()
	{
		PhotonPeer.RegisterType(typeof(Vector4), (byte)'X', SerializeVector4, DeserializeVector4);
		PhotonPeer.RegisterType(typeof(Color),   (byte)'C', SerializeColor,   DeserializeColor);
	}
	#endregion

	#region Serialization Callbacks
	private static readonly byte[] memVector4 = new byte[4 * 4];
    private static short  SerializeVector4  (StreamBuffer outStream, object customobject)
    {
        Vector4 vo = (Vector4)customobject;
        int     index = 0;
        lock (memVector4)
        {
            byte[] bytes = memVector4;
            Protocol.Serialize(vo.x, bytes, ref index);
            Protocol.Serialize(vo.y, bytes, ref index);
            Protocol.Serialize(vo.z, bytes, ref index);
			Protocol.Serialize(vo.w, bytes, ref index);
            outStream.Write(bytes, 0, 4 * 4);
        }
        return 4 * 4;
    }
    private static object DeserializeVector4(StreamBuffer inStream, short length)
    {
        Vector4 result = new Vector4();
        lock (memVector4)
        {
            inStream.Read(memVector4, 0, 4 * 4);
            int index = 0;
            Protocol.Deserialize(out result.x, memVector4, ref index);
            Protocol.Deserialize(out result.y, memVector4, ref index);
            Protocol.Deserialize(out result.z, memVector4, ref index);
			Protocol.Deserialize(out result.w, memVector4, ref index);
        }
        return result;
    }

	private static readonly byte[] memColor = new byte[4 * 4];
    private static short  SerializeColor  (StreamBuffer outStream, object customobject)
    {
        Color vo = (Color)customobject;
        int     index = 0;
        lock (memColor)
        {
            byte[] bytes = memVector4;
            Protocol.Serialize(vo.r, bytes, ref index);
            Protocol.Serialize(vo.g, bytes, ref index);
            Protocol.Serialize(vo.b, bytes, ref index);
			Protocol.Serialize(vo.a, bytes, ref index);
            outStream.Write(bytes, 0, 4 * 4);
        }
        return 4 * 4;
    }
    private static object DeserializeColor(StreamBuffer inStream, short length)
    {
        Color result = new Color();
        lock (memColor)
        {
            inStream.Read(memColor, 0, 4 * 4);
            int index = 0;
            Protocol.Deserialize(out result.r, memColor, ref index);
            Protocol.Deserialize(out result.g, memColor, ref index);
            Protocol.Deserialize(out result.b, memColor, ref index);
			Protocol.Deserialize(out result.a, memColor, ref index);
        }
        return result;
    }
	#endregion
}
