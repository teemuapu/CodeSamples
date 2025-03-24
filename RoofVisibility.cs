using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//From the game "Pump chumps" [https://artturs.itch.io/pump-chumps]
//This script allows players to see inside the building as long as there is no one on the roof
public class RoofVisibility : MonoBehaviour
{
    public GameObject roof;
    public GameObject roofOthers;
    public GameObject numbers;

    private bool meOnRoof;

    //Number of other players on the roof
    private int friendsOnRoof;

    private Renderer[] rs;
    private Canvas[] cv;

    private void Start()
    {
        rs = roof.GetComponentsInChildren<Renderer>();
        cv = roof.GetComponentsInChildren<Canvas>();
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            if(other.gameObject.GetComponent<PhotonView>().IsMine == true){
                meOnRoof = true;
                roofOthers.SetActive(false);

                ShowRoof();
            }
            else{
                friendsOnRoof++;
                if(!meOnRoof)
                {
                    roofOthers.SetActive(true);
                }
                
            }
        }
        
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            if(other.gameObject.GetComponent<PhotonView>().IsMine == true){
                meOnRoof = false;
                
                HideRoof();
                if(friendsOnRoof > 0)
                {
                    roofOthers.SetActive(true);
                }
            }else{
                friendsOnRoof--;
                if(friendsOnRoof == 0){
                    roofOthers.SetActive(false);
                }
                
            }
        }
    }

    public void HideRoof()
    {
        if (rs.Length > 0 && cv.Length > 0)
        {
            foreach (Renderer r in rs)
                r.enabled = false;

            foreach (Canvas c in cv)
                c.enabled = false;
        }
    }
    
    public void ShowRoof()
    {
        if (rs.Length > 0 && cv.Length > 0)
        {
            foreach (Renderer r in rs)
                r.enabled = true;

            foreach (Canvas c in cv)
                c.enabled = true;
        }
    }

    public void UpdateComponents()
    {
        rs = roof.GetComponentsInChildren<Renderer>();
        cv = roof.GetComponentsInChildren<Canvas>();
    }
}
