using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingpositions;   // 1열 어디로 출현할 것인가
    public GameObject[] rooms;  // 방의 종류

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;
    private void Start() {
        int randStartingPos = Random.Range(0, startingpositions.Length);
        transform.position = startingpositions[randStartingPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        direction = Random.Range(1,6);
    }

    private void Update() {
        if(timeBtwRoom <= 0){
            Move();
            timeBtwRoom = startTimeBtwRoom;
        } else {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void Move(){
        if(direction == 1 || direction == 2){ // 방 생성기가 오른쪽
            Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
            transform.position = newPos;
        } else if(direction == 3 || direction == 4){// 방 생성기가 왼쪽
            Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
            transform.position = newPos;
        } else{// 방 생성기가 아래
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
            transform.position = newPos;
        }
        Instantiate(rooms[0], transform.position, Quaternion.identity);
        direction = Random.Range(1,6);
    }
}
