namespace SprinDgml
{
    using System;
    using System.IO;

    internal class DgmlComposer
    {
        private readonly DependenciesGraphSource dependenciesGraphSource;

        private readonly DgmlBuilder dgmlBuilder;

        public DgmlComposer(DependenciesGraphSource dependenciesGraphSource, DgmlBuilder dgmlBuilder)
        {
            this.dependenciesGraphSource = dependenciesGraphSource;
            this.dgmlBuilder = dgmlBuilder;
        }

        public DgmlComposer()
            : this(new DependenciesGraphSource(), new DgmlBuilder())
        {
        }

        public void ComposeDgmlFile(string fileName)
        {
            var dependencies = this.dependenciesGraphSource.GetDependencies();

            var filePath = fileName;
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            }

            dgmlBuilder.SaveGraph(filePath, dependencies);
        }
    }
}