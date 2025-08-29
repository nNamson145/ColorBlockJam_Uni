using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DragAndDropObject : MonoBehaviour
{
    private Vector3 mouseOffset;
    private Camera cam;
    private float yLock;
    private float gridSize = 1f;
    private Vector3 lastValidPosition; // Lưu vị trí hợp lệ gần nhất

    public LayerMask blockingLayer;

    void Start()
    {
        cam = Camera.main;
        lastValidPosition = transform.position; // Khởi tạo vị trí hợp lệ ban đầu
    }

    private void OnMouseDown()
    {
        Vector3 targetPos = OnSnapGrid(GetMouseWorldPos() - mouseOffset);

        yLock = transform.position.y;
        if (!IsBlocked(targetPos))
        {
            mouseOffset = GetMouseWorldPos() - transform.position;
        }

    }

    private void OnMouseDrag()
    {
        Vector3 targetPos = OnSnapGrid(GetMouseWorldPos() - mouseOffset);

        // Nếu vị trí mới không bị vật cản thì cập nhật vị trí
        if (!IsBlocked(targetPos))
        {
            transform.position = targetPos;
            lastValidPosition = targetPos; // Cập nhật vị trí hợp lệ
        }
        else
        {
            transform.position = lastValidPosition; // Giữ nguyên vị trí hợp lệ trước đó
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(mouseScreenPos);
    }

    private Vector3 OnSnapGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.z = Mathf.Round(position.z / gridSize) * gridSize;
        position.y = yLock; // Giữ nguyên trục Y
        return position;
    }

    private bool IsBlocked(Vector3 targetPos)
    {
        // Giảm kích thước kiểm tra để tránh lỗi va chạm giả
        Vector3 size = GetComponent<Collider>().bounds.extents * 0.9f;

        // Kiểm tra xem có Collider nào chặn vị trí mới không
        Collider[] colliders = Physics.OverlapBox(targetPos, size, Quaternion.identity, blockingLayer);

        return colliders.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 size = GetComponent<Collider>().bounds.extents * 0.9f;
        Gizmos.DrawWireCube(transform.position, size * 2);
    }
}