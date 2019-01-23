using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRelativeTo : MonoBehaviour {

   public GameObject gameObjectToFollow;
   private Vector3 offset;
   void Start () {
     offset = transform.position - gameObjectToFollow.transform.position;
   }
	
   // Update is called once per frame
   void LateUpdate () {
     transform.position = gameObjectToFollow.transform.position + offset;
   }
}
