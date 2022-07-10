namespace Gekka.WPF.Sample.AskDialog
{
    using System;
    using System.Windows.Input;
    #region

    public class ModelBase : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            var pc = PropertyChanged;
            if (pc != null)
            {
                pc(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }

        protected bool SetValue<T>(T value, ref T newvalue, [System.Runtime.CompilerServices.CallerMemberNameAttribute] string name = "")
        {
            if (object.Equals(value, newvalue))
            {
                return false;
            }
            newvalue = value;
            OnPropertyChanged(name);
            return true;
        }
    }

    #endregion

    #region

    class DelegateCommand : ICommand
    {
        public DelegateCommand(Action<object> exec, Func<object, bool> ask = null)
        {
            _exec = exec;
            _ask = ask ?? ((o) => true);
        }

        private Action<object> _exec;
        private Func<object, bool> _ask;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _ask(parameter);
        public void Execute(object parameter) => _exec(parameter);
    }

    class UIDelegateCommand : DelegateCommand, IUICommand
    {
        public UIDelegateCommand(Action<object> exec, Func<object, bool> ask = null, string text = null, object image = null) : base(exec, ask)
        {
            Image = image;
            Text = text;
        }

        public string Text { get; set; }
        public object Image { get; set; }
    }

    class UIAskCommand : UIDelegateCommand, IUIAskCommand
    {
        public UIAskCommand(Action<object> exec, Func<object, bool> ask = null, string text = null, object image = null) : base(exec, ask, text, image)
        {
        }

        public AskCommandType AskCommandType { get; set; }
    }

    public interface IUICommand : ICommand
    {
        object Image { get; }
        string Text { get; }
    }

    public interface IUIAskCommand : IUICommand
    {
        AskCommandType AskCommandType { get; }
    }

    #endregion
}
