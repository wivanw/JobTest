using System;
using System.Linq;
using Core.VBCM.Helper;
using UnityEngine;

namespace VBCM.Asserts
{
    public static class Assert
    {
        public static void Warn(bool condition)
        {
            if (!condition)
                Debug.LogWarning("Warning!  See call stack");
        }

        public static void Warn(bool condition, Func<string> messageGenerator)
        {
            if (!condition)
                Debug.LogWarning("Warning Assert hit! " + messageGenerator());
        }

        public static void Warn(bool condition, string message)
        {
            if (!condition)
                Debug.LogWarning("Warning Assert hit! " + message);
        }

        public static void That(
            bool condition, string message)
        {
            if (!condition)
                throw CreateException("Assert hit! " + message);
        }

        // We don't use params here to avoid the memory alloc
        public static void That(
            bool condition, string message, object p1)
        {
            if (!condition)
                throw CreateException("Assert hit! " + FormatString(message, p1));
        }

        // We don't use params here to avoid the memory alloc
        public static void That(
            bool condition, string message, object p1, object p2)
        {
            if (!condition)
                throw CreateException("Assert hit! " + FormatString(message, p1, p2));
        }

        // We don't use params here to avoid the memory alloc
        public static void That(
            bool condition, string message, object p1, object p2, object p3)
        {
            if (!condition)
                throw CreateException("Assert hit! " + FormatString(message, p1, p2, p3));
        }

        public static void IsNull(object val)
        {
            if (!val.IsNull())
                throw CreateException("Assert Hit! Expected null pointer but instead found '{0}'", val);
        }

        public static void IsNull(object val, string message)
        {
            if (!val.IsNull())
                throw CreateException("Assert Hit! {0}", message);
        }

        // We don't use params here to avoid the memory alloc
        public static void IsNull(object val, string message, object p1)
        {
            if (!val.IsNull())
                throw CreateException("Assert Hit! {0}", FormatString(message, p1));
        }

        public static void IsNotNull(object val)
        {
            if (val.IsNull())
                throw CreateException("Assert Hit! Found null pointer when value was expected");
        }

        public static void IsNotNull(object val, string message)
        {
            if (val.IsNull())
                throw CreateException("Assert Hit! {0}", message);
        }

        // We don't use params here to avoid the memory alloc
        public static void IsNotNull(object val, string message, object p1)
        {
            if (val.IsNull())
                throw CreateException("Assert Hit! {0}", FormatString(message, p1));
        }

        // We don't use params here to avoid the memory alloc
        public static void IsNotNull(object val, string message, object p1, object p2)
        {
            if (val.IsNull())
                throw CreateException("Assert Hit! {0}", FormatString(message, p1, p2));
        }

        public static void Throws(Action action)
        {
            Throws<Exception>(action);
        }

        public static void Throws<TException>(Action action)
            where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }

            throw CreateException(
                "Expected to receive exception of type '{0}' but nothing was thrown", typeof(TException).Name);
        }

        static string FormatString(string format, params object[] parameters)
        {
            // ensure nulls are replaced with "NULL"
            // and that the original parameters array will not be modified
            if (parameters == null || parameters.Length <= 0)
                return format;

            var paramToUse = parameters;

            if (parameters.Any(cur => cur == null))
            {
                paramToUse = new object[parameters.Length];
                for (var i = 0; i < parameters.Length; ++i)
                    paramToUse[i] = parameters[i] ?? "NULL";
            }

            format = string.Format(format, paramToUse);
            return format;
        }

        public static VbcmException CreateException()
        {
            return new VbcmException("Assert hit!");
        }

        public static VbcmException CreateException(string message)
        {
            return new VbcmException(message);
        }

        public static VbcmException CreateException(string message, params object[] parameters)
        {
            return new VbcmException(FormatString(message, parameters));
        }

        public static VbcmException CreateException(Exception innerException, string message,
            params object[] parameters)
        {
            return new VbcmException(FormatString(message, parameters), innerException);
        }
    }
}