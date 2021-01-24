using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;
    // 생성되는 방들을 배열에 저장
    public List<GameObject> rooms;
    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;

    private void Update() {
        if(waitTime <= 0 && spawnedBoss == false){
            Instantiate(boss, rooms[rooms.Count-1].transform.position, Quaternion.identity);
        } else {
            waitTime -= Time.deltaTime;
        }
    }
}
