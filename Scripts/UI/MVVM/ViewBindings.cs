using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FazAppFramework.Reflection;
using TMPro;
using UnityEngine.UI;

namespace FazAppFramework.UI.MVVM
{
    public abstract partial class View<T> where T : ViewModel
    {
        private Dictionary<PropertyInfo, PropertyInfo> bindings;
        private Dictionary<PropertyInfo, FieldInfo> textBindings;

        private void AssignPropertiesBindings()
        {
            var propertiesToBind = this.GetPropertiesWithAttribute<View<T>, PropertyBindAttribute>();
            bindings = new Dictionary<PropertyInfo, PropertyInfo>(propertiesToBind.Length);
            foreach (var property in propertiesToBind)
            {
                if (TryToGetPropertyInfoFromViewModel<T>(property, out var viewModelProperty))
                {
                    bindings.Add(viewModelProperty, property);
                }
                else
                {
                    throw new Exception($"Invalid binding on property '{property.Name}'. \n" +
                                        $"There is no ViewModel property that match name and type on '{viewModel.GetType().Name}'.");
                }
            }

            if (bindings.Count > 0)
            {
                viewModel.AssingOnPropertChange(OnPropertyChange);
            }
        }

        private void AssingTextBindings()
        {
            var fieldsToBind = this.GetFieldsWithAttribute<View<T>, TextBindAttribute>();
            textBindings = new Dictionary<PropertyInfo, FieldInfo>(fieldsToBind.Length);
            foreach (var field in fieldsToBind)
            {
                if (TryToGetPropertyInfoFromViewModel<T>(field, out var viewModelProperty))
                {
                    textBindings.Add(viewModelProperty, field);
                }
                else
                {
                    throw new Exception($"Invalid binding on text field '{field.Name}'. \n" +
                                        $"There is no ViewModel property that match name and type on '{viewModel.GetType().Name}'.");
                }
            }

            if (textBindings.Count > 0)
            {
                viewModel.AssingOnPropertChange(OnPropertyChangeTextFields);
            }
        }

        private void AssignInputFieldsBindings()
        {
            var inputFieldsForBind = this.GetFieldsWithAttribute<View<T>, InputFieldBindAttribute>();
            foreach (var field in inputFieldsForBind)
            {
                var inputField = field.GetValue(this);
                if (inputField is InputField uiInputField)
                {
                    BindInputField(uiInputField, field);
                }
                else
                {
                    throw new Exception(
                        $"InputFieldBindAttribute can be used only on UnityEngine.UI.InputField fields.\n" +
                        $"Field '{field.Name}' in '{GetType().Name}'.");
                }
            }
        }

        private void BindInputField(InputField inputField, FieldInfo fieldInfo)
        {
            if (inputField == null)
            {
                throw new Exception($"InputField for bind is null. Field '{fieldInfo.Name}' in '{GetType().Name}'.");
            }

            var attribute = fieldInfo.GetCustomAttribute<InputFieldBindAttribute>();
            var viewModelStringPropertyName = attribute.ViewModelStringProperty;

            if (TryToGetPropertyInfoFromViewModel<T, string>(viewModelStringPropertyName, out var viewModelProperty))
            {
                if (!viewModelProperty.CanWrite)
                {
                    throw new Exception($"ViewModel property must have setter if you want to bind it with input field.\n" +
                                        $"Field '{fieldInfo.Name}' in '{GetType().Name}'.");
                }

                inputField.onValueChanged.AddListener(delegate
                {
                    viewModelProperty.SetValue(viewModel, inputField.text);
                });
            }
            else
            {
                throw new Exception(
                    $"There is no string property '{viewModelStringPropertyName}' in '{viewModel.GetType().Name}'.\n" +
                    $"InputField '{fieldInfo.Name}' in '{GetType().Name}'.");
            }
        }

        private void AssignButtonBindings()
        {
            var buttonsToBind = this.GetFieldsWithAttribute<View<T>, ButtonBindAttribute>();

            foreach (var buttonField in buttonsToBind)
            {
                var button = buttonField.GetValue(this) as Button;

                if (button == null)
                {
                    throw new Exception($"ButtonBindAttribute can be used only on UnityEngine.UI.Button fields.\n" +
                                        $"Field '{buttonField.Name}' in '{GetType().Name}'.");
                }

                var attribute = buttonField.GetCustomAttribute<ButtonBindAttribute>();
                var functionName = attribute.OnButtonClickFunctionName;
                if (TryToGetMethodInfoFromViewModel<T>(functionName, out var methodInfo))
                {
                    button.onClick.AddListener(() => methodInfo.Invoke(viewModel, null));
                }
                else
                {
                    throw new Exception($"Invalid binding on button '{buttonField.Name}'\n" +
                                        $"There is no ViewModel function in '{viewModel.GetType().Name} that match name {functionName}'");
                }
            }
        }

        
        private void OnPropertyChange(PropertyInfo viewModelProperty)
        {
            if (!bindings.ContainsKey(viewModelProperty))
            {
                return;
            }

            try
            {
                var viewProperty = bindings[viewModelProperty];
                var modelProperty = bindings.Keys.FirstOrDefault(b => b == viewModelProperty);

                if (modelProperty == null)
                {
                    throw new Exception($"There is no matching property info in '{this.GetType().Name}' " +
                                        $"bindings for property changed '{viewModelProperty.Name}'.");
                }

                if (viewProperty.CanWrite)
                {
                    viewProperty.SetValue(this, modelProperty.GetValue(viewModel));
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Cannot resolve runtime OnPropertyChanged in View({this.GetType().Name}) on property '{viewModelProperty.Name}'. {e.Message}");
            }
        }

        private void OnPropertyChangeTextFields(PropertyInfo viewModelProperty)
        {
            if (!textBindings.ContainsKey(viewModelProperty))
            {
                return;
            }

            try
            {
                var viewTextField = textBindings[viewModelProperty];
                var modelProperty = textBindings.Keys.FirstOrDefault(b => b == viewModelProperty);

                if (modelProperty == null)
                {
                    throw new Exception($"There is no matching property info in '{this.GetType().Name}' " +
                                        $"bindings for property changed '{viewModelProperty.Name}'.");
                }

                var textField = viewTextField.GetValue(this);
                if (textField is Text uiText)
                {
                    uiText.text = (string) modelProperty.GetValue(viewModel);
                }
                else if (textField is TextMeshProUGUI textMeshPro)
                {
                    textMeshPro.text = (string) modelProperty.GetValue(viewModel);
                }
                else
                {
                    throw new Exception($"Text binding attribute can be used only for UnityEngine.UI.Text or TMPro.TextMeshProUGUI. View({this.GetType().Name}) on property '{viewModelProperty.Name}");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Cannot resolve runtime OnPropertyChanged in View({this.GetType().Name}) on property '{viewModelProperty.Name}'. {e.Message}");
            }
        }
    }
}