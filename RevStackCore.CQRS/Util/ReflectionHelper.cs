﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RevStackCore.CQRS.Attribute;
using RevStackCore.CQRS.Domain;
using RevStackCore.CQRS.Event;

namespace RevStackCore.CQRS.Util
{
    public static class ReflectionHelper
    {
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, string>> AggregateEventHandlerCache =
            new ConcurrentDictionary<Type, ConcurrentDictionary<Type, string>>();

        public static Dictionary<Type, string> FindEventHandlerMethodsInAggregate(Type aggregateType)
        {
            if (AggregateEventHandlerCache.ContainsKey(aggregateType) == false)
            {
                var eventHandlers = new ConcurrentDictionary<Type, string>();

                var methods = aggregateType.GetMethodsBySig(typeof(void), typeof(InternalEventHandler), true, typeof(IEvent)).ToList();

                if (methods.Any())
                {
                    foreach (var m in methods)
                    {
                        var parameter = m.GetParameters().First();
                        if (eventHandlers.TryAdd(parameter.ParameterType, m.Name) == false)
                        {
                            throw new TargetException($"Multiple methods found handling same event in {aggregateType.Name}");
                        }
                    }
                }

                if (AggregateEventHandlerCache.TryAdd(aggregateType, eventHandlers) == false)
                {
                    throw new TargetException($"Error registering methods for handling events in {aggregateType.Name}");
                }
            }


            return AggregateEventHandlerCache[aggregateType].ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static IEnumerable<MethodInfo> GetMethodsBySig(this Type type,
                                                               Type returnType,
                                                               Type customAttributeType,
                                                               bool matchParameterInheritence,
                                                               params Type[] parameterTypes)
        {
            return type.GetTypeInfo().GetMethods().Where((m) =>
            {
                if (m.ReturnType != returnType) return false;

                if ((customAttributeType != null) && (m.GetCustomAttributes(customAttributeType, true).Any() == false))
                    return false;

                var parameters = m.GetParameters();

                if ((parameterTypes == null || parameterTypes.Length == 0))
                    return parameters.Length == 0;

                if (parameters.Length != parameterTypes.Length)
                    return false;

                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    if (((parameters[i].ParameterType == parameterTypes[i]) ||
                    (matchParameterInheritence && parameterTypes[i].GetTypeInfo().IsAssignableFrom(parameters[i].ParameterType))) == false)
                        return false;
                }

                return true;
            });
        }

        public static string GetTypeName(Type t)
        {
            return t.Name;
        }

        public static string GetTypeFullName(Type t)
        {
            return t.AssemblyQualifiedName;
        }

        public static MethodInfo[] GetMethods(Type t)
        {
            return t.GetTypeInfo().GetMethods();
        }

        public static MethodInfo GetMethod(Type t, string methodName, Type[] paramTypes)
        {
            return t.GetTypeInfo().GetMethod(methodName, paramTypes);
        }

        public static MemberInfo[] GetMembers(Type t)
        {
            return t.GetTypeInfo().GetMembers();
        }

        public static T CreateInstance<T>() where T : AggregateRoot
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        public static object CreateInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
