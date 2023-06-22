using amiliur.web.shared.Attributes.Datagrid.Enums;
using Radzen;

namespace amiliur.web.blazor.RadzenExtensions
{
    public static class TextAlignExtensions
    {
        // Convert from TextAlign to TextAlignment
        public static TextAlignment ConvertFromRadzen(this TextAlign? textAlign)
        {
            if (textAlign == null)
                return TextAlignment.Left;
            return (TextAlignment) textAlign;
        }

        // Convert from TextAlignment to TextAlign
        public static TextAlign ConvertToRadzen(this TextAlignment? textAlignment)
        {
            if (textAlignment == null)
                return TextAlign.Left;

            return (TextAlign) textAlignment;
        }
    }
}