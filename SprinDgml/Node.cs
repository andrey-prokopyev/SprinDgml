namespace SprinDgml
{
    public class Node
    {
        public string Id { get; set; }

        public string Label { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"Id: {Id}, Label: {Label}";
        }
    }
}