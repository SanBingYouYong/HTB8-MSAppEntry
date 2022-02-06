# HTB8-MSAppEntry - "M&S Store Navigator"

This is our team's entry for HTB8, M&S App Challenge. 

Team: 

https://github.com/LawrenceZhu22

https://github.com/ettolrach

https://github.com/GuifuLiu

Me :)


## Quick Introduction

[Detailed introduction document](https://devpost.com/software/new-project-1)

1. Mockup of actual app interfaces in the start, GUI implemented and connected with database after that. 
2. Locate the shelf for customers, indicating horizontal locations as well. 
3. Help update the product-shelf info for staff, support adding and deleting items. 

So we hide the entrance of our functionality in the side bar, the "Store Finder" part, where the user can either enter a location or use the current location to search for nearby M&S shops. We extends the original interface that when user clicks on it, it leads to another page, we call it search (and maybe navigate). 

So there is a search bar, input any word it will find you relevant product with its name containing your input. It's simple but working. Inputing nothing and pressing the button will simply give you a list of all items, as in empty string is in any string. We use a built-in unity scroll bar to showcase all results. Below the search part there is the map. The map is quite a simple representation of shelves and passways and is hard-coded for now, but we did consider something like a map editor for staff to use to update the shelves in the store. The shelf ID is displayed above each shelf. Once the user clicks on one of the search results, details are displayed and the precise location - which shelf it is on and where in the shelf exactly - is highlighted. This is one of the main purposes. 

Any human being must have noticed a staff login portal in the upper right corner. A staff may use one's staff number or ID to sign in (we have abstracted it away for now), and to update on any change of products on shelfs - add items to shelfs, remove items etc. (we say etc but we actually only implemented these two). 

## Unity Project Setup

Unity 3.25f1; 

Three Scenes: Main(mockup), Search, StoreLogin

Main: Add Overlay.cs as a component/script to an empty game object, preferably rename the object to "Overlay"; 

Search: Add Search.cs as a component/script to an empty game object, rename the object to "SearchOverlay"; 

StoreLogin: Add StaffInterface.cs as a component/script to an empty game object, rename the object to "StaffOverlay"

Other placement of buttons and text and stuff, I'm too lazy to state everything. Maybe I will, but now I sleep. 
