using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Notes.Converters;

public class BoolToTextDecorationsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Check if the value is a boolean and return a TextDecorations value
        if (value is bool isStrikethrough && isStrikethrough)
        {
            return TextDecorations.Strikethrough;
        }
        return TextDecorations.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Convert back to a boolean if the TextDecorations is Strikethrough
        return value is TextDecorations decorations && decorations == TextDecorations.Strikethrough;
    }
}

