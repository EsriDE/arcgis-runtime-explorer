﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using GalaSoft.MvvmLight;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class MainDataViewModel : ViewModelBase
    {
        private string _filePath;

        public string FilePath
        {
            get => _filePath;
            set => Set(ref _filePath, value);
        }

        public ObservableCollection<MapViewModel> MapViews { get; } = new ObservableCollection<MapViewModel>();

        private MapViewModel _selectedMapView;

        public MapViewModel SelectedMapView
        {
            get => _selectedMapView;
            set => Set(ref _selectedMapView, value);
        }

        private MobileMapPackage _mmpk;

        public MobileMapPackage Mmpk
        {
            get => _mmpk;
            set => Set(ref _mmpk, value);
        }

        private Geodatabase _geodatabase;
        public Geodatabase Geodatabase
        {
            get => _geodatabase;
            set => Set(ref _geodatabase, value);
        }

        private LocatorTask _locatorTask;

        public LocatorTask LocatorTask
        {
            get => _locatorTask;
            set => Set(ref _locatorTask, value);
        }

        private string _locatorSearchText;

        public string LocatorSearchText
        {
            get => _locatorSearchText;
            set => Set(ref _locatorSearchText, value);
        }

        public ObservableCollection<GeocodeResult> GeocodeResults { get; } = new ObservableCollection<GeocodeResult>();

        private GeocodeResult _selectedGeocodeResult;

        public GeocodeResult SelectedGeocodeResult
        {
            get => _selectedGeocodeResult;
            set => Set(ref _selectedGeocodeResult, value);
        }

        public MainDataViewModel()
        {
            PropertyChanged += async (sender, args) =>
            {
                try
                {
                    if (args.PropertyName == nameof(FilePath))
                    {
                        MapViews.Clear();
                        if (FilePath.EndsWith(".geodatabase"))
                        {
                            await OpenGeodatabaseAsync();
                        }
                        else if (FilePath.EndsWith(".mmpk") || Directory.Exists(FilePath))
                        {
                            await OpenMmpkAsync();
                        }
                    }
                    if (args.PropertyName == nameof(LocatorSearchText) && LocatorTask != null)
                    {
                        if (LocatorTask.LoadStatus != LoadStatus.Loaded)
                        {
                            await LocatorTask.LoadAsync();
                        }
                        var geocodeResults = await LocatorTask.GeocodeAsync(LocatorSearchText);
                        GeocodeResults.Clear();
                        foreach (var geocodeResult in geocodeResults.Take(10))
                        {
                            GeocodeResults.Add(geocodeResult);
                        }
                    }
                    if (args.PropertyName == nameof(SelectedMapView) && SelectedMapView != null)
                    {
                        SelectedMapView.IdentifyModeEnabled = true;
                    }
                    if (args.PropertyName == nameof(SelectedGeocodeResult))
                    {
                        SelectedMapView.SelectedGeocodeResult = SelectedGeocodeResult;
                        LocatorSearchText = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            };
            var geocodeServiceUrl = @"http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer";
            LocatorTask = new LocatorTask(new Uri(geocodeServiceUrl));
        }

        private async Task OpenGeodatabaseAsync()
        {
            Geodatabase = await Geodatabase.OpenAsync(FilePath);
            var map = new Map(Basemap.CreateStreetsVector());
            foreach (var table in Geodatabase.GeodatabaseFeatureTables.Reverse())
            {
                var layer = new FeatureLayer(table);
                map.OperationalLayers.Add(layer);
            }
            var mapViewModel = new MapViewModel { Map = map };
            MapViews.Add(mapViewModel);
            SelectedMapView = mapViewModel;
        }

        private async Task OpenMmpkAsync()
        {
            try
            {
                Mmpk = await MobileMapPackage.OpenAsync(FilePath);
            }
            catch (Exception e)
            {
                //packed MMPK files that contains raster data could not be loaded, they must be unpacked
                if (e.Message.Contains("Mobile map package contains raster data that requires the mobile map package to be unpacked in a directory before use"))
                {
                    var message = $"{e.Message}{Environment.NewLine}{Environment.NewLine}{Properties.Resources.QuestionUnpackMMPKToFolder}";
                    var result = MessageBox.Show(message, Properties.Resources.MessageBoxDecision, MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        var folder = FilePath.Replace(Path.GetExtension(FilePath), "");
                        if (!Directory.Exists(folder))
                        {
                            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read))
                            {
                                zip.ExtractToDirectory(folder);
                            }
                        }
                        Mmpk = await MobileMapPackage.OpenAsync(folder);
                    }
                }
            }

            if (Mmpk == null || string.IsNullOrEmpty(Mmpk.Path))
            {
                MessageBox.Show(Properties.Resources.ErrorMmpkCouldNotBeLoaded, Properties.Resources.MessageBoxError,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var map in Mmpk.Maps)
            {
                await map.LoadAsync();
                MapViews.Add(new MapViewModel {Map = map});
            }
            if (Mmpk.LocatorTask != null)
            {
                LocatorTask = Mmpk.LocatorTask;
            }
        }
    }
}