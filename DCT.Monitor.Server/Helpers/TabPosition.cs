namespace DCT.Monitor.Server.Helpers
{
	public class TabPosition
	{
		private int _tp;

		public TabPosition(int tabPosition)
		{
			_tp = tabPosition;
		}

		public int Position
		{
			get { return _tp; }
			set { _tp = value; }
		}

		public string CheckPos(string className, int pos)
		{
			return _tp == pos ? className : "";
		}
	}
}