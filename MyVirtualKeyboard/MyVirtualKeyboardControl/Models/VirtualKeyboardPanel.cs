using MyVirtualKeyboardControl.Enums;
using MyVirtualKeyboardControl.WinAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MyVirtualKeyboardControl.Models
{
    public class VirtualKeyboardPanel : Panel
    {
        private const UInt32 KLF_SETFORPROCESS = 0x00000100;
        private const double SPACE_BETWEEN_KEYBOARDS = 20;
        private const int ROWS_COUNT = 4;
        private double keyWidth = 0;

        Helpers helpers = new Helpers();
        List<int> rows;

        static VirtualKeyboardPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VirtualKeyboardPanel), new FrameworkPropertyMetadata(typeof(VirtualKeyboardPanel)));
        }

        public VirtualKeyboardPanel()
        {
            this.Focusable = false;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            rows = GetKeysList();

            Size size = availableSize;

            double marginInRows = CalculateMargin(rows) + SPACE_BETWEEN_KEYBOARDS * (rows.Count / ROWS_COUNT - 1);

            keyWidth = (size.Width - marginInRows) / helpers.CountKeysInRows(rows, ROWS_COUNT, InternalChildren);

            SetMinWidth(marginInRows, size);
            SetNewWidth();
            SetNewHeight(size);

            foreach (UIElement child in InternalChildren)
            {
                SetButtonContent(child);
                child.Measure(size);
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double currentHeightShift = 0.0;
            double currentWidthShift = 0.0;
            double previousKeyboardMaxWidth = 0;

            int currentKeyboard = 0;
            int currentKey = 0;
            int currentKeyInRow = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < rows[i]; j++)
                {
                    double shiftFromMaxWidth = 0;

                    if (helpers.CountKeysInVirtualKeyboard(rows, currentKeyboard, ROWS_COUNT, InternalChildren)
                        - helpers.CountKeysInRow(rows[i], currentKeyInRow, InternalChildren) != 0)
                    {
                        Thickness margin = (Thickness)InternalChildren[currentKey].GetValue(MarginProperty);
                        shiftFromMaxWidth = (keyWidth + margin.Left + margin.Right)
                            * (helpers.CountKeysInVirtualKeyboard(rows, currentKeyboard, ROWS_COUNT, InternalChildren)
                            - helpers.CountKeysInRow(rows[i], currentKey, InternalChildren)) / 2;
                    }

                    InternalChildren[currentKey].Arrange(new Rect(new Point(currentWidthShift + shiftFromMaxWidth, currentHeightShift), InternalChildren[currentKey].DesiredSize));

                    currentWidthShift += InternalChildren[currentKey].DesiredSize.Width;

                    if (i != rows.Count || i != rows[i] - 1)
                    {
                        currentKey++;
                    }
                }

                currentWidthShift = previousKeyboardMaxWidth;
                currentHeightShift += InternalChildren[currentKey - 1].DesiredSize.Height;

                if ((i + 1) % ROWS_COUNT == 0 && (i + 1) != 0)
                {
                    double currentKeyboardMaxWidth = keyWidth * helpers.CountKeysInVirtualKeyboard(rows, currentKeyboard, ROWS_COUNT, InternalChildren)
                        + helpers.CalculateMarginInVirtualKeyboard(rows, currentKeyboard, ROWS_COUNT, InternalChildren, (Thickness)InternalChildren[currentKey - 1].GetValue(MarginProperty));

                    previousKeyboardMaxWidth += currentKeyboardMaxWidth + SPACE_BETWEEN_KEYBOARDS;
                    currentHeightShift = 0;
                    currentWidthShift = previousKeyboardMaxWidth;
                    currentKeyboard++;
                }

                currentKeyInRow += rows[i];
            }

            return base.ArrangeOverride(finalSize);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            if (visualAdded != null)
            {
                if (visualAdded is ButtonBase button)
                {
                    ushort virtualKeyCode = VirtualKeyboard.GetKeyMetadataProperty(button).VirtualKeyCode;

                    if (virtualKeyCode == (ushort)VirtualKeyCode.SHIFT)
                    {
                        button.Click += ShiftKeyPressed;
                    }
                    else
                    {
                        button.Click += VirtualKeyPressed;
                    }
                }
                else if (visualAdded is ComboBox comboBox)
                {
                    comboBox.SelectionChanged += ChangeLayout;
                }
            }

            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        public void Send(VirtualKeyCode virtualKeyCode, KEYEVENTF flag)
        {
            INPUT[] Inputs = new INPUT[1];
            INPUT Input = new INPUT();
            Input.type = 1;
            Input.inputUnion.ki.wVK = virtualKeyCode;
            Input.inputUnion.ki.dwFlags = flag;
            Inputs[0] = Input;
            API.SendInput(1, Inputs, INPUT.Size);
        }

        private List<int> GetKeysList()
        {
            List<int> keys = new List<int>();

            int currentRow = 1;
            int keysInRow = 0;

            foreach (UIElement child in InternalChildren)
            {
                var row = VirtualKeyboard.GetKeyMetadataProperty(child).Row;

                if (row == currentRow + 1)
                {
                    keys.Add(keysInRow);
                    keysInRow = 0;
                    currentRow++;
                }

                keysInRow++;
            }

            if (keysInRow != 0)
            {
                keys.Add(keysInRow);
            }

            return keys;
        }

        private double CalculateMargin(List<int> keys)
        {
            double result = 0;
            double keyboardMargin = 0;
            int currentKey = 0;

            for (int i = 0; i < keys.Count; i++)
            {
                double marginInRow = helpers.CalculateMarginInRow(keys[i], 
                    (Thickness)InternalChildren[currentKey].GetValue(MarginProperty), 
                    currentKey, InternalChildren);

                if (marginInRow > keyboardMargin)
                {
                    keyboardMargin = marginInRow;
                }

                if ((i + 1) % ROWS_COUNT == 0 && i != 0)
                {
                    result += keyboardMargin;
                    keyboardMargin = 0;
                }

                currentKey += keys[i];
            }

            return result;
        }

        private Size SetMinWidth(double marginInRows, Size availableSize)
        {
            if (Application.Current.MainWindow.Width < 30 * helpers.CountKeysInRows(rows, ROWS_COUNT, InternalChildren) + marginInRows)
            {
                keyWidth = 30;

                double minSize = keyWidth * helpers.CountKeysInRows(rows, ROWS_COUNT, InternalChildren);
                availableSize.Width = minSize;
                Application.Current.MainWindow.MinWidth = minSize;
            }

            return availableSize;
        }

        private void SetNewWidth()
        {
            foreach (UIElement child in InternalChildren)
            {
                var widthCoefficient = VirtualKeyboard.GetKeyMetadataProperty(child).WidthCoefficient;

                child.SetValue(WidthProperty, keyWidth * widthCoefficient + (((Thickness)child.GetValue(MarginProperty)).Right 
                    + ((Thickness)child.GetValue(MarginProperty)).Left) * (widthCoefficient - 1));
            }
        }

        private void SetNewHeight(Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                //if (!double.IsInfinity(availableSize.Height) && availableSize.Height > 61.5 * ROWS_COUNT)
                //{
                    child.SetValue(HeightProperty, (availableSize.Height - (((Thickness)child.GetValue(MarginProperty)).Top
                        + ((Thickness)child.GetValue(MarginProperty)).Bottom) * ROWS_COUNT) / ROWS_COUNT);
                //}
                //else
                //{
                //    double height = (250 - (((Thickness)child.GetValue(MarginProperty)).Top 
                //        + ((Thickness)child.GetValue(MarginProperty)).Bottom) * ROWS_COUNT) / ROWS_COUNT;

                //    child.SetValue(HeightProperty, height);

                //    Application.Current.MainWindow.MinHeight = SystemParameters.WindowCaptionHeight + height;
                //}
            }
        }

        private void SetButtonContent(UIElement child)
        {
            if (child is ButtonBase button)
            {
                ushort virtualKeyCode = VirtualKeyboard.GetKeyMetadataProperty(button).VirtualKeyCode;

                string content = "";

                content = virtualKeyCode switch
                {
                    (ushort)VirtualKeyCode.TAB => "TAB",
                    (ushort)VirtualKeyCode.SHIFT => "SHIFT",
                    (ushort)VirtualKeyCode.BACK => "BACKSPACE",
                    (ushort)VirtualKeyCode.RETURN => "ENTER",
                    (ushort)VirtualKeyCode.LEFT => "<",
                    (ushort)VirtualKeyCode.RIGHT => ">",
                    _ => null,
                };

                if (content == null)
                {
                    if (virtualKeyCode == 0x00)
                    {
                        button.Content = button.Content.ToString();
                    }
                    else
                    {
                        button.Content = GetCharsFromKeys((VirtualKeyCode)virtualKeyCode, VirtualKeyboard.ShiftIsActive, false);
                    }
                }
                else
                { 
                    button.Content = content;
                }
            }
        }

        private string GetCharsFromKeys(VirtualKeyCode key, bool shifted, bool altGr)
        {
            var buffer = new StringBuilder(256);
            var keyboardState = new byte[256];

            if (shifted)
            {
                keyboardState[(int)VirtualKeyCode.SHIFT] = 0xff;
            }

            if (altGr)
            {
                keyboardState[(int)VirtualKeyCode.CONTROL] = 0xff;
                keyboardState[(int)VirtualKeyCode.MENU] = 0xff;
            }

            API.ToUnicode((uint)key, 0, keyboardState, buffer, 256, 0);

            return buffer.ToString();
        }

        private void VirtualKeyPressed(object sender, RoutedEventArgs e)
        {
            var button = (ButtonBase)sender;
            ushort virtualKeyCode = VirtualKeyboard.GetKeyMetadataProperty(button).VirtualKeyCode;

            Send((VirtualKeyCode)virtualKeyCode, KEYEVENTF.KEYDOWN);
        }

        private void ShiftKeyPressed(object sender, RoutedEventArgs e)
        {
            var button = (ButtonBase)sender;
            ushort virtualKeyCode = VirtualKeyboard.GetKeyMetadataProperty(button).VirtualKeyCode;

            KEYEVENTF flag = KEYEVENTF.KEYDOWN;

            if (VirtualKeyboard.ShiftIsActive)
            {
                flag = KEYEVENTF.KEYUP;
                VirtualKeyboard.ShiftIsActive = false;
            }
            else
            {
                VirtualKeyboard.ShiftIsActive = true;
            }

            Send((VirtualKeyCode)virtualKeyCode, flag);

            int currentKey = 0;

            for (int i = 0; i < ROWS_COUNT; i++)
            {
                for (int j = 0; j < rows[i]; j++)
                {
                    SetButtonContent(InternalChildren[currentKey]);
                    currentKey++;
                }
            }
        }

        private void ChangeLayout(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            CultureInfo layoutInfo = new CultureInfo((UInt16)((ComboBoxItem)comboBox.SelectedItem).Tag, false);

            API.ActivateKeyboardLayout((IntPtr)layoutInfo.KeyboardLayoutId, KLF_SETFORPROCESS);

            foreach (UIElement child in InternalChildren)
            {
                SetButtonContent(child);
            }
        }
    }
}
