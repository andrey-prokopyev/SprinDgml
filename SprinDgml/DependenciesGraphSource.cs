namespace SprinDgml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Spring.Context.Support;
    using Spring.Objects.Factory.Config;

    internal class DependenciesGraphSource
    {
        public IEnumerable<Dependency> GetDependencies()
        {
            var context = (XmlApplicationContext)ContextRegistry.GetContext();

            var names = context.GetObjectDefinitionNames();

            foreach (var name in names)
            {
                var def = context.GetObjectDefinition(name);
                var allArguments = def.ConstructorArgumentValues.NamedArgumentValues.Values.Cast<ConstructorArgumentValues.ValueHolder>().Concat(def.ConstructorArgumentValues.IndexedArgumentValues.Values).Concat(def.ConstructorArgumentValues.GenericArgumentValues).ToList();

                for (int i = 0; i < allArguments.Count; i++)
                {
                    var arg = allArguments[i];

                    var runtimeObjectReference = arg.Value as RuntimeObjectReference;
                    if (runtimeObjectReference != null)
                    {
                        yield return new Dependency(name, runtimeObjectReference.ObjectName, allArguments.Count > 1 ? i : (int?)null);
                    }

                    var managedList = arg.Value as ManagedList;
                    if (managedList != null)
                    {
                        for (int index = 0; index < managedList.Count; index++)
                        {
                            var l = managedList[index];
                            var reference = l as RuntimeObjectReference;

                            yield return new Dependency(name, reference != null ? reference.ObjectName : l.ToString(), managedList.Count > 1 ? index : 0);
                        }
                    }

                    var argType = arg.Value?.GetType();

                    if (argType != null && (argType.IsPrimitive || argType.IsEnum || argType == typeof(string) || argType == typeof(decimal) || argType == typeof(DateTime)))
                    {
                        yield return new Dependency(name, arg.Value?.ToString(), allArguments.Count > 1 ? i : (int?)null);
                    }
                }
            }
        }
    }
}