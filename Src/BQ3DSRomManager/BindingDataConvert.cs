using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BQ3DSRomManager
{
    public class BindingDataConvert: IValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    State state = (State)value;
        //    switch (state)
        //    {
        //        case State.Running:
        //            return true;
        //        case State.Stop:
        //            return false;
        //        case State.Unknow:
        //        default:
        //            break;
        //    }

        //    return null;
        //}

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == "" || value.ToString() == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
    }
}
