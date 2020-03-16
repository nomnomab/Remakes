using System;

namespace Attributes {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RequireChildAttribute : Attribute {
        public string Name { get; }
        public int ChildIndex { get; }
        public Type Type { get; }

        public RequireChildAttribute(string name) {
            Name = name;
        }
        
        public RequireChildAttribute(string name, Type type) {
            Name = name;
            Type = type;
        }
        
        public RequireChildAttribute(int index) {
            ChildIndex = index;
        }
        
        public RequireChildAttribute(int index, Type type) {
            ChildIndex = index;
            Type = type;
        }
    }
}