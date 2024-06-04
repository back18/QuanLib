using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.VisualObject
{
    public class ParameterView
    {
        public ParameterView(ParameterInfo parameterInfo)
        {
            ArgumentNullException.ThrowIfNull(parameterInfo, nameof(parameterInfo));

            ParameterInfo = parameterInfo;
        }

        public ParameterInfo ParameterInfo { get; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            if (ParameterInfo.ParameterType.IsByRef)
            {
                if (ParameterInfo.IsIn)
                    stringBuilder.Append("in");
                else if (ParameterInfo.IsOut)
                    stringBuilder.Append("out");
                else
                    stringBuilder.Append("ref");
                stringBuilder.Append(' ');
            }

            stringBuilder.Append(ObjectFormatter.Format(ParameterInfo.ParameterType));
            stringBuilder.Append(' ');
            stringBuilder.Append(ParameterInfo.Name);

            if (ParameterInfo.HasDefaultValue)
            {
                stringBuilder.Append(" = ");
                stringBuilder.Append(ObjectFormatter.Format(ParameterInfo.DefaultValue));
            }

            return stringBuilder.ToString();
        }
    }
}
