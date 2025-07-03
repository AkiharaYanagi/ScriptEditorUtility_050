
namespace ScriptEditor
{
	public interface IName
	{
		string Name { get; set; }
	}

	public class TName : IName
	{
		public string Name { get; set; } = "TName";

		public TName ()
		{
		}

		public TName ( object ob )
		{
			Name = ob?.ToString () ?? string.Empty;
		}

		public TName ( string n )
		{
			Name = n;
		}
	}
}
