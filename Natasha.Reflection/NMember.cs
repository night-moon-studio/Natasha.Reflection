using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Natasha.Reflection
{
    
    public class NMember
    {
        public readonly bool CanWrite;
        public readonly bool CanRead;
        public readonly bool IsAsync;
        public readonly bool IsConst;
        public readonly bool IsArray;
        public readonly bool IsStatic;
        public readonly bool IsAbstract;
        public readonly bool IsVirtual;
        public readonly bool IsInterface;
        public readonly bool IsOverride;
        public readonly bool IsNew;
        public readonly bool IsOverrided;
        public readonly string MemberName;
        public readonly Type MemberType;
        public readonly Type ElementType;
        public readonly int ArrayLayer;
        public readonly int ArrayDimensions;
        public readonly MemberTypes MemberKind;
        public NMember(
            bool canWrite,
            bool canRead,
            bool isConst,
            string name,
            Type type,
            MemberTypes kind,
            bool isStatic,
            bool isAsync,
            bool isAbstract,
            bool isVirtural,
            bool isNew,
            bool isOverride)
            
        {

            CanWrite = canWrite;
            CanRead = canRead;
            IsConst = isConst;
            IsStatic = isStatic;
            IsAsync = isAsync;
            IsAbstract = isAbstract;
            IsVirtual = isVirtural;
            IsOverride = isOverride;
            IsNew = isNew;
            MemberName = name;
            MemberType = type;
            MemberKind = kind;
            IsArray = ElementType.IsArray;
            IsInterface = type.IsInterface;
            if (IsArray)
            {

                while (ElementType.HasElementType)
                {

                    ArrayLayer += 1;
                    ElementType = ElementType.GetElementType();

                }
                ArrayDimensions = type.GetConstructors()[0].GetParameters().Length;

            }

        }


        public static implicit operator NMember(FieldInfo info)
        {

            var isNew = false;
            var baseType = info.DeclaringType.BaseType;
            if (baseType != null && baseType != typeof(object))
            {
                isNew = baseType.GetField(info.Name) != null;
            }
                
            return new NMember(
               !info.IsInitOnly,
               !info.IsPrivate,
               info.IsLiteral,
               info.Name,
               info.FieldType,
               info.MemberType,
               info.IsStatic,false,false,false,
               isNew,false);

        }


        public static implicit operator NMember(PropertyInfo info)
        {
            var (isAsync, isStatic, isAbstract, isVirtual, isNew, isOverride) = info.CanRead ? GetMethodInfo(info.GetGetMethod()) : GetMethodInfo(info.GetSetMethod());
            return new NMember(
                info.CanWrite,
                info.CanRead,
                info.CanRead && !info.CanWrite,
                info.Name,
                info.PropertyType,
                info.MemberType,
                isStatic,
                isAsync,
                isAbstract,
                isVirtual,
                isNew,
                isOverride);

        }


        public static implicit operator NMember(MethodInfo info)
        {
            var (isAsync, isStatic, isAbstract, isVirtual, isNew, isOverride) = GetMethodInfo(info);
            return new NMember(
                false,
                true,
                false,
                info.Name,
                info.ReturnType,
                info.MemberType,
                isStatic,
                isAsync,
                isAbstract,
                isVirtual,
                isNew,
                isOverride);

        }


        public static (bool isAsync,
            bool isStatic,
            bool isAbstract, 
            bool isVirtual, 
            bool isNew,
            bool isOverride
            ) GetMethodInfo(MethodInfo info)
        {
            var isAsync = info.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
            bool isStatic = info.IsStatic;
            bool isAbstract = false;
            bool isVirtual = false;
            bool isNew = false;
            bool isOverride = false;
            if (!info.DeclaringType.IsInterface && !isStatic)
            {

                //如果没有被重写
                if (info.Equals(info.GetBaseDefinition()))
                {

                    if (info.IsAbstract)
                    {
                        isAbstract = true;
                    }
                    else if (!info.IsFinal && info.IsVirtual)
                    {
                        isVirtual = false;
                    }
                    else
                    {

                        var baseType = info.DeclaringType.BaseType;
                        if (baseType != null && baseType != typeof(object))
                        {
                            var baseInfo = info
                            .DeclaringType
                            .BaseType
                            .GetMethod(info.Name, 
                            BindingFlags.Public
                            | BindingFlags.Instance
                            | BindingFlags.NonPublic);
                            if (info != baseInfo)
                            {
                                isNew = true;
                            }
                        }

                    }

                }
                else
                {
                    isOverride = true;
                }

            }

            return (isAsync, isStatic, isAbstract, isVirtual, isNew, isOverride);
        }
    }
}
