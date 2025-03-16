using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DragAndDropObject : MonoBehaviour
{
    Vector3 mousePosition;

    private float ylock;

    private float gridSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //chuyển object pos từ world sang screen
    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    //Nhấn lưu vị trí chênh lệch giữa chuột và object vào mousePosition
    private void OnMouseDown()
    {
        ylock = transform.position.y;
        mousePosition = Input.mousePosition - GetMousePos();
    }

    //Chuyển tọa độ chuột trên màn hình thành tọa độ thế giới và gán vào object pos
    private void OnMouseDrag()
    {
        //Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        
        transform.position = OnSnapGrid();

        
    }

    private Vector3 OnSnapGrid()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);

        targetPos.x = Mathf.Round(targetPos.x / gridSize) * gridSize;
        targetPos.z = Mathf.Round(targetPos.z / gridSize) * gridSize;

        targetPos = new Vector3(targetPos.x, ylock, targetPos.z);

        return targetPos;
    }
}
