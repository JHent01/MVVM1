using System;
using System.Windows.Markup;
using System.Windows.Data;
using System.Collections;
using System.Linq;

namespace MVVM
{
    public class ConvertEnum : MarkupExtension
    {
        public Type EnumType { get; set; }

        public ConvertEnum() { }

        public ConvertEnum(Type enumType)
        {
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EnumType == null || !EnumType.IsEnum)
                return null;

            return Enum.GetValues(EnumType);
        }
    }
}
