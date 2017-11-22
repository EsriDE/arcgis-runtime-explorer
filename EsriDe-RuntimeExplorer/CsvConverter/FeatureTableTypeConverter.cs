using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Esri.ArcGISRuntime.Data;

namespace EsriDe.RuntimeExplorer.CsvConverter
{
    class FeatureTableTypeConverter : ITypeConverter
    {
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            var featureTable = (FeatureTable) value;
            return featureTable.TableName;
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }
    }
}
