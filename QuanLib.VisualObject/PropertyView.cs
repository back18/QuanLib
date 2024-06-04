using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.VisualObject
{
    public class PropertyView
    {
        public PropertyView(PropertyInfo propertyInfo, object? obj)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

            PropertyInfo = propertyInfo;
            _obj = obj;
        }

        private readonly object? _obj;

        public PropertyInfo PropertyInfo { get; }

        public AccessModifier AccessModifier => PropertyInfo.GetAccessModifier();

        public bool IsStatic => PropertyInfo.IsStatic();

        public MemberValue GetValue()
        {
            return MemberValue.GetValue(PropertyInfo, _obj);
        }

        public override string ToString()
        {
            MemberValue memberValue = GetValue();
            StringBuilder stringBuilder = new();

            stringBuilder.Append(AccessModifier.ToCodeString());
            stringBuilder.Append(' ');

            if (IsStatic)
                stringBuilder.Append("static ");

            stringBuilder.Append(ObjectFormatter.Format(memberValue.MemberType));

            if (memberValue.ValueType != memberValue.MemberType)
                stringBuilder.AppendFormat("({0})", ObjectFormatter.Format(memberValue.ValueType));

            stringBuilder.Append(' ');
            stringBuilder.Append(PropertyInfo.Name);
            stringBuilder.Append(" { ");

            if (PropertyInfo.GetMethod is MethodInfo getMethod)
            {
                AccessModifier accessModifier = getMethod.GetAccessModifier();
                if (accessModifier != AccessModifier)
                {
                    stringBuilder.Append(accessModifier.ToCodeString());
                    stringBuilder.Append(' ');
                }

                stringBuilder.Append("get; ");
            }

            if (PropertyInfo.SetMethod is MethodInfo setMethod)
            {
                AccessModifier accessModifier = setMethod.GetAccessModifier();
                if (accessModifier != AccessModifier)
                {
                    stringBuilder.Append(accessModifier.ToCodeString());
                    stringBuilder.Append(' ');
                }

                stringBuilder.Append("set; ");
            }

            stringBuilder.AppendFormat("}} = {{{0}}}", ObjectFormatter.Format(memberValue.Value));

            return stringBuilder.ToString();
        }
    }
}
