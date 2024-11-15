using System;
using System.Collections.Generic;
using UnityEngine;

public class OneLineController : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private List<Cell> _cells = new List<Cell>();
    [SerializeField] private List<Vector3> _pathPoints = new List<Vector3>();
    [SerializeField] private int _cellCount;
    [SerializeField] private float _offset;
    private Camera _camera;
    private bool _isDrawing;
    private bool _isWin;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            if (Input.GetMouseButton(0) && !_isWin)
            {
                var ray = GetRayOnMousePosition();
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f);
                if (hit.collider != null)
                {
                    Cell cell = hit.collider.GetComponent<Cell>();
                    if (_cells.Count == 0 && !cell.IsStart)
                    {
                        return;
                    }

                    _lineRenderer.enabled = true;
                    int cellIndex = _cells.IndexOf(cell);

                    if (cellIndex >= 0)
                    {
                        if (_cells.Count > 1 && cell == _cells[_cells.Count - 2])
                        {
                            _cells[_cells.Count - 1].ResetCell();
                            _cells.RemoveAt(_cells.Count - 1);
                            _pathPoints.RemoveAt(_pathPoints.Count - 1);
                            UpdateLineRenderer();
                        }
                    }
                    else if (IsValidNextCell(cell))
                    {
                        cell.Picked();
                        _isDrawing = true;
                        _cells.Add(cell);
                        _pathPoints.Add(cell.transform.position);
                    }

                    UpdateLineRenderer();
                    CheckWin();
                }
            }
        }
    }
    
    private void CheckWin()
    {
        if (_cells.Count == _cellCount)
        {
            Cell lastCell = _cells[_cells.Count - 1];
            lastCell.End();
            _isWin = true;
            GameUIManager.Instance.Confetti(true);
            if (ResourceManager.OneLine < 12)
            {
                ResourceManager.OneLine++;
            }
            else
            {
                ResourceManager.OneLine = 1;
            }
        }
    }
    
    private void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = _pathPoints.Count;
        _lineRenderer.SetPositions(_pathPoints.ToArray());
    }
    
    private Ray GetRayOnMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        return _camera.ScreenPointToRay(mousePosition);
    }

    private bool IsValidNextCell(Cell newCell)
    {
        if (_cells.Count == 0) return true;

        Cell lastCell = _cells[_cells.Count - 1];
        Vector3 lastPosition = lastCell.transform.localPosition;
        Vector3 newPosition = newCell.transform.localPosition;

        bool isSameRow = Mathf.Abs(newPosition.y - lastPosition.y) < 0.1f;
        bool isSameColumn = Mathf.Abs(newPosition.x - lastPosition.x) < 0.1f;

        float distance = Vector3.Distance(newPosition, lastPosition);

        if ((isSameRow || isSameColumn) && distance <= _offset)
        {
            return true;
        }

        return false;
    }
    
    public void ResetLevel()
    {
        AudioManager.PlaySound("Click");
        GameUIManager.Instance.Reload();
    }
}
