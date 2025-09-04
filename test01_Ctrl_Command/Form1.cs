using ScriptEditorUtility;


namespace test01_Ctrl_Command
{
	public partial class Form1 : Form
	{
		public Form1 ()
		{
			InitializeComponent ();

			EditListbox < TName > el_int = new EditListbox < TName > ();
			this.Controls.Add (el_int);
		}
	}
}
