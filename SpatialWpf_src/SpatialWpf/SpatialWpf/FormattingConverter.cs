using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SpatialWpf
{
    public class FormattingConverter
        : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {
                string strFormatString = parameter.ToString();

                if (!string.IsNullOrEmpty(strFormatString))
                    return string.Format(culture, strFormatString, value);
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.ComponentModel.TypeConverter objTypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(targetType);
            object objReturnValue = null;

            if (objTypeConverter.CanConvertFrom(value.GetType()))
                objReturnValue = objTypeConverter.ConvertFrom(value);

            return objReturnValue;
        }

        #endregion
    }
}
