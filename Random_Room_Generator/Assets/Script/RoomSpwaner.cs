using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpwaner : MonoBehaviour
{
    public int openingDirection;
    // 1 : need B
    // 2 : need T
    // 3 : need L
    // 4 : need R

    private RoomTemplate templates;
    private int rand;   // 방 생성을 랜덤으로 하기 위해
    public bool spwaned = false;  // 방 생성이 연속되지 않게 하기 위한 불 변수

    public float waitTime = 4f;
    void Start() {
        Destroy(gameObject, waitTime); // 4초쯤 지나면 다 생성되어있으니 포인트들도 메모리를 위해 제거
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplate>();
        Invoke("Spwan", 0.1f);
    }

    void Spwan() {
        Debug.Log("방 생성");
        if(spwaned == false){
            if(openingDirection == 1){
                // 아래쪽 문이 있는 방이 필요함
                rand = Random.Range(0, templates.bottomRooms.Length-1);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            } else if(openingDirection == 2){
                // 위쪽 문이 있는 방이 필요함
                rand = Random.Range(0, templates.topRooms.Length-1);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            } else if(openingDirection == 3){
                // 왼쪽 문이 있는 방이 필요함
                rand = Random.Range(0, templates.leftRooms.Length-1);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            } else if(openingDirection == 4){
                // 오른쪽 문이 있는 방이 필요함
                rand = Random.Range(0, templates.rightRooms.Length-1);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            spwaned = true;
        }
    }

    // 다른 방에 의해서 spwanpoint에 방이 이미 생성되어 있다면 Destroy로 방이 생성되지 않게 한다
    // 콜라이더랑 리지드바디가 다 있기 때문에 가능
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("SpwanPoint")){
            if(other.GetComponent<RoomSpwaner>().spwaned == false && spwaned==false){
                Debug.Log("포인트 파괴");
                // 아직 스폰되지 않은 방 위치에 서로 스폰이 겹친다면
                Instantiate(templates.closedRooms, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spwaned = true;
        }
    }
}
