using System;
using System.Linq.Expressions;
using System.Reflection;
using FazAppFramework.EventSystem;

namespace FazAppFramework.UI.MVVM
{
    public abstract class ViewModel : IDisposable
    {
        private bool isInitialzied;

        public Action<bool> OnEnableDisable;

        private Action<PropertyInfo> onPropertyChange;

        public virtual void Initialize()
        {
            if(isInitialzied)
                return;

            this.SubscribeEventHandlers();
            isInitialzied = true;
        }

        public void AssingOnPropertChange(Action<PropertyInfo> onPropertyChangeHandler)
        {
            onPropertyChange += onPropertyChangeHandler;
        }

        protected void InvokeOnPropertyChange<T>(Expression<Func<T>> property)
        {
            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new Exception("Failed to invoke InvokeOnPropertyChange. Expression body is not valid");
            }

            onPropertyChange?.Invoke(memberExpression.Member as PropertyInfo);
        }

        public virtual void Refresh()
        {
        }

        public void Dispose()
        {
            this.UnsubscribeEventHandlers();
        }
    }
}
