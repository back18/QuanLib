using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class ReflectionUtil
    {
        private static readonly string[] _autoMethodPrefixs = ["get_", "set_", "add_", "remove_", "op_"];

        public static Type[] GetInheritanceTree(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            List<Type> result = [];
            Type? current = type;

            while (current is not null)
            {
                result.Add(current);
                current = current.BaseType;
            }

            result.Reverse();
            return result.ToArray();
        }

        public static bool IsStatic(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            return type.IsAbstract && type.IsSealed;
        }

        public static bool IsStatic(this PropertyInfo propertyInfo)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

            if (propertyInfo.GetMethod is MethodInfo getMethod)
                return getMethod.IsStatic;
            else if (propertyInfo.SetMethod is MethodInfo setMethod)
                return setMethod.IsStatic;
            else
                return false;
        }

        public static bool IsAutoMethod(this MemberInfo methodInfo)
        {
            ArgumentNullException.ThrowIfNull(methodInfo, nameof(methodInfo));

            foreach (string prefix in _autoMethodPrefixs)
            {
                string[] items = methodInfo.Name.Split('.');
                if (items[^1].StartsWith(prefix))
                    return true;
            }

            return false;
        }

        public static MemberType GetMemberType(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof (type));

            if (type.IsEnum)
                return MemberType.Enum;
            else if (type.IsValueType)
                return MemberType.Struct;
            else if (type.IsSubclassOf(typeof(Delegate)))
                return MemberType.Delegate;
            else if (type.IsArray)
                return MemberType.Array;
            else if (type.IsInterface)
                return MemberType.Interface;
            else if (type.IsClass)
                return MemberType.Class;
            else
                throw new InvalidEnumArgumentException();
        }

        public static AccessModifier GetAccessModifier(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            if (type.IsPublic || type.IsNotPublic)
                return AccessModifier.Public;
            else if (type.IsNotPublic || type.IsNestedPrivate)
                return AccessModifier.Private;
            else if (type.IsNestedFamily)
                return AccessModifier.Protected;
            else if (type.IsNestedAssembly)
                return AccessModifier.Internal;
            else if (type.IsNestedFamORAssem)
                return AccessModifier.ProtectedInternal;
            else if (type.IsNestedFamANDAssem)
                return AccessModifier.PrivateProtected;
            else
                return AccessModifier.File;
        }

        public static AccessModifier GetAccessModifier(this FieldInfo fieldInfo)
        {
            ArgumentNullException.ThrowIfNull(fieldInfo, nameof(fieldInfo));

            if (fieldInfo.IsPublic)
                return AccessModifier.Public;
            else if (fieldInfo.IsPrivate)
                return AccessModifier.Private;
            else if (fieldInfo.IsFamily)
                return AccessModifier.Protected;
            else if (fieldInfo.IsAssembly)
                return AccessModifier.Internal;
            else if (fieldInfo.IsFamilyOrAssembly)
                return AccessModifier.ProtectedInternal;
            else if (fieldInfo.IsFamilyAndAssembly)
                return AccessModifier.PrivateProtected;
            else
                return AccessModifier.File;
        }

        public static AccessModifier GetAccessModifier(this PropertyInfo propertyInfo)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

            AccessModifier? getMethod = propertyInfo.GetMethod?.GetAccessModifier();
            AccessModifier? setMethod = propertyInfo.SetMethod?.GetAccessModifier();

            if (getMethod.HasValue && setMethod.HasValue)
                return getMethod.Value > setMethod.Value ? getMethod.Value : setMethod.Value;
            else if (getMethod.HasValue)
                return getMethod.Value;
            else if (setMethod.HasValue)
                return setMethod.Value;
            else
                return AccessModifier.File;
        }

        public static AccessModifier GetAccessModifier(this MethodBase methodBase)
        {
            ArgumentNullException.ThrowIfNull(methodBase, nameof(methodBase));

            if (methodBase.IsPublic)
                return AccessModifier.Public;
            else if (methodBase.IsPrivate)
                return AccessModifier.Private;
            else if (methodBase.IsFamily)
                return AccessModifier.Protected;
            else if (methodBase.IsAssembly)
                return AccessModifier.Internal;
            else if (methodBase.IsFamilyOrAssembly)
                return AccessModifier.ProtectedInternal;
            else if (methodBase.IsFamilyAndAssembly)
                return AccessModifier.PrivateProtected;
            else
                return AccessModifier.File;
        }

        public static string ToCodeString(this MemberType memberType)
        {
            return memberType switch
            {
                MemberType.Class => "class",
                MemberType.Interface => "interface",
                MemberType.Struct => "struct",
                MemberType.Enum => "enum",
                MemberType.Array => "array",
                MemberType.Delegate => "delegate",
                _ => throw new InvalidEnumArgumentException()
            };
        }

        public static string ToCodeString(this AccessModifier accessModifier)
        {
            return accessModifier switch
            {
                AccessModifier.Public => "public",
                AccessModifier.Private => "private",
                AccessModifier.Protected => "protected",
                AccessModifier.Internal => "internal",
                AccessModifier.ProtectedInternal => "protected internal",
                AccessModifier.PrivateProtected => "private protected",
                AccessModifier.File => "file",
                _ => throw new InvalidEnumArgumentException()
            };
        }
    }
}
