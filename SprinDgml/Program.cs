namespace SprinDgml
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new ConfigurationFileLoader().Load(args.Length > 1 ? args[1] : null);

            var dgmlComposer = new DgmlComposer();
            dgmlComposer.ComposeDgmlFile(args.Length > 0 ? args[0] : "spring.dgml");
        }
    }
}
