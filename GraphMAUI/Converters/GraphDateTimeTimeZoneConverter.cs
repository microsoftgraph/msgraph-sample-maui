// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Globalization;
using Microsoft.Graph;

namespace GraphMAUI.Converters
{
    /// <summary>
    /// Convert a Graph DateTimeTimeZone value to a human-readable string
    /// </summary>
    public class GraphDateTimeTimeZoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeTimeZone date)
            {
                var parsedDateAs = DateTimeOffset.Parse(date.DateTime);
                // Return the local date time string
                return $"{parsedDateAs.LocalDateTime.ToShortDateString()} {parsedDateAs.LocalDateTime.ToShortTimeString()}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
