using System;
using System.Collections.Generic;
using VBCM.Interfaces;

// <copyright company="XIM Inc.">
// <author>Sergey Orlov, sergey.orlov@ximxim.com</author>
// <author>Ivan Bondarenko, wivanw@gmail.com</author>
// </copyright>

namespace VBCM
{
    public sealed class Validator : IValidator
    {
        private readonly Dictionary<Type, List<WeakReference>> _commandValidationStorage =
            new Dictionary<Type, List<WeakReference>>();

        /// <inheritdoc />
        public void Add<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.IValidated validated)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            var type = typeof(THub);
            var list = GetCommandList(type);
            list.Add(new WeakReference(validated));
        }

        /// <inheritdoc />
        public void Remove<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.IValidated validated)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            var type = typeof(THub);
            var list = GetCommandList(type);
            list.RemoveAll(com => com.Target == validated);
        }

        /// <inheritdoc />
        public bool IsValidStateOperation<THub, TSendValue, TCallBackValue>(TSendValue sendValue,
            out string errorMessage)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            var type = typeof(THub);
            var list = GetCommandList(type);

            foreach (var weakReference in list)
            {
                var validated = (Hub<THub, TSendValue, TCallBackValue>.IValidated) weakReference.Target;
                var isValid = validated.Validate(sendValue, out errorMessage);
                if (!isValid)
                    return false;
            }

            return GetDefaultValidResult(out errorMessage);
            ;
        }

        private List<WeakReference> GetCommandList(Type subscriberType)
        {
            List<WeakReference> subscribers;
            var isExists = _commandValidationStorage.TryGetValue(subscriberType, out subscribers);
            if (isExists)
            {
                subscribers.RemoveAll(obj => !obj.IsAlive);
                return subscribers;
            }

            subscribers = new List<WeakReference>();
            _commandValidationStorage.Add(subscriberType, subscribers);

            return subscribers;
        }

        private static bool GetDefaultValidResult(out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}