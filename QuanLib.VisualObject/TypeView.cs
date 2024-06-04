using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.VisualObject
{
    public class TypeView
    {
        public TypeView(Type type, object? obj)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            Type = type;
            _obj = obj;
        }

        private readonly object? _obj;

        public Type Type { get; }

        public MemberType MemberType => Type.GetMemberType();

        public AccessModifier AccessModifier => Type.GetAccessModifier();

        public bool IsStatic => Type.IsStatic();

        public Type[] GetInheritanceTree()
        {
            return Type.GetInheritanceTree();
        }

        public FieldView[] GetStaticFields()
        {
            return Type
                .GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(s => new FieldView(s, _obj))
                .ToArray();
        }

        public PropertyView[] GetStaticProperties()
        {
            return Type
                .GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(w => w.GetIndexParameters().Length == 0)
                .Select(s => new PropertyView(s, _obj))
                .ToArray();
        }

        public MethodView[] GetStaticMethods()
        {
            return Type
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(w => !w.IsAutoMethod())
                .Select(s => new MethodView(s, _obj))
                .ToArray();
        }

        public FieldView[] GetFields()
        {
            return Type
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(s => new FieldView(s, _obj))
                .ToArray();
        }

        public PropertyView[] GetProperties()
        {
            return Type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(w => w.GetIndexParameters().Length == 0)
                .Select(s => new PropertyView(s, _obj))
                .ToArray();

        }

        public MethodView[] GetMethods()
        {
            return Type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(w => !w.IsAutoMethod())
                .Select(s => new MethodView(s, _obj))
                .ToArray();
        }

        public ConstructorView[] GetConstructors()
        {
            return Type
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(s => new ConstructorView(s))
                .ToArray();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(AccessModifier.ToCodeString());
            stringBuilder.Append(' ');

            if (Type.IsAbstract && Type.IsSealed)
                stringBuilder.Append("static ");
            else if (Type.IsAbstract)
                stringBuilder.Append("abstract ");
            else if (Type.IsSealed)
                stringBuilder.Append("sealed ");

            stringBuilder.Append(MemberType.ToCodeString());
            stringBuilder.Append(' ');

            Type[] inheritanceTree = GetInheritanceTree();
            Array.Reverse(inheritanceTree);
            stringBuilder.AppendJoin(" -> ", inheritanceTree.Select(s => ObjectFormatter.Format(s)));
            stringBuilder.AppendLine();

            AppendLines(GetConstructors());
            AppendLines(GetStaticFields());
            AppendLines(GetStaticProperties());
            AppendLines(GetStaticMethods());
            AppendLines(GetFields());
            AppendLines(GetProperties());
            AppendLines(GetMethods());

            return stringBuilder.ToString();

            void AppendLines<T>(T[] values) where T : notnull
            {
                if (values.Length == 0)
                    return;

                stringBuilder.AppendLine();
                foreach (T value in values)
                    stringBuilder.AppendLine(value.ToString());
            }
        }
    }
}
