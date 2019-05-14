using System;
using System.Collections.Generic;
using VBCM.Interfaces;
using Zenject;

// <copyright file="Hub.cs">
// <author>Ivan Bondarenko, ivan.bondarenko@zimad.com</author>
// </copyright>

namespace VBCM
{
    public abstract class Hub<THub, TSendValue, TCallBackValue> : IHub<THub, TSendValue, TCallBackValue>
        where THub : IHub<THub, TSendValue, TCallBackValue>
    {

        private IBinder _binder;
        private IValidator _validator;
        private IController _controller;

        [Inject]
        private void Constructor(IBinder binder, IValidator validator, IController controller,
            IList<IHandler> handlers, IList<IValidated> validateds,
            IList<ICallBackBindable> callBackBindables, IList<IEventSource> eventSources, IBindable bindable)
        {
            _binder = binder;
            _validator = validator;
            _controller = controller;
            foreach (var controllable in handlers)
                Controlled(controllable);

            foreach (var validated in validateds)
                Validated(validated);

            foreach (var callBackBindable in callBackBindables)
                BindCallBack(callBackBindable);

            foreach (var eventSource in eventSources)
                Bind(eventSource, bindable);
        }

        /// <inheritdoc />
        public void Bind(IEventSource source, IBindable bindable)
        {
            _binder.Bind(source, bindable);
        }

        /// <inheritdoc />
        public void UnBind(IEventSource source)
        {
            _binder.UnBind(source);
        }

        /// <inheritdoc />
        public void BindCallBack(ICallBackBindable callBack)
        {
            _controller.RegCallBack<THub, TSendValue, TCallBackValue>(callBack.CallBack);
        }

        /// <inheritdoc />
        public void UnBindCallBack(ICallBackBindable callBack)
        {
            _controller.UnRegCallBack<THub, TSendValue, TCallBackValue>(callBack.CallBack);
        }

        /// <inheritdoc />
        public void Validated(IValidated validated)
        {
            _validator.Add(validated);
        }

        /// <inheritdoc />
        public void UnValidated(IValidated validated)
        {
            _validator.Remove(validated);
        }

        /// <inheritdoc />
        public void Controlled(IHandler handler)
        {
            _controller.Add(handler);
        }

        /// <inheritdoc />
        public void UnControlled(IHandler handler)
        {
            _controller.Remove(handler);
        }

        public interface IValidated
        {
            bool Validate(TSendValue sendValue, out string errorMessage);
        }

        public interface IBindable
        {
            ActionsPack BindActions { get; set; }
        }

        public interface ICallBackBindable
        {
            Action<TCallBackValue> CallBack { get; set; }
        }

        public interface IHandler
        {
            TCallBackValue GetCallBackValue(TSendValue sendValue);
        }

        public interface IEventSource
        {
            event Action Event;
        }

        public static BuilderClass Builder(IBindable bindable)
        {
            return new BuilderClass(bindable);
        }

        /// <summary>
        /// It is part of Extended Command in Binder-Controller infrastructure.
        /// See Extended Register(...) Controller method
        /// </summary>
        public sealed class ActionsPack
        {
            #region Properties

            public Func<TSendValue> GetSendValue { get; set; }
            public Action<string> NonValidAction { get; set; }
            public Action<TCallBackValue> CallBack { get; set; }

            #endregion
            
            public ActionsPack()
            {
            }

            public ActionsPack(Func<TSendValue> getSendValue = null, Action<string> nonValidAction = null,
                Action<TCallBackValue> callBack = null)
            {
                GetSendValue = getSendValue ?? EmptyGetSendValue;
                NonValidAction = nonValidAction ?? EmptyNonValidAction;
                CallBack = callBack ?? EmptyCallBack;
            }

            public static TSendValue EmptyGetSendValue()
            {
                return default(TSendValue);
            }

            public static void EmptyNonValidAction(string s)
            {
            }

            public static void EmptyCallBack(TCallBackValue callBackValue)
            {
            }
        }

        public class BuilderClass
        {
            private readonly IBindable _bindable;
            private readonly ActionsPack _actionsPack = new ActionsPack();

            public BuilderClass(IBindable bindable)
            {
                _bindable = bindable;
            }

            /// <summary>
            /// Add a function to get a value from the object that sends the event
            /// </summary>
            public BuilderClass FuncGetSendValue(Func<TSendValue> getSendValue)
            {
                _actionsPack.GetSendValue = getSendValue;
                return this;
            }

            /// <summary>
            /// Add a action of the reaction with negative validation
            /// </summary>
            public BuilderClass NonValidAction(Action<string> nonValidAction)
            {
                _actionsPack.NonValidAction = nonValidAction;
                return this;
            }

            /// <summary>
            /// Add a action CallBack
            /// </summary>
            public BuilderClass CallBack(Action<TCallBackValue> callBack)
            {
                _actionsPack.CallBack = callBack;
                return this;
            }

            public void Build()
            {
                _bindable.BindActions = InitActionPack(_actionsPack);
            }

            private ActionsPack InitActionPack( ActionsPack actionsPack)
            {
                if (actionsPack.GetSendValue == null)
                    actionsPack.GetSendValue = ActionsPack.EmptyGetSendValue;

                if (actionsPack.NonValidAction == null)
                    actionsPack.NonValidAction = ActionsPack.EmptyNonValidAction;

                if (actionsPack.CallBack == null)
                    actionsPack.CallBack = ActionsPack.EmptyCallBack;

                return actionsPack;
            }
        }
    }
}