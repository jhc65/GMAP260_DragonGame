using System.Collections;
using UnityEngine;

public class MoveTowards : MonoBehaviour {
    private Transform target;
    public float speed = 5f;

    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
   

	void Update () {
        
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
