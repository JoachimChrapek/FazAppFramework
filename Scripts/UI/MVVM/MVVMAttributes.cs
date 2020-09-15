using System;

namespace FazAppFramework.UI.MVVM
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyBindAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Field)]
    public class ButtonBindAttribute : Attribute
    {
        public string OnButtonClickFunctionName { get; }

        public ButtonBindAttribute(string onButtonClickFunctionName)
        {
            OnButtonClickFunctionName = onButtonClickFunctionName;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class TextBindAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class InputFieldBindAttribute : Attribute
    {
        public string ViewModelStringProperty { get; }

        public InputFieldBindAttribute(string viewModelStringProperty)
        {
            ViewModelStringProperty = viewModelStringProperty;
        }
    }
}
