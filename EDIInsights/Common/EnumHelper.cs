using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BizTalkFusion.Solutions.Integration.Common
{
    public class EnumHelper
    {
        public static string GetEnumDescription(System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}