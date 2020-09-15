using System.Reflection;

namespace FazAppFramework.UI.MVVM
{
    public abstract partial class View<T> where T : ViewModel
    {
        private bool TryToGetPropertyInfoFromViewModel<TViewModel>(
            PropertyInfo property,
            out PropertyInfo viewModelProperty) where TViewModel : ViewModel
        {
            viewModelProperty = typeof(TViewModel).GetProperty(property.Name,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (viewModelProperty == null)
                return false;

            return viewModelProperty.PropertyType == property.PropertyType;
        }

        private bool TryToGetPropertyInfoFromViewModel<TViewModel>(
            FieldInfo field,
            out PropertyInfo viewModelProperty) where TViewModel : ViewModel
        {
            viewModelProperty = typeof(TViewModel).GetProperty(field.Name,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (viewModelProperty == null)
                return false;

            return viewModelProperty.PropertyType == field.FieldType;
        }

        private bool TryToGetPropertyInfoFromViewModel<TViewModel, TViewModelProperty>(
            string propertyName,
            out PropertyInfo viewModelProperty) where TViewModel : ViewModel
        {
            viewModelProperty = typeof(TViewModel).GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (viewModelProperty == null)
                return false;

            return viewModelProperty.PropertyType == typeof(TViewModelProperty);
        }

        private bool TryToGetMethodInfoFromViewModel<TViewModel>(
            string name, 
            out MethodInfo methodInfo) where TViewModel : ViewModel
        {
            methodInfo = typeof(TViewModel).GetMethod(name,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return methodInfo != null;
        }
    }
}
