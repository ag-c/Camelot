using Camelot.ViewModels.Services;

namespace Camelot.ViewModels.Implementations.Dialogs
{
    public abstract class ParameterizedDialogViewModelBase<TResult, TParameter> : DialogViewModelBase<TResult>
        where TParameter : NavigationParameterBase
    {
        public abstract void Activate(TParameter parameter);
    }
    
    public abstract class ParameterizedDialogViewModelBase<TParameter> : ParameterizedDialogViewModelBase<object, TParameter>
        where TParameter : NavigationParameterBase
    {
        
    }
}