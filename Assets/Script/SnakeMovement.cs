using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [Header("Snake Movement")]
    [SerializeField] private float moveTime = 0.1f; // 한칸 이동 시간
    private Vector2 moveDir = Vector2.right; // 이동 방향
    // 실제 Snake 이동방향은 안바뀌었지만 입력 방향에 의해 같은 축으로 이동이 가능한 것을 방지
    private Vector2 lastInputDir = Vector2.right;

    [Header("Snake Segment")]
    [SerializeField] private Transform segmentPrefab; // Segment 프리팹
    [SerializeField] private int spawnSegmentCountAtStart = 4; // 게임 시작 시 Snake의 길이 (머리 포함)
    private List<Transform> segments = new List<Transform>(); // Snake와 Segment를 관리하는 리스트

    private IEnumerator Start()
    {
        Setup();

        while (true)
        {
            MoveSegments();
            yield return StartCoroutine("WaitforSeconds", moveTime);
        }
    }

    private void Update()
    {
        SnakeMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item")) AddSegment();
    }

    private void SnakeMove()
    {
        // 현재 x축으로 이동중이면 y축 방향으로만 방향 전환 가능
        if (moveDir.x != 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) lastInputDir = Vector2.up;

            else if (Input.GetKeyDown(KeyCode.DownArrow)) lastInputDir = Vector2.down;

        }
        // 현재 y축으로 이동중이면 x축 방향으로만 방향 전환 가능
        else if (moveDir.y != 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) lastInputDir = Vector2.left;
            else if (Input.GetKeyDown(KeyCode.RightArrow)) lastInputDir = Vector2.right;
        }
    }

    private IEnumerator WaitforSeconds(float time)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            yield return null;
        }
    }

    private void Setup()
    {
        // Snake 본체를 segments 리스트에 저장
        segments.Add(transform);

        // Snake를 쫓아다니는 꼬리 오브젝트를 생성하고, segments 리스트에 저장
        for (int i = 0; i < spawnSegmentCountAtStart; i++)
        {
            AddSegment();
        }
    }

    private void AddSegment()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    private void MoveSegments()
    {
        // 실제 이동할 때 마지막 입력 방향으로 이동하도록 설정
        moveDir = lastInputDir;

        for (int i = segments.Count - 1; i > 0; -- i)
        {
            segments[i].position = segments[i - 1].position;
        }

        transform.position = (Vector2)transform.position + moveDir;
    }
    
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Wall") GameManager.instance.GameOver();
    }
}
