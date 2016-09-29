namespace SprinDgml
{
    internal class Dependency
    {
        public Dependency(Node source, Node target, int? order = 0, bool isTargetPrimitive = false)
        {
            Source = source;
            Target = target;
            Order = order;
            IsTargetPrimitive = isTargetPrimitive;
        }

        protected bool Equals(Dependency other)
        {
            return Equals(Source, other.Source) && Equals(Target, other.Target);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Dependency)obj);
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Source != null ? Source.GetHashCode() : 0) * 397) ^ (Target != null ? Target.GetHashCode() : 0);
            }
        }

        public Node Source { get; }

        public Node Target { get; }

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
            return $"{this.Order}. [{this.Source}] => [{this.Target}], IsTargetPrimitive = {this.IsTargetPrimitive}";
        }
    }
}