namespace Game.Logic.Phy.Object
{
	public class Layer : PhysicalObj
	{
		public override int Type => 2;

		public Layer(int id, string name, string model, string defaultAction, int scale, int rotation)
			: base(id, name, model, defaultAction, scale, rotation, 0)
		{
		}
	}
}
