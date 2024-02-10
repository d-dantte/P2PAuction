namespace P2PAuction.Utils
{
    public static class Extensions
    {
        public static TOut ApplyTo<TIn, TOut>(this TIn @in, Func<TIn, TOut> mapper)
        {
            ArgumentNullException.ThrowIfNull(mapper);

            return mapper.Invoke(@in);
        }
    }
}
