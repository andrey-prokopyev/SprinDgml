namespace SprinDgml
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.GraphModel;

    internal class DgmlBuilder
    {
        public void SaveGraph(string fileName, IEnumerable<Dependency> dependencies)
        {
            var graph = new Graph();

            foreach (var dependency in dependencies)
            {
                if (dependency.IsTargetPrimitive && dependency.Target.Label.Length > 50)
                {
                    var id = dependency.Target.Id;

                    var container = graph.Nodes.CreateNew("container:" + id);
                    container.IsGroup = true;
                    container.Label = container.Id.LiteralValue;
                    container.SetValue(GraphCommonSchema.Group, GraphGroupStyle.Collapsed);

                    var primitiveNode = graph.Nodes.CreateNew(id);
                    primitiveNode.Label = $"{dependency.Target.Label}";

                    var containerLink = graph.Links.GetOrCreate(container, primitiveNode, dependency.Order?.ToString() ?? string.Empty, GraphCommonSchema.Contains);
                    containerLink.IsGroup = true;

                    graph.Links.GetOrCreate(dependency.Source.Id, container.Id);
                }
                else
                {
                    var sourceNode = graph.Nodes.GetOrCreate(dependency.Source.Id);
                    sourceNode.Label = $"{dependency.Source.Label}";

                    var targetNode = graph.Nodes.GetOrCreate(dependency.Target.Id);
                    targetNode.Label = $"{dependency.Target.Label}";

                    var link = graph.Links.GetOrCreate(sourceNode, targetNode);

                    if (sourceNode.IsContained)
                    {
                        var container = sourceNode.ParentGroups.FirstOrDefault();
                        if (container != null)
                        {
                            var containerLink = graph.Links.GetOrCreate(container.GroupNode, targetNode, dependency.Order?.ToString() ?? string.Empty, GraphCommonSchema.Contains);
                            containerLink.IsGroup = true;
                        }
                    }

                    if (dependency.Order != null)
                    {
                        link.Label = dependency.Order.ToString();
                    }
                }
            }

            graph.Save(fileName);
        }
    }
}