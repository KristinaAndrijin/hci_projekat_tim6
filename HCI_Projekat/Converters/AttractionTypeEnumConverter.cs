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
    internal class AttractionTypeEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AttractionType accomodationType)
            {
                switch (accomodationType)
                {
                    case AttractionType.Cultural:
                        return "Kulturna";
                    case AttractionType.Nature:
                        return "Prirodna";
                    case AttractionType.Entertainment:
                        return "Zabavna";
                    case AttractionType.Religious:
                        return "Verska";
                }
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if ((string)value == "Kulturna")
                {
                    return AttractionType.Cultural;
                }
                else if ((string)value == "Prirodna")
                {
                    return AttractionType.Nature;
                }
                else if ((string)value == "Zabavna")
                {
                    return AttractionType.Entertainment;
                }
                else if ((string)value == "Verska")
                {
                    return AttractionType.Religious;
                }
            }
            return null;
        }
    }
}
