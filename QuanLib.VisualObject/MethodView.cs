using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.VisualObject
{
    public class MethodView
    {
        public MethodView(MethodInfo methodInfo, object? obj)
        {
            ArgumentNullException.ThrowIfNull(methodInfo, nameof(methodInfo));

            MethodInfo = methodInfo;
            _obj = obj;
        }

        private readonly object? _obj;

        public MethodInfo MethodInfo { get; }

        public AccessModifier AccessModifier => MethodInfo.GetAccessModifier();

        public MemberValue GetValue(object?[]? parameters)
        {
            return MemberValue.GetValue(MethodInfo, _obj, parameters);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(AccessModifier.ToCodeString());
            stringBuilder.Append(' ');

            if (MethodInfo.IsStatic)
                stringBuilder.Append("static ");

            stringBuilder.Append(ObjectFormatter.Format(MethodInfo.ReturnType));
            stringBuilder.Append(' ');
            stringBuilder.Append(MethodInfo.Name);
            stringBuilder.Append('(');
            stringBuilder.AppendJoin(", ", MethodInfo.GetParameters().Select(s => new ParameterView(s)));
            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }
    }
}
