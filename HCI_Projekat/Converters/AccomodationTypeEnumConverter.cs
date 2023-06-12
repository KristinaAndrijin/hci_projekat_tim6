using HCI_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HCI_Projekat.Converters
{
    internal class AccomodationTypeEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AccomodationType accomodationType)
            {
                switch (accomodationType)
                {
                    case AccomodationType.Hotel:
                        return "Hotel";
                    case AccomodationType.Motel:
                        return "Motel";
                    case AccomodationType.Tent:
                        return "Kamp mesto";
                }
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if ((string)value == "Hotel")
                {
                    return AccomodationType.Hotel;
                }
                else if ((string)value == "Motel")
                {
                    return AccomodationType.Motel;
                }
                else if ((string)value == "Kamp mesto")
                {
                    return AccomodationType.Tent;
                }
            }
            return null;
        }
    }
}
