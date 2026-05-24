using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
		{
			Debug.Log(MouseWorld.GetPosition());
		}
    }
}
