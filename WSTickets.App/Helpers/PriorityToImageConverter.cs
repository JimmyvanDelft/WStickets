using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSTickets.App.Helpers;

public class PriorityToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var priority = value?.ToString()?.ToLowerInvariant();

        return priority switch
        {
            "low" => "low.png",
            "medium" => "medium.png",
            "high" => "high.png",
            "urgent" => "highest.png",
            _ => "medium.png"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}