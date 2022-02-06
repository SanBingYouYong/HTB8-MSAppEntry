using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Overlay : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // not building one anymore
        //#if UNITY_STANDALONE
        //        Screen.SetResolution(1080, 1920, true);
        //#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonAlphaChange(string oriButtonName, string newButtonName, bool noNewButton = false)
    {
        var oriButton = GameObject.Find(oriButtonName);
        // decreasing alpha to 0% for original button
        var color = oriButton.GetComponent<Image>().color;
        color.a = 0f;
        oriButton.GetComponent<Image>().color = color;
        if (!noNewButton)
        {
            var newButton = GameObject.Find(newButtonName);
            // increasing alpha to 50% for new button
            color = newButton.GetComponent<Image>().color;
            color.a += 0.5f;
            newButton.GetComponent<Image>().color = color;
        }
    }

    public void MenuClicked()
    {
        var panel_go = GameObject.Find("Panel");
        panel_go.GetComponent<Image>().sprite = Resources.Load<Sprite>("menu");
        ButtonAlphaChange("Menu", "Store Finder");
    }
    public void StoreFinderClicked()
    {
        var panel_go = GameObject.Find("Panel");
        panel_go.GetComponent<Image>().sprite = Resources.Load<Sprite>("store_finder");
        ButtonAlphaChange("Store Finder", "Use Current");
    }
    public void UseCurrentClicked()
    {
        var panel_go = GameObject.Find("Panel");
        panel_go.GetComponent<Image>().sprite = Resources.Load<Sprite>("use_current(search Edi)");
        ButtonAlphaChange("Use Current", "Shop");
    }
    public void ShopClicked()
    {

        SceneManager.LoadScene("Search");
    }
}
