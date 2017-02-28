namespace Genesis.Selection
{
	public interface IPopulationSelector
	{
		double[] GetSelectionPointers(uint numPointers);
	}
}
