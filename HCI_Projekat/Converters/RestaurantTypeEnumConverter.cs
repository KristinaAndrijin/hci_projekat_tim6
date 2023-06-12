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
    internal class RestaurantTypeEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RestaurantType restaurantType)
            {
                switch (restaurantType)
                {
                    case RestaurantType.FastFood:
                        return "Brza hrana";
                    case RestaurantType.Ethno:
                        return "Nacionalna kuhinja";
                    case RestaurantType.Modern:
                        return "Moderna kuhinja";
                }
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
