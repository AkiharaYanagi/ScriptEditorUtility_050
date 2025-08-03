namespace ScriptEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            FormUtility.InitPosition ( this );
            InitializeComponent();

            //test Chara
            Chara chara = new Chara ();
            TestChara testChara = new TestChara ();
            testChara.Test ( chara );
        }
    }
}
