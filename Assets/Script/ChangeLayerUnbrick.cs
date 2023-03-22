using System;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeLayerUnbrick : MonoBehaviour
{
    public int numberTimesMove;

    private void Start()
    {
        numberTimesMove = 0;
    }
    private void Update()
    {
        ChangeLayer();
    }

    private void ChangeLayer()
    {
        if(numberTimesMove % 2 == 0)
        {
            int LayerIgnoreRaycast = LayerMask.NameToLayer("UnBrick");
            this.gameObject.layer = LayerIgnoreRaycast;
            this.gameObject.tag = "UnBrick";
        }
        else
        {
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Brick");
            this.gameObject.layer = LayerIgnoreRaycast;
            this.gameObject.tag = "Brick";
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            numberTimesMove++;
        }
    }

}