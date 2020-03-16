using System;
using System.Collections.Generic;
using System.Reflection;
using Attributes;
using UnityEngine;

public class CustomBehaviour: MonoBehaviour {
    private const BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private void Awake() {
        FieldInfo[] fields = GetType().GetFields(_bindingFlags);
        PropertyInfo[] properties = GetType().GetProperties(_bindingFlags);
        List<MemberInfo> members = new List<MemberInfo>();
        members.AddRange(fields);
        members.AddRange(properties);

        foreach (MemberInfo member in members) {
            RequireAttribute requireAttribute = member.GetCustomAttribute<RequireAttribute>();
            RequireChildAttribute requireChildAttribute = member.GetCustomAttribute<RequireChildAttribute>();

            if (requireAttribute != null) {
                switch (member) {
                    case FieldInfo field:
                        field.SetValue(this, GetComponent(field.FieldType));
                        continue;
                    case PropertyInfo property:
                        property.SetValue(this, GetComponent(property.PropertyType));
                        continue;
                }
            } else if (requireChildAttribute != null) {
                Transform child;

                if (string.IsNullOrEmpty(requireChildAttribute.Name)) {
                    // index
                    child = transform.GetChild(requireChildAttribute.ChildIndex);
                }
                else {
                    // name
                    child = transform.Find(requireChildAttribute.Name);
                }
                
                switch (member) {
                    case FieldInfo field: {
                        Type type = field.FieldType;
                        
                        if (type == typeof(GameObject)) {
                            field.SetValue(this, child.gameObject);
                            continue;
                        }
                        
                        field.SetValue(this, child.GetComponent(requireChildAttribute.Type == null ? type : requireChildAttribute.Type));
                        continue;
                    }
                    case PropertyInfo property: {
                        Type type = property.PropertyType;
                        
                        if (type == typeof(GameObject)) {
                            property.SetValue(this, child.gameObject);
                            continue;
                        }
                        
                        property.SetValue(this, child.GetComponent(requireChildAttribute.Type == null ? type : requireChildAttribute.Type));
                        continue;
                    }
                }
            }
        }
    }
}