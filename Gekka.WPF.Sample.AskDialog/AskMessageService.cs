namespace Gekka.WPF.Sample.AskDialog
{
    using System;
    using System.Linq;
    using System.Windows;

    #region AskMessageService

    /// <summary>問い合わせのプロパティを受け取るための添付プロパティ</summary>
    public static class AskMessageService
    {

        public static AskMessageModelBase GetAskMessage(DependencyObject obj)
        {
            return (AskMessageModelBase)obj.GetValue(AskMessageProperty);
        }

        public static void SetAskMessage(DependencyObject obj, AskMessageModelBase value)
        {
            obj.SetValue(AskMessageProperty, value);
        }

        public static readonly DependencyProperty AskMessageProperty =
            DependencyProperty.RegisterAttached
            ("AskMessage"
            , typeof(AskMessageModelBase)
            , typeof(AskMessageService)
            , new UIPropertyMetadata
                (default(AskMessageModelBase)
                 , new PropertyChangedCallback(OnAskMessagePropertyChanged)));

        private static void OnAskMessagePropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            DependencyObject target = dpo as DependencyObject;
            if (target != null)
            {
                AskMessageModelBase newValue = (AskMessageModelBase)e.NewValue;
                AskMessageModelBase oldValue = (AskMessageModelBase)e.OldValue;

                if (newValue is AskMessageModel ask)
                {
                   // 問い合わせを標準のモーダルダイアログで

                   MessageBoxResult ret = MessageBoxResult.Cancel;

                    switch (ask.AskType)
                    {
                    case AskType.Custom:

                    case AskType.OK:
                        ret = MessageBox.Show(ask.Message, "", MessageBoxButton.OK);
                        break;
                    case AskType.OKCancel:
                        ret = MessageBox.Show(ask.Message, "", MessageBoxButton.OKCancel);
                        break;
                    case AskType.YesNo:
                        ret = MessageBox.Show(ask.Message, "", MessageBoxButton.YesNo);
                        break;
                    case AskType.YesNoCancel:
                        ret = MessageBox.Show(ask.Message, "", MessageBoxButton.YesNoCancel);
                        break;
                    default:
                        break;
                    }

                    AskCommandType at = AskCommandType.None;
                    switch (ret)
                    {
                    case MessageBoxResult.None:
                        at = AskCommandType.None;
                        break;
                    case MessageBoxResult.OK:
                        at = AskCommandType.OK;
                        break;
                    case MessageBoxResult.Cancel:
                        at = AskCommandType.Cancel;
                        break;
                    case MessageBoxResult.Yes:
                        at = AskCommandType.Yes;
                        break;
                    case MessageBoxResult.No:
                        at = AskCommandType.No;
                        break;
                    default:
                        break;
                    }


                    object parameter = GetAskMessageCommandParameter(dpo);

                    foreach (var command in ask.Commands.Where(_ => _.AskCommandType == at))
                    {
                        if (command.CanExecute(parameter))
                        {
                            command.Execute(parameter);
                        }
                    }
                }


            }
        }




        public static object GetAskMessageCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(AskMessageCommandParameterProperty);
        }

        public static void SetAskMessageCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(AskMessageCommandParameterProperty, value);
        }

        // Using a DependencyProperty as the backing store for AskMessageCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AskMessageCommandParameterProperty =
            DependencyProperty.RegisterAttached
                ("AskMessageCommandParameter", typeof(object), typeof(AskMessageService), new PropertyMetadata(null));


    }

    #endregion

}
