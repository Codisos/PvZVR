using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour, ILateInit
{
    [SerializeField] private ZombieType type;

    GridCord cord;
    private int currentTilePos = -2;
    private int tempTilePos = -2;
    private int lastTilePos = -2;
    public int CurrentRowPos => currentTilePos;
    public event Action<GridCord> OnRowChange;

    private Rigidbody rb;
    private Vector3 curPos;
    private Vector3 _endPos;     //set on enable
    private bool canMove = false;
    private float endBuffer = 1f;

    private float gridMin;
    private float gridMax;
    private float tileScale;

    public void LateInitialize()    //basic init
    {

    }

    public void OnSpawned(Vector3 endPos)
    {
        ResumeMovement();

        _endPos = endPos;

        cord = GridManager.Instance.GetTileCord(transform.position);
    }

    //initialize
    private void OnEnable()
    {

        //set temp endPos
        _endPos = new Vector3(0, 0, 0);

        canMove = true;

        LevelActions.OnGameOver += OnGameOver;

    }

    private void OnDisable()
    {
        LevelActions.OnGameOver -= OnGameOver;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gridMin = GridManager.Instance.validminmin.position.x;
        gridMax = GridManager.Instance.validmaxmax.position.x;
        tileScale = GridManager.Instance.tileScale;
    }

    void FixedUpdate()
    {
        

        if (canMove)
        {
            
            curPos = transform.position;

            tempTilePos = CalCurrentTilePos(curPos.x);

            if(tempTilePos != currentTilePos)
            {
                lastTilePos = currentTilePos;
                currentTilePos = tempTilePos;
                OnRowChanged(currentTilePos);
            }

            float distance = Vector3.Distance(curPos,_endPos);

            if(distance > endBuffer)
            {
                Vector3 dir = _endPos - curPos;
                dir.Normalize();

                rb.MovePosition(curPos + (dir * type.WalkSpeed.Value * Time.fixedDeltaTime));
            }
            else
            {
                PauseMovement();
                EndReached();
                // follow player or something???
            }
        }
    }

    public void PauseMovement()    //for eating, death, pause menu(that one with event, but not directly here)
    {
        canMove = false;

        rb.velocity = Vector3.zero;
        
    }

    void EndReached()
    {
        LevelActions.OnGameOver?.Invoke();
    }

    public void ResumeMovement()
    {
        canMove = true;
    }

    public void SetNewEndPoint(Vector3 newEnd)
    {
        _endPos = newEnd;
    }

    int CalCurrentTilePos(float x)
    {
        int cord = CalcValidCord(x) * -1;

        if (x > gridMin + tileScale / 2)
        {
            cord = -1;
        }
        else if (x < gridMax - tileScale / 2)
        {
            cord = 8;
        }

        if (cord == 0) cord = 1;

        return cord;
    }

    int CalcValidCord(float x)
    {
        float minX = gridMin + tileScale / 2;


        int cord = (int)Math.Round(((x - minX) / 1.5f));

        return cord;
    }

    void OnRowChanged(int row)
    {
        cord.row = row;
        OnRowChange?.Invoke(cord);
    }

    void OnGameOver()
    {
        PauseMovement();
    }

}
