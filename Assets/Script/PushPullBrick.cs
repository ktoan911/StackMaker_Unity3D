using UnityEngine;

public class PushPullBrick : MonoBehaviour
{
    [SerializeField] private GameObject pushPullBrick;
    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Player" && this.gameObject.tag == "UnBrick")
        {
            this.pushPullBrick.SetActive(true);
        }

        if (other.tag == "Player" && this.gameObject.tag == "Brick")
        {
            this.pushPullBrick.SetActive(false);
        }
    }


}
