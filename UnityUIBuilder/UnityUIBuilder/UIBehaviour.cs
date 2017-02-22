using UnityEngine;
using System.Collections;

public class UIBehaviour : MonoBehaviour {

	public void SayHello()
    {
        Debug.Log("Hello!");
    }

    void RegisterMe(GameObject obj)
    {
        Debug.Log("I registerd the object " + obj);
    }
}
