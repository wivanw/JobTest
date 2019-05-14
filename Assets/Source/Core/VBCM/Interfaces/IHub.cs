// <copyright file="IHub.cs">
// <author>Ivan Bondarenko, ivan.bondarenko@zimad.com</author>
// </copyright>

namespace VBCM.Interfaces
{
    public interface IHub<THub, TSendValue, TCallBackValue> where THub : IHub<THub, TSendValue, TCallBackValue>
    {
        /// <summary>
        /// Usual UI event based control binding
        /// </summary>
        void Bind(Hub<THub,TSendValue,TCallBackValue>.IEventSource source, Hub<THub, TSendValue, TCallBackValue>.IBindable bindable);

        /// <summary>
        /// Usual UI event based control unbinding
        /// </summary>
        void UnBind(Hub<THub,TSendValue,TCallBackValue>.IEventSource source);

        /// <summary>
        /// Used for delayed execution by external event
        /// </summary>
        void BindCallBack(Hub<THub, TSendValue, TCallBackValue>.ICallBackBindable callBack);

        /// <summary>
        /// Unused for delayed execution by external event
        /// </summary>
        void UnBindCallBack(Hub<THub, TSendValue, TCallBackValue>.ICallBackBindable callBack);

        /// <summary>
        /// Add a function of the listener from the Ui event sources
        /// </summary>
        void Controlled(Hub<THub, TSendValue, TCallBackValue>.IHandler handler);

        /// <summary>
        /// Remove a function of the listener from the Ui event sources
        /// </summary>
        void UnControlled(Hub<THub, TSendValue, TCallBackValue>.IHandler handler);

        /// <summary>
        /// Add unit for logic unit validation.
        /// </summary>
        void Validated(Hub<THub, TSendValue, TCallBackValue>.IValidated validated);

        /// <summary>
        /// Remove unit for logic unit validation.
        /// </summary>
        void UnValidated(Hub<THub, TSendValue, TCallBackValue>.IValidated validated);
    }
}