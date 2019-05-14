// <copyright company="XIM Inc.">
// <author>Sergey Orlov, sergey.orlov@ximxim.com</author>
// <author>Ivan Bondarenko, wivanw@gmail.com</author>
// </copyright>

namespace VBCM.Interfaces
{
    /// <summary>
    /// Translate logic to IController part
    /// </summary>
    public interface IBinder
    {
        /// <summary>
        /// Usual UI event based control binding
        /// </summary>
        void Bind<THub, TSendValue, TCallBackValue>(Hub<THub,TSendValue,TCallBackValue>.IEventSource eventSource,
            Hub<THub, TSendValue, TCallBackValue>.IBindable bindable)
            where THub : IHub<THub, TSendValue, TCallBackValue>;

        /// <summary>
        /// Usual UI event based control unbinding
        /// </summary>
        void UnBind<THub, TSendValue, TCallBackValue>(Hub<THub,TSendValue,TCallBackValue>.IEventSource eventSource)
            where THub : IHub<THub, TSendValue, TCallBackValue>;

        /// <summary>
        /// Used for delayed execution by external event
        /// </summary>
        void BindCallBack<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.ICallBackBindable callBack)
            where THub : IHub<THub, TSendValue, TCallBackValue>;

        /// <summary>
        /// Unused for delayed execution by external event
        /// </summary>
        void UnBindCallBack<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.ICallBackBindable callBack)
            where THub : IHub<THub, TSendValue, TCallBackValue>;
    }
}