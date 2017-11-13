using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stuffy : MonoBehaviour
{

    Transform newParent;
    public Text stuffyCount;
    public int stuffies;
    Vector3 position;

    void Start() {
        SetStuffyCount();
        position = transform.position;

    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Stealer"))
        {
            newParent = collision.transform;
            transform.parent = newParent; //changes stuffy parent
            gameObject.tag = "Untagged";

            Debug.Log("stuffy get!"); //displays in console
        }
        if (collision.gameObject.CompareTag("Door"))
        {
            stuffies--;
            SetStuffyCount();
            gameObject.SetActive(false);

        }
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.position = position; //return to original position

            Debug.Log("return"); //displays in console
        }

    }

    //updates stuffy count
    void SetStuffyCount() {
        stuffyCount.text = "Stuffies Left: " + stuffies.ToString(); 
    }
}
