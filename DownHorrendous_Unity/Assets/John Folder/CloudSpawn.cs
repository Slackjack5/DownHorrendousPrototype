using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawn : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, Random.Range(2,5) * Time.deltaTime);
        if (transform.position.z > 80) {
            transform.position = new Vector3(transform.position.x, transform.position.y, -20);
        }
    }
}
