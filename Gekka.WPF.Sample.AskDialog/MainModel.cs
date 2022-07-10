namespace Gekka.WPF.Sample.AskDialog
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class MainModel : ModelBase
    {

        public string Value
        {
            get { return _Value; }
            private set { if (_Value != value) { _Value = value; OnPropertyChanged("Value"); } }
        }
        private string _Value;

        public MainModel()
        {
            TestCommand1 = new DelegateCommand((o) => Method_CallFromOuter_1(o));
            TestCommand2 = new DelegateCommand((o) => Method_CallFromOuter_2(o));
        }

        public ICommand TestCommand1 { get; }
        public ICommand TestCommand2 { get; }

        #region  てすと１

        public void Method_CallFromOuter_1(object arg)
        {
            //外部からコマンドで要求されたら、問い合わせのためのプロパティを設定する
            this.AskMessage1 = AskMessageModel.CreateOKCancel
                ($"確認 {arg} ?"
                , ok: (o) =>
                    {//問い合わせの結果がOK
                        this.Value = arg?.ToString();
                        if (this.AskMessage1 == o) { this.AskMessage1 = null; }; //不要になった問い合わせを削除
                    }
                , cancel: (o) =>
                    {//問い合わせの結果がCancel
                        if (this.AskMessage1 == o) { this.AskMessage1 = null; }; //不要になった問い合わせを削除
                    });
        }

        /// <summary>View側に問い合わせを表示させで表示させる内容を設定するプロパティ</summary>
        public AskMessageModelBase AskMessage1
        {
            get => _AskMessage1;
            private set => SetValue(value, ref _AskMessage1);
        }
        private AskMessageModelBase _AskMessage1;

        #endregion

        #region てすと２

        public void Method_CallFromOuter_2(object arg)
        {
            //外部からコマンドで要求されたら、問い合わせのためのプロパティを設定する
            this.AskMessage2 = AskMessageModel.CreateYesNo
                ($"確認 {arg} ?"
                , yes: (o) =>
                    {//問い合わせの結果がYes
                        this.Value = "Yes";
                    }
                , no: (o) =>
                    {//問い合わせの結果がNo
                        this.Value = "No";
                    });
        }


        /// <summary>View側に問い合わせを表示させで表示させる内容を設定するプロパティ</summary>
        public AskMessageModelBase AskMessage2
        {
            get => _AskMessage2;
            private set => SetValue(value, ref _AskMessage2);
        }
        private AskMessageModelBase _AskMessage2;


        #endregion



    }


}
