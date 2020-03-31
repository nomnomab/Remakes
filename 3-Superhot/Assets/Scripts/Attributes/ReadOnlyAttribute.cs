using System;
using UnityEngine;

namespace Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute: PropertyAttribute { }
}