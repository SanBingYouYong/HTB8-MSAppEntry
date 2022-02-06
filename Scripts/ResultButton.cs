using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultButton : MonoBehaviour
{

    //bool clicked = false;
    private string buttonName;

    public void ThisResultButtonClicked()
    {
        //Debug.Log("Result Clicked");
        //Debug.Log(this.GetComponentInChildren<Text>().text);
        var buttonName = EventSystem.current.currentSelectedGameObject.name + "Name";
        //Debug.Log(buttonName);
        Debug.Log("clicked: " + GameObject.Find(buttonName).GetComponent<Text>().text);
        buttonName = GameObject.Find(buttonName).GetComponent<Text>().text;
        //clicked = true;
        var search_go = GameObject.Find("SearchOverlay");
        Debug.Log(search_go.name);
        search_go.GetComponent<Search>().clickedResultProductName = buttonName;
        search_go.GetComponent<Search>().DisplayDetail();
    }

    void Update()
    {
        //if (clicked)
        //{
        //    Debug.Log("clicked triggered");
        //    var search_go = GameObject.Find("Search");
        //    search_go.GetComponent<Search>().clickedResultProductName = buttonName;
        //    clicked = false;
        //}
    }
}
