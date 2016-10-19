using UnityEngine;
using System.Collections;
using character;

public class cast : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        GameObject.Find("GameObject").GetComponent<battle>().expectedposition.transform.localPosition = new Vector2(0, GameObject.Find("GameObject").GetComponent<battle>().expectedposition.transform.localPosition.y);
    }

}
