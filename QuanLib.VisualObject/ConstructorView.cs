using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.VisualObject
{
    public class ConstructorView
    {
        public ConstructorView(ConstructorInfo constructorInfo)
        {
            ArgumentNullException.ThrowIfNull(constructorInfo, nameof(constructorInfo));

            ConstructorInfo = constructorInfo;
        }

        public ConstructorInfo ConstructorInfo { get; }

        public AccessModifier AccessModifier => ConstructorInfo.GetAccessModifier();

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(AccessModifier.ToString());
            stringBuilder.Append(' ');
            stringBuilder.Append(ObjectFormatter.Format(ConstructorInfo.DeclaringType).Split('.')[^1]);
            stringBuilder.Append('(');
            stringBuilder.AppendJoin(", ", ConstructorInfo.GetParameters().Select(s => new ParameterView(s)));
            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }
    }
}
