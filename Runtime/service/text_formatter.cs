﻿using System;
using System.Text.RegularExpressions;
using Unity.UIWidgets.foundation;

namespace Unity.UIWidgets.service
{
    public abstract class TextInputFormatter
    {
        public delegate TextEditingValue TextInputFormatFunction(TextEditingValue oldValue,
            TextEditingValue newValue);
        
        public abstract TextEditingValue formatEditUpdate(TextEditingValue oldValue, TextEditingValue newValue);

        static TextInputFormatter withFunction(TextInputFormatFunction formatFunction)
        {
            return new _SimpleTextInputFormatter(formatFunction);
        }
    }
 
    internal class _SimpleTextInputFormatter : TextInputFormatter
    {
        public readonly TextInputFormatFunction formatFunction;

        internal _SimpleTextInputFormatter(TextInputFormatFunction formatFunction)
        {
            D.assert(formatFunction != null);
            this.formatFunction = formatFunction;
        }
            
        public override TextEditingValue formatEditUpdate(TextEditingValue oldValue, TextEditingValue newValue)
        {
            return formatFunction(oldValue, newValue);
        }
    }

    public class BlacklistingTextInputFormatter : TextInputFormatter
    {
        public readonly Regex blacklistedPattern;

        public readonly string replacementString;

        public static readonly BlacklistingTextInputFormatter singleLineFormatter
            = new BlacklistingTextInputFormatter(new Regex(@"\n"));
            
        public BlacklistingTextInputFormatter(Regex blacklistedPattern, string replacementString = "")
        {
            D.assert(blacklistedPattern != null);
            this.blacklistedPattern = blacklistedPattern;
            this.replacementString = replacementString;
        }


        public override TextEditingValue formatEditUpdate(TextEditingValue oldValue, TextEditingValue newValue)
        {
            return Util._selectionAwareTextManipulation(newValue,
                (substring) => blacklistedPattern.Replace(substring, replacementString));
        }
    }

    internal static class Util
    {
        internal static TextEditingValue _selectionAwareTextManipulation(TextEditingValue value,
            Func<string, string> substringManipulation)
        {
            int selectionStartIndex = value.selection.start;
            int selectionEndIndex = value.selection.end;
            string manipulatedText;
            TextSelection manipulatedSelection = null;
            if (selectionStartIndex < 0 || selectionEndIndex < 0)
            {
                manipulatedText = substringManipulation(value.text);
            }
            else
            {
                var beforeSelection = substringManipulation(
                    value.text.Substring(0, selectionStartIndex)
                );
                var inSelection = substringManipulation(
                    value.text.Substring(selectionStartIndex, selectionEndIndex - selectionStartIndex)
                );
                var afterSelection = substringManipulation(
                    value.text.Substring(selectionEndIndex)
                );
                manipulatedText = beforeSelection + inSelection + afterSelection;
                if (value.selection.baseOffset > value.selection.extentOffset)
                {
                    manipulatedSelection = value.selection.copyWith(
                        baseOffset: beforeSelection.Length + inSelection.Length,
                        extentOffset: beforeSelection.Length
                    );
                }
                else
                {
                    manipulatedSelection = value.selection.copyWith(
                        baseOffset: beforeSelection.Length,
                        extentOffset: beforeSelection.Length + inSelection.Length
                    );
                }
            }

            return new TextEditingValue(
                text: manipulatedText,
                selection: manipulatedSelection ?? TextSelection.collapsed(offset: -1),
                composing: manipulatedText == value.text ? value.composing : TextRange.empty
            );
        }
    }
}