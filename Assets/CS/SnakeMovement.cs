using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float moveSpeed = 1f; // 스네이크의 움직임 속도

    private Vector2Int gridMoveDirection; // 스네이크의 이동 방향
    private Vector2Int gridPosition; // 스네이크의 현재 위치
    private float gridMoveTimer; // 스네이크 움직임 타이머 (움직일 때마다 초기화)

    void Start()
    {
        gridPosition = new Vector2Int(0, 0); // 스네이크의 초기 위치 설정
        gridMoveDirection = new Vector2Int(1, 0); // 스네이크의 초기 이동 방향 설정 (오른쪽)
        gridMoveTimer = 0; // 움직임 타이머 초기화
    }

    void Update()
    {
        HandleInput(); // 사용자 입력 처리

        gridMoveTimer += Time.deltaTime * moveSpeed; // 움직임 타이머 업데이트
        if (gridMoveTimer >= 1) // 움직임 타이머가 1 이상이면
        {
            gridMoveTimer = 0; // 움직임 타이머 초기화
            Move(); // 스네이크 움직임 처리
        }
    }

    /// <summary>
    /// 플레이어 조작
    /// </summary>
    private void HandleInput()
    {
        // 방향키 입력에 따른 스네이크 이동 방향 변경
        // 이전 방향과 반대 방향으로 변경하지 않도록 함
        if (Input.GetKeyDown(KeyCode.UpArrow) && gridMoveDirection.y != -1)
        {
            gridMoveDirection = new Vector2Int(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && gridMoveDirection.y != 1)
        {
            gridMoveDirection = new Vector2Int(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && gridMoveDirection.x != 1)
        {
            gridMoveDirection = new Vector2Int(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && gridMoveDirection.x != -1)
        {
            gridMoveDirection = new Vector2Int(1, 0);
        }
    }

    private void Move()
    {
        gridPosition += gridMoveDirection; // 스네이크 위치 업데이트
        transform.position = new Vector3(gridPosition.x, gridPosition.y); // 스네이크 게임 오브젝트 위치 업데이트
    }
}
