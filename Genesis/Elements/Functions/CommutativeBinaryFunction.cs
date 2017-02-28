namespace Genesis.Elements.Functions
{
    public abstract class CommutativeBinaryFunction : BinaryFunction
    {
        protected CommutativeBinaryFunction(IElement firstElement, IElement secondElement) : base(
            firstElement.CompareTo(secondElement) >= 0 ? firstElement : secondElement,
            firstElement.CompareTo(secondElement) >= 0 ? secondElement : firstElement)
        {
        }
    }
}