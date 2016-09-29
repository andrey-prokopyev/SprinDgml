namespace SprinDgml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Spring.Context.Support;
    using Spring.Objects.Factory.Config;

    internal class DependenciesGraphSource
    {
        private readonly Dictionary<string, Node> nodes = new Dictionary<string, Node>();

        public IEnumerable<Dependency> GetDependencies()
        {
            var context = (XmlApplicationContext)ContextRegistry.GetContext();

            var names = context.GetObjectDefinitionNames();

            var result = new List<Dependency>();

            for (int i = 0; i < names.Count; i++)
            {
                var source = names[i];
                var def = context.GetObjectDefinition(source);

                this.FillDependencies(context, null, def, result, i);
            }

            return result;
        }

        private void FillDependencies(XmlApplicationContext context, string source, object obj, List<Dependency> dependencies, int index)
        {
            IObjectDefinition def = null;

            var holder = obj as ObjectDefinitionHolder;
            if (holder != null)
            {
                def = holder.ObjectDefinition;
            }

            if (def == null)
            {
                def = obj as IObjectDefinition;
            }

            if (def != null)
            {
                if (!string.IsNullOrEmpty(source))
                {
                    var dependency = new Dependency(this.GetNode(source), this.GetNode(def.ObjectTypeName), index);
                    if (!dependencies.Contains(dependency))
                    {
                        dependencies.Add(dependency);
                    }
                }

                var allArguments = def.ConstructorArgumentValues.NamedArgumentValues.Values.Cast<ConstructorArgumentValues.ValueHolder>().Concat(def.ConstructorArgumentValues.IndexedArgumentValues.Values).Concat(def.ConstructorArgumentValues.GenericArgumentValues).ToList();

                for (int i = 0; i < allArguments.Count; i++)
                {
                    var argument = allArguments[i];
                    this.FillDependencies(context, def.ObjectTypeName, argument.Value, dependencies, i);
                }
            }

            var runtimeObjectReference = obj as RuntimeObjectReference;
            if (runtimeObjectReference != null)
            {
                var definition = context.GetObjectDefinition(runtimeObjectReference.ObjectName);
                var dependency = new Dependency(this.GetNode(source), this.GetNode(definition.ObjectTypeName), index);
                if (!dependencies.Contains(dependency))
                {
                    dependencies.Add(dependency);
                }
            }

            var managedList = obj as ManagedList;
            if (managedList != null)
            {
                var listId = $"list-{index}";
                var dependency = new Dependency(this.GetNode(source), this.GetNode(listId), index);
                if (!dependencies.Contains(dependency))
                {
                    dependencies.Add(dependency);
                }

                for (int i = 0; i < managedList.Count; i++)
                {
                    var item = managedList[i];
                    this.FillDependencies(context, listId, item, dependencies, i);
                }
            }

            var managedDictionary = obj as ManagedDictionary;
            if (managedDictionary != null)
            {
                var keys = managedDictionary.Keys.Cast<object>().ToArray();
                for (int i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    var value = managedDictionary[key];
                    this.FillDependencies(context, source, key, dependencies, i);
                    this.FillDependencies(context, source, value, dependencies, i);
                }
            }

            var managedSet = obj as ManagedSet;
            if (managedSet != null)
            {
                int i = 0;
                foreach (var item in managedSet)
                {
                    this.FillDependencies(context, source, item, dependencies, i++);
                }
            }

            var managedNvc = obj as ManagedNameValueCollection;
            if (managedNvc != null)
            {
                for (int i = 0; i < managedNvc.AllKeys.Length; i++)
                {
                    var key = managedNvc.AllKeys[i];
                    var value = managedNvc[key];
                    this.FillDependencies(context, source, key, dependencies, i);
                    this.FillDependencies(context, key, value, dependencies, i);
                }
            }

            var argType = obj.GetType();

            if (argType.IsPrimitive || argType.IsEnum || argType == typeof(string) || argType == typeof(decimal) || argType == typeof(DateTime))
            {
                var dependency = new Dependency(this.GetNode(source), this.GetNode(obj.ToString()), index);
                if (!dependencies.Contains(dependency))
                {
                    dependencies.Add(dependency);
                }
            }
        }

        private Node GetNode(string label)
        {
            if (!this.nodes.ContainsKey(label))
            {
                var node = new Node { Id = Guid.NewGuid().ToString(), Label = label };

                this.nodes[label] = node;

                return this.nodes[label];
            }

            return this.nodes[label];
        }
    }
}