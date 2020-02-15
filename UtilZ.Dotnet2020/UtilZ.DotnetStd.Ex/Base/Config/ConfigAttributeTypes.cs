using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetStd.Ex.Base
{
    internal class ConfigAttributeTypes
    {
        public Type RootAttributeType { get; private set; } = typeof(ConfigRootAttribute);
        public Type IgnoreAttributeType { get; private set; } = typeof(ConfigIgnoreAttribute);
        public Type CollectionAttributeType { get; private set; } = typeof(ConfigItemCollectionAttribute);
        public Type ComplexItemAttribute { get; private set; } = typeof(ConfigComplexItemAttribute);
        public Type ItemAttributeType { get; private set; } = typeof(ConfigItemAttribute);
        public Type CustomerAttribute { get; private set; } = typeof(ConfigItemCustomerAttribute);

        public ConfigAttributeTypes()
        {

        }
    }
}
