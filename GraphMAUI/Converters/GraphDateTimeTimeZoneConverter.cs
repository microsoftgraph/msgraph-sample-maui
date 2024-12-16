﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Globalization;
using Microsoft.Graph.Models;

namespace GraphMAUI.Converters
{
    /// <summary>
    /// Convert a Graph DateTimeTimeZone value to a human-readable string.
    /// </summary>
    public class GraphDateTimeTimeZoneConverter : IValueConverter
    {
        /// <summary>
        /// Convert a Graph DateTimeTimeZone value to a human-readable string.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeTimeZone"/> to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The date and time in human-readable form.</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTimeTimeZone date && date.DateTime != null)
            {
                var parsedDateAs = DateTimeOffset.Parse(date.DateTime);

                // Return the local date time string
                return $"{parsedDateAs.LocalDateTime.ToShortDateString()} {parsedDateAs.LocalDateTime.ToShortTimeString()}";
            }

            return string.Empty;
        }

        /// <inheritdoc/>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
