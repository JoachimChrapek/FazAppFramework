using System;
using FazAppFramework.Attributes;

namespace FazAppFramework.UI.MVVM
{
    public abstract partial class View<T> : MainBehaviour where T : ViewModel
    {
        [DependencyInject] protected T viewModel;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            CheckViewModel();
            viewModel.Initialize();

            AssignPropertiesBindings();
            AssingTextBindings();
            AssignInputFieldsBindings();
            AssignButtonBindings();

            AssignEnableDisableBind();
        }

        protected virtual void OnEnable()
        {
            viewModel.Refresh();
        }

        private void CheckViewModel()
        {
            if(viewModel != null)
                return;
            
            throw new Exception($"ViewModel in {GetType().FullName} is null");
        }

        private void AssignEnableDisableBind()
        {
            viewModel.OnEnableDisable += OnEnableDisable;
        }

        protected virtual void OnEnableDisable(bool active)
        {
            gameObject.SetActive(active);
        }
        
    }
}
