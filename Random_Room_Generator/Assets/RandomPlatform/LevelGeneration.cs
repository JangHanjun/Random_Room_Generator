using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingpositions;   // 1열 어디로 출현할 것인가
    public GameObject[] rooms;  // 방의 종류   ( 0 : LR / 1 : LRB / 2 : LRT / 3 : LRBT )

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    // 경계선
    public float minX;
    public float maxX;
    public float minY;
    public bool stopGeneration;    // 경계선 체크 후 범위 벗어나면 더이상 생성 x
    public LayerMask room; //이전 방을 탐색하기 위한 변수
    //
    private int downCounter;

    private void Start() {
        int randStartingPos = Random.Range(0, startingpositions.Length);
        transform.position = startingpositions[randStartingPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        direction = Random.Range(1,6);
    }

    private void Update() {
        if(timeBtwRoom <= 0 && stopGeneration == false){
            Move();
            timeBtwRoom = startTimeBtwRoom;
        } else {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void Move(){
        if(direction == 1 || direction == 2){   // 방 생성기가 오른쪽
            downCounter = 0;
            if(transform.position.x < maxX){    // 범위를 초과했다면
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                // 방 생성
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                //다음 움직임에서 왼쪽이 없게
                direction = Random.Range(1,6);
                if(direction == 3){
                    direction = 2;
                } else if(direction == 4){
                    direction = 5;
                }
            } else {    // 끝에 도달했으니 아래로 내린다
                direction = 5;
            }
        }else if(direction == 3 || direction == 4){// 방 생성기가 왼쪽
            if(transform.position.x > minX){        // 범위를 초과했다면
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                // 방 생성
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                //다음 움직임에서 오른쪽이 없게
                direction = Random.Range(3,6);
                
            } else {    // 끝에 도달했으니 아래로 내린다
                direction = 5;
            }
            
        } else if(direction == 5){// 방 생성기가 아래
            downCounter++;

            if(transform.position.y > minY){
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                if(roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3){
                    
                    if(downCounter >= 2){
                        roomDetection.GetComponent<RoomType>().RoomDestruction(); // 삭제
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    } else{
                        // 바닥이 없다는 뜻
                        roomDetection.GetComponent<RoomType>().RoomDestruction(); // 삭제

                        int randBottomRoom = Random.Range(1,4);
                        if(randBottomRoom == 2){
                            randBottomRoom = 1;
                        }
                        Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                // 방이 생성되기 위해서는 이전 방이 B포함, 생성되는 방이 T포함이어야 한다
                int rand = Random.Range(2,4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1,6);
            } else{
                // 아래로 꽉 찼으니 이제 그만
                stopGeneration = true;
            }
            
        }
    }
}
