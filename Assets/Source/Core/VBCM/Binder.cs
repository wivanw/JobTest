using System;
using System.Collections.Generic;
using Core.VBCM.Helper;
using VBCM.Asserts;
using VBCM.Interfaces;

// <copyright company="XIM Inc.">
// <author>Sergey Orlov, sergey.orlov@ximxim.com</author>
// <author>Ivan Bondarenko, wivanw@gmail.com</author>
// </copyright>

namespace VBCM
{
    public class Binder : IBinder
    {
        private readonly IController _controller;
        private readonly IValidator _validator;
        private readonly Dictionary<Type, Dictionary<WeakReference, Action>> _binds;

        protected Binder(IController controller, IValidator validator)
        {
            _controller = controller;
            _validator = validator;
            _binds = new Dictionary<Type, Dictionary<WeakReference, Action>>();
        }

        /// <inheritdoc />
        public void Bind<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.IEventSource eventSource,
            Hub<THub, TSendValue, TCallBackValue>.IBindable bindable)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            Assert.IsNotNull(eventSource, AssertMessage.CantProvide);
            // ======================================================
            var bindActions = bindable.BindActions;

            Action action = () =>
            {
                // Get simple data from UI
                var sendValue = bindActions.GetSendValue.Invoke();
                // ======================================================
                string errorMessage;
                var isValid =
                    _validator.IsValidStateOperation<THub, TSendValue, TCallBackValue>(sendValue, out errorMessage);

                if (isValid)
                    _controller.DoLogicWork(sendValue, bindActions);
                else
                    bindActions.NonValidAction(errorMessage);
            };
            // ==========================================================

            eventSource.Event += action;
            var eventUnitsDic = GetBindList(typeof(THub));
            eventUnitsDic.Add(new WeakReference(eventSource), action);
        }

        /// <inheritdoc />
        public void UnBind<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.IEventSource eventSource)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            Assert.IsNull(eventSource, AssertMessage.CantProvide);
            // ======================================================
            var type = typeof(THub);
            var eventUnitsDic = GetBindList(type);

            Action action;
            var isExist = eventUnitsDic.TryGetValue(new WeakReference(eventSource), out action);
            Assert.Warn(isExist, AssertMessage.UnBindError);
            if (isExist)
            {
                eventSource.Event -= action;
                if (_binds[type].Count == 0)
                    _binds.Remove(type);
            }
        }


        /// <inheritdoc />
        public void BindCallBack<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.ICallBackBindable callBack)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            _controller.RegCallBack<THub, TSendValue, TCallBackValue>(callBack.CallBack);
        }

        /// <inheritdoc />
        public void UnBindCallBack<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.ICallBackBindable callBack)
            where THub : IHub<THub, TSendValue, TCallBackValue>
        {
            _controller.UnRegCallBack<THub, TSendValue, TCallBackValue>(callBack.CallBack);
        }

        private Dictionary<WeakReference, Action> GetBindList(Type type)
        {
            Dictionary<WeakReference, Action> dictionary;
            var isExist = _binds.TryGetValue(type, out dictionary);
            if (isExist)
                return dictionary;

            dictionary =
                new Dictionary<WeakReference, Action>(WeakReferenceEqualityComparer.BindsComparer);
            _binds.Add(type, dictionary);
            return dictionary;
        }
    }
}