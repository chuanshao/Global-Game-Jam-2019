using UnityEngine;
using System.Collections;

public class MomAI : MonoBehaviour {

	public Transform[] Rooms;
	public GameObject[] Doors;
	private GameObject Door;

	public int Work_Time;
	public int Room_ID;
	public int Room;

	public float Y_Axis;
	public float Speed;

	public bool Go_To_Door = false;
	public bool Go_To_Room = false;

	void Start()
	{
		int Random_Room = Random.Range(0, Rooms.Length);
		transform.position = Rooms[Random_Room].position;

		StartCoroutine(Work());
	}

	void Update()
	{

		if(Room_ID == 0|| Room_ID == 1|| Room_ID == 2)
		{
			Doors[0].SetActive(true);
			Doors[1].SetActive(false);
			Doors[2].SetActive(false);
			Door = Doors[0];
		}
		else if(Room_ID == 3 || Room_ID == 4 || Room_ID == 5)
		{
			Doors[0].SetActive(false);
			Doors[1].SetActive(true);
			Doors[2].SetActive(false);
			Door = Doors[1];
		}
		else if (Room_ID == 6 || Room_ID == 7 || Room_ID == 8)
		{
			Doors[0].SetActive(false);
			Doors[1].SetActive(false);
			Doors[2].SetActive(true);
			Door = Doors[2];
		}

		if (Go_To_Door)
		{
			transform.position = Vector2.MoveTowards(transform.position, Door.transform.position, Speed * Time.deltaTime);
		}

		if (Go_To_Room)
		{
			transform.position = Vector2.MoveTowards(transform.position, Rooms[Room].transform.position, Speed * Time.deltaTime);
		}
	}

	void New_Room()
	{
		int Random_Room = Random.Range(0, Rooms.Length);
		Room = Random_Room;
		
		if(Rooms[Random_Room].GetComponent<RoomsInfo>().Position.y == transform.position.y)
		{
			Go_To_Room = true;
		}

		if (Rooms[Random_Room].GetComponent<RoomsInfo>().Position.y < transform.position.y)
		{
			Go_To_Door = true;
		}
		else if(Rooms[Random_Room].GetComponent<RoomsInfo>().Position.y > transform.position.y)
		{
			Go_To_Door = true;
		}
	}

	public void Get_Room_ID(int Random)
	{
		Room_ID = Rooms[Random].GetComponent<RoomsInfo>().Room_ID;

		Y_Axis = Rooms[Random].GetComponent<RoomsInfo>().Position.y;
	}

	IEnumerator Work()
	{
		Work_Time = Random.Range(0, 5);
		yield return new WaitForSeconds(Work_Time);
		New_Room();
	}
}
