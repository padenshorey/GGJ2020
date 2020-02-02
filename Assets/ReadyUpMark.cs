using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyUpMark : MonoBehaviour
{
    bool rdy = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(this.GetComponent<SpriteRenderer>().enabled == false)
        {
            foreach (Transform eachChild in transform)
            {
                Debug.Log(eachChild.name);
                Debug.Log(eachChild.tag);


                if(eachChild.tag.Equals("Untagged"))
                {
                    if(eachChild.GetComponent<PlayerController>().ReadyToPlay )
                        rdy = true;
                }
                if (eachChild.tag.Equals( "ReadyCheckmark") && rdy == true)
                { 
                    eachChild.GetComponent<SpriteRenderer>().enabled = true;
                    Debug.Log("enabled checkmark");
                }



            }
                
        }
        
    }
}
