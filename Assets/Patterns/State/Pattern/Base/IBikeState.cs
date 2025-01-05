namespace Patterns.State
{
	public interface IBikeState
	{
		public void Handle(BikeController controller);
	}
}
