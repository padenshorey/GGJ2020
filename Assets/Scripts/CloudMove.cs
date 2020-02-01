using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{

    public float cloudSpeed = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        this.transform.position = new Vector3(this.transform.position.x + cloudSpeed, this.transform.position.y, this.transform.position.z);
        if(this.transform.position.x < -40)
            this.transform.position = new Vector3(40, this.transform.position.y);
        if (this.transform.position.x > 40)
            this.transform.position = new Vector3(-40 + cloudSpeed, this.transform.position.y);

    }
}
