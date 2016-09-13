namespace SprinDgml
{
    internal class Dependency
    {
        public Dependency(string source, string target, int? order = 0, bool isTargetPrimitive = false)
        {
            Source = source;
            Target = target;
            Order = order;
            IsTargetPrimitive = isTargetPrimitive;
        }

        public string Source { get; }

        public string Target { get; }

        public bool IsTargetPrimitive { get; }

        public int? Order { get; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Order}. {this.Source} => {this.Target}, IsTargetPrimitive = {this.IsTargetPrimitive}";
        }
    }
}