using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSTickets.App.Helpers;

public static class EnumHelper
{
    public static List<T> GetValues<T>() where T : Enum =>
        Enum.GetValues(typeof(T)).Cast<T>().ToList();
}
