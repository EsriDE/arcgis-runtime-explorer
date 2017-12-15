# arcgis-runtime-explorer
This is a tool from developers for developers. The main focus is to give ArcGIS Runtime developers a tool to visualize and evaluate mobile caches very quickly.

## How to start
The application can currently handle two ArcGIS Runtime data formats
* Mobile Geodatabases (*.geodatabase)
* Mobile Map Packages (*.mmpk)

And there are two ways to read these file formats
* Start the application and load a file
* Open a cache file with the application

#### Start the application and load a file
![Menu - Open Folder](https://github.com/EsriDE/arcgis-runtime-explorer/blob/master/docs/mainmenu_openfile.png)

Select a cache you want to load into the application and watch the result

#### Open a cache file with the application

In Windows Explorer, right click on the file you want to load and select _Open_ in the context menu.
Choose the _ArcGIS Runtime Explorer_ as application to load this file. 
Select the option _Always open with this application_ if you want to open files by double clicking in future.
Clicking _OK_ opens the _ArcGIS Runtime Explorer_ and directly loads your cache.

## Have a view
Here it is:
![Application - Cache loaded](https://github.com/EsriDE/arcgis-runtime-explorer/blob/master/docs/app_mmpk_loaded.png)

## Features
* Shows MMPK and mobile geodatabase files
* underlay a basemap
* shows coordinate and scale information of current view
* lists all layers
* shows layer symbology
* search for places with the ArcGIS world geocoding service or (if included in the MMPK file) with the delivered locator
* allows deep inspection (including changes) of 
  * MMPK
  * Geodatabase
  * Map
  * Layer
  * Background grid
* lists detail information about all layers
  * ability to export these details to a CSV file
