Basic functions -

Add an item
Edit an intem
Delete an item
Get All items.

Other features -

Pagination: 

This is implemented in get all items, default page size configured is 5 in appsettings.json file, page number is taken as input in API.
Items are sorted in alphabetial order based on item name.


Quantity management: 

Items have property called "AvailableQuantity", add call increments this value if item with existing name is provided, else adds a new item.
Similarly, delete call decrements this number when delete is executed.
