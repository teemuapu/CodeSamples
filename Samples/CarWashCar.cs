using PathCreation;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//From the game "Pump chumps" [https://artturs.itch.io/pump-chumps]
//This script includes the code for the "Car Wash" minigame, where
//the player is required to match randomized colors and numbers.
public class CarWashCar : MonoBehaviour
{
    public float speed;
    public int program;
    public int time;
    public bool wait;
    public float origSpeed;
    public bool taskActive;
    public bool rollable;
    public bool exit;

    public bool fail;

    public Vector3 startPos;

    public Image programImg;
    public Text timeText;
    public Sprite[] colours;

    public CarWashScript carWashScript;
    private PhotonView photonView;

    public float carSpeed;
    public PathCreator startPath;
    public PathCreator endPath;
    public PathCreator failPath;
    public bool startPathActive;
    public float distanceTravelled;
    public EndOfPathInstruction endOfPathInstruction;

    void Start()
    {
        rollable = true;
        photonView = GetComponent<PhotonView>();
        startPath = GameObject.Find("Carwash Car Path Start").GetComponent<PathCreator>();
        endPath = GameObject.Find("Carwash Car Path End").GetComponent<PathCreator>();
        failPath = GameObject.Find("Carwash Car Path Fail").GetComponent<PathCreator>();
        startPos = transform.position;
        origSpeed = speed;
        exit = true;
        startPathActive = true;
    }
    public void ReRoll()
    {
        program = Random.Range(0, 4);
        time = Random.Range(9, 30);
        programImg.sprite = colours[program];
        timeText.text = time.ToString();

        photonView.RPC("RPC_ReRollToOthers", RpcTarget.Others, program, time);
    }

    [PunRPC]
    public void RPC_ReRollToOthers(int mProgram, int mTime)
    {
        program = mProgram;
        time = mTime;
        programImg.sprite = colours[program];
        timeText.text = time.ToString();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Finish" && rollable)
        {
            carWashScript.carsIn++;
            wait = true;
            distanceTravelled = 0;
            speed = 0;
            startPathActive = false;
            carWashScript.currentCar = this.gameObject;
            rollable = false;

            carWashScript.timerActive = true;

            if (PhotonNetwork.IsMasterClient)
            {
                ReRoll();
            }
        }
        if (col.tag == "ReturnCar" && exit)
        {
            carWashScript.carsIn--;
            speed = 0;
            exit = false;
            wait = false;
        }
        if (col.tag == "Respawn")
        {
            Destroy(gameObject);

        }


    }

    void Update()
    {
        transform.position = transform.position + new Vector3(0, 0, speed * Time.deltaTime);

        if (!wait && startPathActive)
        {
            distanceTravelled += carSpeed * Time.deltaTime;
            gameObject.transform.position = startPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            gameObject.transform.rotation = startPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        } else if(fail)
        {
            distanceTravelled += carSpeed * Time.deltaTime;
            gameObject.transform.position = failPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            gameObject.transform.rotation = failPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }
        else if(!wait && !startPathActive)
        {
            distanceTravelled += carSpeed * Time.deltaTime;
            gameObject.transform.position = endPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            gameObject.transform.rotation = endPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }

    }

    void FixedUpdate()
    {
        if (!carWashScript)
        {
            carWashScript = GameObject.FindGameObjectWithTag("Motomaatti").GetComponent<CarWashScript>();
            carWashScript.currentCar = this.gameObject;
        }
    }

}
