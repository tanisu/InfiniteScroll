using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public enum Direction
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
    public float stageSpeed;

    public Direction moveDirection;
    [SerializeField] GameObject tilePrefab,treePrefab,housePrefab;
    private float tileSize,height,width;
    const int BUFFER = 2;
    


    private void Start()
    {
        if (tilePrefab.GetComponent<Renderer>() != null)
        {
            tileSize = tilePrefab.GetComponent<Renderer>().bounds.size.y;
        }

        SetTiles();
    }


    void SetTiles()
    {
        
        //画面に対しての縦のユニット数
        float cameraHeight = 2f * Camera.main.orthographicSize;
        //画面に対しての横のユニット数
        float cameraWidth = cameraHeight * Camera.main.aspect;

        //タイルを横幅一杯に並べれる数
        int tilesX = Mathf.CeilToInt(cameraWidth / tileSize) % 2 == 1 ? Mathf.CeilToInt(cameraWidth / tileSize) + 1 : Mathf.CeilToInt(cameraWidth / tileSize);
        tilesX += BUFFER;
        //タイルを縦幅一杯に並べられる数
        int tilesY = Mathf.CeilToInt(cameraHeight / tileSize) % 2 == 1 ? Mathf.CeilToInt(cameraHeight / tileSize) + 1 : Mathf.CeilToInt(cameraHeight / tileSize);
        //タイル縦幅にバッファをもたせる
        tilesY += BUFFER;

        //高さ計算
        height = tilesY * tileSize;
        //横幅計算
        width = tilesX * tileSize;
        
        //Y座標の開始位置
        float startY = tilesY / 2 * tileSize;

        //X座標の開始位置
        float startX = tilesX / 2 * tileSize;
        if (gameObject.tag == "Original")
        {
            for (float y = 0; y <= tilesY; y++)
            {
                for (float x = 0; x <= tilesX; x++)
                {
                    Vector2 spawnPosition = new Vector2(x * tileSize - startX, y * tileSize - startY);
                    Instantiate(tilePrefab, spawnPosition, Quaternion.identity,transform);
                    if(x%2 == 0 && Random.Range(0,5) == 4)
                    {
                        Instantiate(treePrefab, spawnPosition, Quaternion.identity,transform);
                    }

                    if(x%2 == 1 && Random.Range(0,12) == 11)
                    {
                        Instantiate(housePrefab, spawnPosition, Quaternion.identity, transform);
                    }
                }
            }
            Vector3 clonePos;
            switch (moveDirection)
            {
                case Direction.UP:
                    clonePos = new Vector3(0, -(height + tileSize), 0);
                    break;
                case Direction.RIGHT:
                    clonePos = new Vector3(-(width + tileSize), 0);
                    break;
                case Direction.DOWN:
                    clonePos = new Vector3(0, height + tileSize, 0);
                    break;
                case Direction.LEFT:
                    clonePos = new Vector3(width + tileSize, 0);
                    break;
                default :
                    clonePos = new Vector3(0, height + tileSize, 0);
                    break;
            }
            GameObject clone = Instantiate(gameObject, clonePos, Quaternion.identity);
            clone.tag = "Clone";
        }

    }


    void Update()
    {
        
        switch (moveDirection)
        {
            case Direction.UP:
                transform.Translate(Vector3.up * Time.deltaTime * stageSpeed);

                if (transform.position.y > height)
                {
                    float remainder = (transform.position.y - (height + tileSize)) % height;
                    transform.position = new Vector3(0, (remainder - height) - tileSize, 0);
                }
                break;
            case Direction.RIGHT:
                transform.Translate(Vector3.right * Time.deltaTime * stageSpeed);
                if (transform.position.x > width)
                {
                    float remainder = (transform.position.x - (width + tileSize)) % width;
                    transform.position = new Vector3((remainder - width) - tileSize, 0, 0);
                }
                break;
            case Direction.DOWN:
                transform.Translate(Vector3.down * Time.deltaTime * stageSpeed);
                if (transform.position.y < -(height))
                {
                    float remainder = (transform.position.y + (height + tileSize)) % height;
                    transform.position = new Vector3(0, remainder + height + tileSize, 0);
                }
                break;
            case Direction.LEFT:
                transform.Translate(Vector3.left * Time.deltaTime * stageSpeed);
                if (transform.position.x < -width)
                {
                    float remainder = (transform.position.x + (width + tileSize)) % width;
                    transform.position = new Vector3(remainder + width + tileSize, 0, 0);
                }
                break;
            default:
                break;
        }

      
        



    }
}


