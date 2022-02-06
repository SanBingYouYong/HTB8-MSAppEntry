using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Item
{
    public string id { get; set; }
    public string name { get; set; }

    //public string categories { get; set; }  // omitted fields for now
    public string description { get; set; }
    //public string image { get; set; }  // omitted fields for now
    //public string url { get; set; }  // omitted fields for now
    public string shelfID { get; set; }
    public string horiIndex { get; set; }

}

public class StaffInterface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // test code for Item creation
        //Item newItem = new Item()
        //{
        //    id = "123",
        //    name = "name",
        //    //categories = "23",
        //    description = "",
        //    //image = "",
        //    //url = "",
        //    shelfID = "",
        //    horiIndex = ""
        //};
        //AddNewItem(newItem);
        //DeleteItem("P60515662");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DeleteItem(string itemID)
    {
        string jsonPath = "Assets/StreamingAssets/databaseItems.json";
        string jsonString = File.ReadAllText(jsonPath);
        List<Item> list = JsonConvert.DeserializeObject<List<Item>>(jsonString);
        List<Item> NewList = new List<Item>();
        foreach (Item item in list)  // delete all with same ID
                                     // -> although it's not reasonable to have same ID, it's not impossible
        {
            if (!item.id.Equals(itemID))
            {
                NewList.Add(item);
            }
        }
        var convertedJson = JsonConvert.SerializeObject(NewList, Formatting.Indented);
        File.WriteAllText(jsonPath, convertedJson);
        Debug.Log(convertedJson.ToString());
    }


    void AddNewItem(Item newItem)
    {
        string jsonPath = "Assets/StreamingAssets/databaseItems.json";
        string jsonString = File.ReadAllText(jsonPath);
        var list = JsonConvert.DeserializeObject<List<Item>>(jsonString);

        list.Add(newItem);
        var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
        File.WriteAllText(jsonPath, convertedJson);
        Debug.Log(convertedJson.ToString());
    }

    public void CreateItemAndAdd()
    {
        var ID = GameObject.Find("IDInputText").GetComponent<Text>().text;
        var Name = GameObject.Find("NameInputText").GetComponent<Text>().text;
        var Description = GameObject.Find("DescriptionInputText").GetComponent<Text>().text;
        var ShelfID = GameObject.Find("ShelfIDInputText").GetComponent<Text>().text;
        var HoriIndex = GameObject.Find("HoriIndexInputText").GetComponent<Text>().text;

        if (ID != null & Name != null & ShelfID != null & HoriIndex != null)
        {
            Debug.Log("Creating new Item " + ID);
            Item newItem = new Item()
            {
                id = ID,
                name = Name,
                description = Description,
                shelfID = ShelfID,
                horiIndex = HoriIndex
            };
            Debug.Log("New Item created, ID: " + newItem.id);
            AddNewItem(newItem);
            Debug.Log("New Item added into database");
        }
    }

    public void DeleteItemClicked()
    {
        var ID = GameObject.Find("IDInputText").GetComponent<Text>().text;

        if (ID != null)
        {
            DeleteItem(ID);
        }
    }

    public void ReturnToSearch()
    {
        SceneManager.LoadScene("Search");
    }

}