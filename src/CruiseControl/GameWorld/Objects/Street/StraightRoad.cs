namespace CruiseControl.GameWorld.Objects.Street
{
    internal class StraightRoad : Road
    {
        internal int Length { get; private set; }

        public StraightRoad(IObjectContainer parent, int length)
            : base(parent)
        {
            Length = length;
        }
    }
}
