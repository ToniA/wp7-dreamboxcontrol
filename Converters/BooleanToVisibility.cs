// <copyright file="BooleanToVisibility.cs" company="$registerdorganization$">
// Copyright (c) 2012 . All Right Reserved
// </copyright>
// <author>Matthias</author>
// <email></email>
// <date>2012-07-23</date>
// <summary>A value converter for WPF and Silverlight data binding</summary>

namespace DreamboxControl.Converters
{
    using System;
    using System.Windows.Data;
    using System.Windows;
    using System.Globalization;

    public class BooleanToVisibility : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool v = (bool)value;
                if (!v)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
