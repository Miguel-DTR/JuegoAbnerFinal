using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseWorld : MonoBehaviour
{
	private static MouseWorld instance;
	
	[SerializeField] private LayerMask mouseFloorLayerMask;
	
    void Awake()
    {
        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Vector2 screenPos = 
			Mouse.current.position.ReadValue();
		
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		
		Physics.Raycast(
			ray,	// configuración del rayo, inicio, dirección
			out RaycastHit hit, // info del impacto
			float.MaxValue, 	// distancia
			instance.mouseFloorLayerMask);	// capas a interactuar
			
		return hit.point;
    }
}
