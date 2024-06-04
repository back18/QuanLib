using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.VisualObject
{
    public class FieldView
    {
        public FieldView(FieldInfo fieldInfo, object? obj)
        {
            ArgumentNullException.ThrowIfNull(fieldInfo, nameof(fieldInfo));

            FieldInfo = fieldInfo;
            _obj = obj;
        }

        private readonly object? _obj;

        public FieldInfo FieldInfo { get; }

        public AccessModifier AccessModifier => FieldInfo.GetAccessModifier();

        public MemberValue GetValue()
        {
            return MemberValue.GetValue(FieldInfo, _obj);
        }

        public override string ToString()
        {
            MemberValue memberValue = GetValue();
            StringBuilder stringBuilder = new();

            stringBuilder.Append(AccessModifier.ToCodeString());
            stringBuilder.Append(' ');
            
            if (FieldInfo.IsStatic)
                stringBuilder.Append("static ");

            if (FieldInfo.IsInitOnly)
                stringBuilder.Append("readonly ");

            stringBuilder.Append(ObjectFormatter.Format(memberValue.MemberType));

            if (memberValue.ValueType != memberValue.MemberType)
                stringBuilder.AppendFormat("({0})", ObjectFormatter.Format(memberValue.ValueType));

            stringBuilder.Append(' ');
            stringBuilder.Append(FieldInfo.Name);

            stringBuilder.AppendFormat(" = {{{0}}}", ObjectFormatter.Format(memberValue.Value));

            return stringBuilder.ToString();
        }
    }
}
