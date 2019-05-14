using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VBCM.Asserts;
using VBCM.Interfaces;

// <copyright company="XIM Inc.">
// <author>Sergey Orlov, sergey.orlov@ximxim.com</author>
// <author>Ivan Bondarenko, wivanw@gmail.com</author>
// </copyright>

namespace VBCM
{
    /// <summary>
    /// Base class for any Logic group management
    /// </summary>
    public class Controller : IController
    {
        private readonly IDictionary<Type, List<WeakReference>> _commandStorage =
            new Dictionary<Type, List<WeakReference>>();

        private readonly IDictionary<Type, List<WeakReference>> _lateCommandStorage =
            new Dictionary<Type, List<WeakReference>>();

        /// <inheritdoc />
        public void Add<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.IHandler handler)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            var commandList = GetCommandList(typeof(THub), _commandStorage);
            commandList.Add(new WeakReference(handler));
        }

        /// <inheritdoc />
        public void Remove<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.IHandler handler)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            var commandList = GetCommandList(typeof(THub), _commandStorage);
            commandList.RemoveAll(com => com.Target == handler);
        }

        /// <inheritdoc />
        public void DoLogicWork<THub, TSendValue, TCallBackValue>(TSendValue sendValue,
            Hub<THub, TSendValue, TCallBackValue>.ActionsPack actionPack)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            var typeHub = typeof(THub);
            var commandList = GetCommandList(typeHub, _commandStorage);
            if (!commandList.Any())
            {
                Debug.LogError("No action for this Hub: " + typeHub);
                return;
            }

            var lateCommandList = GetCommandList(typeHub, _lateCommandStorage);

            foreach (var weakReference in commandList)
            {
                var handler = (Hub<THub, TSendValue, TCallBackValue>.IHandler) weakReference.Target;
                var callBackValue = handler.GetCallBackValue(sendValue);
                actionPack.CallBack.Invoke(callBackValue);

                foreach (var lateWeakReference in lateCommandList)
                {
                    var action = (Action<TCallBackValue>) lateWeakReference.Target;
                    action.Invoke(callBackValue);
                }
            }
        }

        /// <inheritdoc />
        public void RegCallBack<THub, TSendValue, TCallBackValue>(Action<TCallBackValue> callBack)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            var typeHub = typeof(THub);
            var list = GetCommandList(typeHub, _lateCommandStorage);
            list.Add(new WeakReference(callBack));
        }

        /// <inheritdoc />
        public void UnRegCallBack<THub, TSendValue, TCallBackValue>(Action<TCallBackValue> callBack)
        {
            var typeHub = typeof(THub);
            var list = GetCommandList(typeHub, _lateCommandStorage);
            var isRemove = list.Remove(new WeakReference(callBack));
            Assert.Warn(isRemove, AssertMessage.UnRegCallBack);
        }

        private List<WeakReference> GetCommandList(Type subscriberType, IDictionary<Type, List<WeakReference>> target)
        {
            List<WeakReference> subscribers;
            var isExists = target.TryGetValue(subscriberType, out subscribers);
            if (isExists)
            {
                subscribers.RemoveAll(obj => !obj.IsAlive);
                return subscribers;
            }

            subscribers = new List<WeakReference>();
            target.Add(subscriberType, subscribers);

            return subscribers;
        }
    }
}