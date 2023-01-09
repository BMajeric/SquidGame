using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public bool Done {get; private set;}

    private CookieManager manager;  

    public void initialize(CookieManager manager){
        this.manager = manager;
    }
 


  /// <summary>
  /// Called every frame while the mouse is over the GUIElement or Collider.
  /// </summary>
  private void OnMouseOver()
  {
      if (Input.GetMouseButton(0)){
        GetComponent<Renderer>().material.color = Color.cyan;
        Done=true;
        manager.CheckForWin();
      }
       Debug.Log("part clicked");
  }
}
