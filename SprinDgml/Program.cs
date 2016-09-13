namespace SprinDgml
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dgmlComposer = new DgmlComposer();
            dgmlComposer.ComposeDgmlFile(args?[0] ?? "spring.dgml");
        }
    }
}
