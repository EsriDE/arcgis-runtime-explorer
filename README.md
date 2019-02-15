# arcgis-runtime-explorer
This is a tool from developers for developers. The main focus is to give ArcGIS Runtime developers a tool to visualize and evaluate mobile caches very quickly.

## Requirements
[Runtime WPF](https://developers.arcgis.com/net/latest/wpf/guide/system-requirements.htm)

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
* giving detail information about current view
  * coordinate and scale
  * extends of all layers
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

## References
https://community.esri.com/groups/geodev-germany/blog/2019/02/15/view-evaluate-and-modify-arcgis-runtime-caches-with-the-runtime-explorer

## Licensing

Copyright © 2014-2019 Esri.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

A copy of the license is available in the repository's [LICENSE](LICENSE) file.
