using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MyVirtualKeyboardControl.Models
{
    public class Helpers
    {
        public double CalculateMarginInRow(int row, Thickness margin, int startKey, UIElementCollection internalChildren)
        {
            double result = 0;

            for (int i = 0; i < row; i++)
            {
                double widthCoefficient = VirtualKeyboard.GetKeyMetadataProperty(internalChildren[startKey]).WidthCoefficient;

                result += (margin.Left + margin.Right) * (widthCoefficient);

                startKey++;
            }

            return result;
        }

        public double CalculateMarginInVirtualKeyboard(List<int> rowsWithKeys, int keyboardNumber, int rowsCount, 
            UIElementCollection internalChildren, Thickness margin)
        {
            double result = 0;
            int currentKey = 0;

            for (int i = 0; i < keyboardNumber * rowsCount; i++)
            {
                currentKey =+ rowsWithKeys[i];
            }

            for (int i = keyboardNumber * rowsCount; i < keyboardNumber * rowsCount + rowsCount; i++)
            {
                double marginInRow = CalculateMarginInRow(rowsWithKeys[i], margin, currentKey, internalChildren);

                if (marginInRow > result)
                {
                    result += marginInRow;
                }

                currentKey += rowsWithKeys[i];
            }

            return result;
        }

        public double CountKeysInRows(List<int> rows, int rowsCount, UIElementCollection internalChildren)
        {
            double result = 0;
            double keysInKeyboard = 0;
            int currentKey = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                double keysInRow = CountKeysInRow(rows[i], currentKey, internalChildren);

                if (keysInRow > keysInKeyboard)
                {
                    keysInKeyboard = keysInRow;
                }

                if ((i + 1) % rowsCount == 0 && i != 0)
                {
                    result += keysInKeyboard;
                    keysInKeyboard = 0;
                }

                currentKey += rows[i];
            }

            return result;
        }

        public double CountKeysInRow(int row, int startKey, UIElementCollection internalChildren)
        {
            double result = 0;
            int currentKey = startKey;

            for (int i = 0; i < row; i++)
            {
                result += VirtualKeyboard.GetKeyMetadataProperty(internalChildren[currentKey]).WidthCoefficient;

                currentKey++;
            }

            return result;
        }

        public double CountKeysInVirtualKeyboard(List<int> rows, int keyboardNumber, int rowsCount, UIElementCollection internalChildren)
        {
            double result = 0;
            int currentKey = 0;

            for (int i = 0; i < keyboardNumber * rowsCount; i++)
            {
                currentKey += rows[i];
            }

            for (int i = keyboardNumber * rowsCount; i < keyboardNumber * rowsCount + rowsCount - 1; i++)
            {
                double keysInRow = CountKeysInRow(rows[i], currentKey, internalChildren);

                if (keysInRow > result)
                {
                    result = keysInRow;
                }

                currentKey += rows[i];
            }

            return result;
        }
    }
}
