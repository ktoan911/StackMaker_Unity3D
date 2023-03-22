using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using UnityEngine;

public class PlayerChangePos : MonoBehaviour
{
    public static PlayerChangePos instance;

    public float brickThickness;

    private Vector3 spawnPos;

    [SerializeField] private GameObject brickPrefab;
    private GameObject brick;

    public List<GameObject> listBrick;

    private PlayerMovement playerMovement;

    private GameObject brickCloneGroup;

    public bool isMoveContinue;
    public bool isWin;
    [SerializeField] private LayerMask layerAutoRot;

    [SerializeField] private GameObject prizeClose;
    [SerializeField] private GameObject prizeOpen;

    private void Awake()
    {
        instance= this;

        listBrick = new List<GameObject>();

        playerMovement = GetComponent<PlayerMovement>();

        brickCloneGroup = GameObject.Find("BrickClone");
    }
    private void Start()
    {
        brickThickness = 0.3f;

        isMoveContinue= false;

        this.isWin = false;
    }
    private void Update()
    {
        UpDateBrickPos();
        AutoRot();
    }

    private void UpDateBrickPos()
    {
        Vector3 brickPos = this.transform.position;
        for (int i = listBrick.Count - 1; i >= 0; i--)
        {
            brickPos.y -= brickThickness;
            listBrick[i].transform.position = brickPos;
        }
    }

    public void ChangePosUp()
    {
        playerMovement.targetPos.y += brickThickness;
        OnSpawn();
    }

    public void ChangePosDown()
    {
        if (listBrick.Count <= 0) return;
        this.playerMovement.targetPos.y -=brickThickness;
        OnDespawn();
    }

    private void OnSpawn()
    {
        spawnPos = playerMovement.targetPos;
        spawnPos.y -= brickThickness;
        brick = Instantiate(brickPrefab, spawnPos, brickPrefab.transform.rotation);
        listBrick.Add(brick);
        brick.transform.parent = brickCloneGroup.transform;
    }

    private void OnDespawn()
    {
        Destroy(listBrick[0]);
        listBrick.RemoveAt(0);
    }

    private void AutoMoveToWin()
    {
        if (this.playerMovement.mousePastRot == "up")
        {
            this.playerMovement.targetPos.z += 7;
            this.playerMovement.targetPos.y = 6.2f;
            this.playerMovement.Moving();
        }
        else if (this.playerMovement.mousePastRot == "down")
        {
            this.playerMovement.targetPos.z -= 7;
            this.playerMovement.targetPos.y = 6.2f;
            this.playerMovement.Moving();
        }
        else if (this.playerMovement.mousePastRot == "right")
        {
            this.playerMovement.targetPos.x += 7;
            this.playerMovement.targetPos.y = 6.2f;
            this.playerMovement.Moving();
        }
        else if (this.playerMovement.mousePastRot == "left")
        {
            this.playerMovement.targetPos.x -= 7;
            this.playerMovement.targetPos.y = 6.2f;
            this.playerMovement.Moving();
        }
    }
    private void AutoRot()
    {
        Vector3 rayPos = this.transform.position;
        rayPos.y = 10f;
        RaycastHit hit;
        if (Physics.Raycast(rayPos, Vector3.down, out hit, 10f, layerAutoRot) && !this.playerMovement.isMove)
        {
            if (hit.transform.eulerAngles.y - 90f < 0.01f)
            {
                this.playerMovement.mouseRotation = "left";
            }
            else if (hit.transform.eulerAngles.y - 180f < 0.01f)
            {
                this.playerMovement.mouseRotation = "right";
            }
            else if (hit.transform.eulerAngles.y - 270f < 0.01f)
            {
                this.playerMovement.mouseRotation = "down";
            }
            else if (hit.transform.eulerAngles.y < 0.01f)
            {
                this.playerMovement.mouseRotation = "up";
            }
            else
            {
                Debug.Log("iie");
            }
            this.playerMovement.TarGetPosition();
            this.playerMovement.isMove = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Brick")
        {
            ChangePosUp();
            UIManager.instance.SetBrickCount(listBrick.Count);
        }
        if (other.tag == "UnBrick")
        {
            ChangePosDown();
            UIManager.instance.SetBrickCount(listBrick.Count);
        }
        if (other.tag == "CheckPoint")
        {
            this.playerMovement.targetPos.z = other.transform.position.z;
            this.playerMovement.targetPos.x = other.transform.position.x;
            this.playerMovement.mouseRotation = "stay";
        }
        if(other.tag == "BeginWinPos")
        {
            listBrick.Clear();
            UIManager.instance.SetBrickCount(listBrick.Count);
            AutoMoveToWin();
        }
        if(other.tag == "Prize")
        {
            UIManager.instance.endGame.SetActive(true);
            this.prizeClose.SetActive(false);
            this.prizeOpen.SetActive(true);
            isWin = true;
        }
    }

}


