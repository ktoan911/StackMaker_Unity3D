using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;

    private PlayerChangePos playerChangePos;

    protected Rigidbody rb;

    [SerializeField] public float playerSpeed;

    [SerializeField] private LayerMask layerMoveAvailable;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private LayerMask layerUnBrick;

    private Vector3 beginPlayerPos;

    private Vector3 mouseBeginPos;

    private Vector3 roadCtrl;

    public Vector3 targetPos;

    public string mouseRotation;
    public string autoMoveRotation;
    public string mousePastRot;

    private RaycastHit hitWall;
    private RaycastHit hitUnbrick;

    public bool isMove;

    private void Awake()
    {
        PlayerMovement.instance = this;

        this.rb = GetComponent<Rigidbody>();

        playerChangePos = GetComponent<PlayerChangePos>();

        this.beginPlayerPos = this.transform.position;
    }

    private void Start()
    {
        rb.freezeRotation = true;

        this.isMove = false;

        this.playerSpeed = 15f;

        this.mouseRotation = "stay";

    }
    private void Update()
    {
        if(!UIManager.instance.IsStart)
        {
            this.targetPos = this.beginPlayerPos;
        }

        this.UpdateMoving();
        if (isMove)
        {
            this.Moving();
        }

        if (CheckOnUnbrick() && playerChangePos.listBrick.Count > 0 && isMove == false)
        {
            AutoMove();
        }

        if (CheckOnUnbrick())
        {
            if (playerChangePos.listBrick.Count <= 0)
            {
                this.targetPos.z = hitUnbrick.collider.transform.position.z;
                this.targetPos.x = hitUnbrick.collider.transform.position.x;
                this.targetPos.y = 6.2f;
                this.playerChangePos.ChangePosUp();

                UIManager.instance.gameOver.SetActive(true);
            }
        }
    }
    private void UpdateMoving()
    {
        if (this.mouseRotation != "stay")
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && CheckOnMoveAvailable())
        {
            mouseBeginPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && CheckOnMoveAvailable())
        {
            roadCtrl = Input.mousePosition - mouseBeginPos;
            InputMove();
            TarGetPosition();
            isMove = true;
        }
    }
    protected virtual void InputMove()
    {
        if (Vector3.Angle(Vector3.up, roadCtrl) <= 45f && roadCtrl.magnitude > 30)
        {
            this.mouseRotation = "up";
        }

        else if (Vector3.Angle(Vector3.down, roadCtrl) <= 45f && roadCtrl.magnitude > 30)
        {
            this.mouseRotation = "down";
        }

        else if (Vector3.Angle(Vector3.right, roadCtrl) <= 45f && roadCtrl.magnitude > 30)
        {
            this.mouseRotation = "right";
        }

        else if (Vector3.Angle(Vector3.left, roadCtrl) <= 45f && roadCtrl.magnitude > 30)
        {
            this.mouseRotation = "left";
        }
        else
        {
            this.mouseRotation = "stay";
        }
    }
    public virtual void Moving()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPos, playerSpeed * Time.deltaTime);
        if ((this.transform.position - this.targetPos).magnitude < 0.001f && this.playerChangePos.isMoveContinue == false)
        {
            isMove = false;
            this.mouseRotation = "stay";

        }
    }

    public bool CheckOnMoveAvailable()
    {

        RaycastHit hit;
        Vector3 rayPos = this.transform.position;
        rayPos.y = 10f;
        if (Physics.Raycast(rayPos, Vector3.down, out hit, 10f, layerMoveAvailable))
        {
            return true;
        }
        return false;
    }

    private bool CheckOnUnbrick()
    {
        Vector3 rayPos = this.transform.position;
        rayPos.y = 10f;
        if (Physics.Raycast(rayPos, Vector3.down, out hitUnbrick, 5f, layerUnBrick))
        {
            return true;
        }
        return false;
    }
    public void TarGetPosition()
    {
        targetPos = this.transform.position;
        Vector3 rayPos = this.transform.position;
        rayPos.y = 10f;
        if (this.mouseRotation == "up")
        {
            while (Physics.Raycast(rayPos, Vector3.down, out hitWall, 10f, layerMoveAvailable))
            {
                rayPos.z += 1;
                targetPos.z += 1;
            }
            mousePastRot = "up";
            targetPos.z -= 1f;

        }
        else if (this.mouseRotation == "down")
        {
            while (Physics.Raycast(rayPos, Vector3.down, out hitWall, 10f, layerMoveAvailable))
            {
                rayPos.z -= 1;
                targetPos.z -= 1;
            }
            mousePastRot = "down";
            targetPos.z += 1f;
        }
        else if (this.mouseRotation == "right")
        {
            while (Physics.Raycast(rayPos, Vector3.down, out hitWall, 10f, layerMoveAvailable))
            {
                rayPos.x += 1;
                targetPos.x += 1;
            }
            mousePastRot = "right";
            targetPos.x -= 1f;
        }
        else if (this.mouseRotation == "left")
        {
            while (Physics.Raycast(rayPos, Vector3.down, out hitWall, 10f, layerMoveAvailable))
            {
                rayPos.x -= 1;
                targetPos.x -= 1;
            }
            mousePastRot = "left";
            targetPos.x += 1f;
        }
        else
        {
            mousePastRot = "null";
        }
    }

    private void AutoMove()
    {
        if (mousePastRot == "up" || mousePastRot == "down")
        {
            CheckUnBrickRightLeft();
            if (autoMoveRotation == "right")
            {
                
                mouseRotation = "right";
                isMove = true;
                TarGetPosition();
            }
            else if (autoMoveRotation == "left")
            {
                mouseRotation = "left";
                isMove = true;
                TarGetPosition();
            }
        }
        else if (mousePastRot == "left" || mousePastRot == "right")
        {
            CheckUnBrickUpDown();
            if (autoMoveRotation == "up")
            {
                mouseRotation = "up";
                isMove = true;
                TarGetPosition();
            }
            else if (autoMoveRotation == "down")
            {
                mouseRotation = "down";
                isMove = true;
                TarGetPosition();
            }
        }

    }

    private bool CheckUnBrickUpDown()
    {
        RaycastHit hitWall;
        Vector3 rayPos = this.transform.position;
        rayPos.y = 8f;
        rayPos.z += 1f;
        if (Physics.Raycast(rayPos, Vector3.down, out hitWall, 5f, layerUnBrick))
        {
            autoMoveRotation = "up";
            return true;
        }
        rayPos.z -= 2f;
        if (Physics.Raycast(rayPos, Vector3.down, out hitWall, 5f, layerUnBrick))
        {
            autoMoveRotation = "down";
            return true;
        }
        else
        {
            autoMoveRotation = "NotNext";
            return false;
        }
    }

    private bool CheckUnBrickRightLeft()
    {
        RaycastHit hitWall;
        Vector3 rayPos = this.transform.position;
        rayPos.y = 8f;
        rayPos.x += 1;
        if (Physics.Raycast(rayPos, Vector3.down, out hitWall, 5f, layerUnBrick))
        {

            autoMoveRotation = "right";
            return true;
        }
        rayPos.x -= 2;
        if (Physics.Raycast(rayPos, Vector3.down, out hitWall, 5f, layerUnBrick))
        {

            autoMoveRotation = "left";
            return true;
        }
        else
        {
            autoMoveRotation = "NotNext";
            return false;
        }
    }
}




//Debug.DrawRay(transform.position + new Vector3(0, 2.5f, 0), new Vector3(0, -3, -1) * 1.3f, Color.red, 0.5f);

//this.velocity.x = this.playerSpeed;
//rb.MovePosition(this.rb.position - this.velocity * Time.deltaTime);

//Debug.DrawRay(transform.position + new Vector3(0, 2.5f, 0), new Vector3(0, -2, 0.7f) * 1.5f, Color.red, 0.5f);
