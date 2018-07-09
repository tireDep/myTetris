﻿// 실제적인 게임 실행 클래스, grid에 적용됨
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public static int gridWidth = 10;
    public static int gridHeight = 20;
    // 테트리스 게임 화면 크기(블록 공간)

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    // 각각의 블록이 차지하는 공간 변수

	// Use this for initialization
	void Start () {
        SpawnNextTetromino();   // 랜덤으로 블록 자동 생성
    }   // 함수 끝

    // 업데이트 클래스는 필요없으므로 삭제함     -> 프레임당 불러올게 없음

    public bool CheckIsAboveGrid(Tetromino tetromino)   // 블록이 맨 위에 닿았는지 검사
    {
        for(int x=0;x<gridWidth;++x)
        {
            foreach(Transform mino in tetromino.transform)
            {
                Vector2 pos = Round(mino.position);

                if(pos.y>gridHeight-1)
                {
                    return true;
                }
            }
        }
        return false;
    }   // 함수 끝

    public bool IsFullRowAt(int y) // 행이 다 차있는지 검사하는 함수
    {
        for(int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }   // 함수 끝

    public void DeleteMinoAt(int y) // 블록 제거
    {
        for(int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);

            grid[x, y] = null;
        }
    }   // 함수 끝

    public void MoveRowDown(int y)  // 행 내리기
    {
        for(int x = 0;x < gridWidth;++x)
        {
            if(grid[x,y]!=null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }   // 함수 끝

    public void MoveAllRowsDown(int y)  // 전체 행 내리기
    {
        for (int i = y; i < gridHeight; ++i)
            MoveRowDown(i);
    }   // 함수 끝

    public void DeleteRow() // 행 삭제
    {
        for(int y = 0; y < gridHeight; ++y)
        {
            if(IsFullRowAt(y))  // 행이 다 차있을 경우 
            {
                DeleteMinoAt(y);    // 블록제거
                MoveAllRowsDown(y + 1); // 행 내리기
                --y;
            }
        }
    }   // 함수 끝

    public void UpdateGrid(Tetromino tetromino) // 전체 공간 계산(남은공간)
    {
        for(int y=0; y<gridHeight;++y)
        {
            for(int x=0; x<gridWidth; ++x)
            {
                if(grid[x,y]!=null)
                {
                    if (grid[x, y].parent == tetromino.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach(Transform mino in tetromino.transform)  // 블록이 차지하고 있는 공간 계산
        {
            Vector2 pos = Round(mino.position);

            if(pos.y<gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }   // 함수 끝

    public Transform GetTransformAtGridPosition(Vector2 pos)    // 블록이 차지하고 있는 공간 계산
    {
        if (pos.y > gridHeight - 1)
            return null;
        else
            return grid[(int)pos.x, (int)pos.y];
    }   // 함수 끝

    public void SpawnNextTetromino()    // 다음 블록 생성
    {
        GameObject nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(5.0f, 20.0f), Quaternion.identity);
        // 랜덤으로 다음 블록 생성, 위치정보, 회전정보
    }   // 함수 끝

    public bool CheckInsideGrid(Vector2 pos)    // 창 안에 있는지 유무
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
        // 가로가 0보다 크거나 최대크기보다 작음 & 세로가 최소크기보다 크거나 같음
    }   // 함수 끝

    public Vector2 Round(Vector2 pos)   // 위치값 반올림
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        // 해당 위치 값 반올림
    }   // 함수 끝

    string GetRandomTetromino() // 랜덤 블록 생성
    {
        int randomTetromino = Random.Range(1, 8);   // 랜덤 지정
        string randomTetrominoName = "Prefabs/TBlock";  // 기본 지정 블록
        // 프리팹 내부에 블록이 존재하기 때문에 Prefabs/추가

        switch(randomTetromino)
        {
            case 1:
                randomTetrominoName = "Prefabs/TBlock";
                break;
            case 2:
                randomTetrominoName = "Prefabs/IBlock";
                break;
            case 3:
                randomTetrominoName = "Prefabs/OBlock";
                break;
            case 4:
                randomTetrominoName = "Prefabs/LBlock";
                break;
            case 5:
                randomTetrominoName = "Prefabs/L2Block";
                break;
            case 6:
                randomTetrominoName = "Prefabs/SBlock";
                break;
            case 7:
                randomTetrominoName = "Prefabs/S2Block";
                break;
        }
        return randomTetrominoName;
    }   // 함수 끝

    public void GameOver()  // 게임오버
    {
        Application.LoadLevel("GameOver");
    }   // 함수 끝
}
