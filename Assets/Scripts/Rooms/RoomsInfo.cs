using UnityEngine;

public class RoomsInfo : MonoBehaviour {

	public static RoomsInfo instance;

	public int Room_ID;

	public Vector3 Position;

    void Awake()
	{
		instance = this;
	}
}
