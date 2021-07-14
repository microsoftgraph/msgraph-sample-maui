// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// <DateTimeConverterSnippet>
using Microsoft.Graph;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace GraphTutorial.Models
{
    class GraphDateTimeTimeZoneConverter : IValueConverter
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
// </DateTimeConverterSnippet>
