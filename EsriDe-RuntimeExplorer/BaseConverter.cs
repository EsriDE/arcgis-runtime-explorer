using System;
using System.Windows.Markup;

namespace EsriDe.RuntimeExplorer
{
	public class BaseConverter : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}