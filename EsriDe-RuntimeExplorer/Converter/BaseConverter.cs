using System;
using System.Windows.Markup;

namespace EsriDe.RuntimeExplorer.Converter
{
	public class BaseConverter : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}