using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSTickets.App.Helpers;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var status = value?.ToString();

        return status switch
        {
            "Open" => Color.FromArgb("#FFB74D"),
            "InProgress" => Color.FromArgb("#64B5F6"),
            "WorkAround" => Color.FromArgb("#BA68C8"),
            "Resolved" => Color.FromArgb("#81C784"),
            "Closed" => Color.FromArgb("#90A4AE"),
            _ => Colors.LightGray
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}