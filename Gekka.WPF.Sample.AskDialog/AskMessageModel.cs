namespace Gekka.WPF.Sample.AskDialog
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    #region  AskMessageModel

    public abstract class AskMessageModelBase : ModelBase
    {
        public AskMessageModelBase(string message, IEnumerable<IUIAskCommand> commands)
        {
            Message = message;
            Commands = commands;
        }

        /// <summary>問い合わせに表示させるメッセージ</summary>
        public string Message { get; }

        /// <summary>問い合わせの結果を受け取るICommandの一覧</summary>
        public IEnumerable<IUIAskCommand> Commands { get; protected set; }
    }

    public class AskMessageModel : AskMessageModelBase
    {
        public static AskMessageModel CreateOKCancel(string message, Action<AskMessageModel> ok, Action<AskMessageModel> cancel, string okCaption = "OK", string cancelCaption = "Cancel")
        {
            List<IUIAskCommand> commands = new List<IUIAskCommand>();

            var ret = new AskMessageModel(message, commands) { AskType = AskType.OKCancel };

            commands.Add(new UIAskCommand((o) => ok(ret), null, okCaption) { AskCommandType = AskCommandType.OK });
            commands.Add(new UIAskCommand((o) => cancel(ret), null, cancelCaption) { AskCommandType= AskCommandType.Cancel });

            return ret;
        }

        public static AskMessageModel CreateYesNo(string message, Action<AskMessageModel> yes, Action<AskMessageModel> no, string yesCaption = "Yes", string noCaption = "No")
        {
            List<IUIAskCommand> commands = new List<IUIAskCommand>();

            var ret = new AskMessageModel(message, commands) { AskType = AskType.YesNo };

            commands.Add(new UIAskCommand((o) => yes(ret), null, yesCaption) { AskCommandType = AskCommandType.Yes });
            commands.Add(new UIAskCommand((o) => no(ret), null, noCaption) { AskCommandType = AskCommandType.No});

            return ret;
        }



        public AskMessageModel(string message, IEnumerable<IUIAskCommand> commands) : base(message, commands)
        {
        }

        /// <summary>ダイアログに表示させる画像</summary>
        public object Image { get; set; }

        /// <summary>ダイアログの選択肢の組み合わせ</summary>
        public AskType AskType { get; set; } = AskType.Custom;
    }

    /// <summary>ダイアログに表示させる選択肢に対応する結果</summary>
    [Flags]
    public enum AskCommandType
    {
        None,
        OK = 1,
        Yes = 2,
        No = 4,
        Cancel = 8,

        //Help = 256,
    }

    /// <summary>ダイアログの選択肢の組み合わせ</summary>
    public enum AskType
    {
        Custom = 0,

        OK = AskCommandType.OK,
        OKCancel = (AskCommandType.OK | AskCommandType.Cancel),
        YesNo = (AskCommandType.Yes | AskCommandType.No),
        YesNoCancel = (AskCommandType.Yes | AskCommandType.No | AskCommandType.Cancel),
    }

    #endregion



}
