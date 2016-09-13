namespace SprinDgml
{
    using System;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.GraphModel;

    internal class DgmlBuilder
    {
        public void SaveGraph(string fileName, IEnumerable<Dependency> dependencies)
        {
            var graph = new Graph();

            foreach (var dependency in dependencies)
            {
                if (dependency.IsTargetPrimitive)
                {
                    var id = Guid.NewGuid()
                            .ToString();

                    var container = graph.Nodes.CreateNew("container:" + id);
                    container.IsGroup = true;
                    container.Label = "...";
                    container.SetValue(GraphCommonSchema.Group, GraphGroupStyle.Collapsed);

                    var primitiveNode = graph.Nodes.CreateNew(id);
                    primitiveNode.Label = dependency.Target;

                    var containerLink = graph.Links.GetOrCreate(container, primitiveNode, dependency.Order?.ToString() ?? string.Empty, GraphCommonSchema.Contains);
                    containerLink.IsGroup = true;

                    graph.Links.GetOrCreate(dependency.Source, container.Id);
                }
                else
                {
                    var link = graph.Links.GetOrCreate(dependency.Source, dependency.Target);

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