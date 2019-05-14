using System;

// <copyright company="XIM Inc.">
// <author>Sergey Orlov, sergey.orlov@ximxim.com</author>
// <author>Ivan Bondarenko, wivanw@gmail.com</author>
// </copyright>

namespace VBCM.Interfaces
{
    /// <summary>
    /// Class-manager as main logic operations provider
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Add a function of the listener from the Ui event sources
        /// </summary>
        void Add<THub, TSendValue, TCallBackValue>(Hub<THub, TSendValue, TCallBackValue>.IHandler
            handler)
            where THub : IHub<THub, TSendValue, TCallBackValue>;

        /// <summary>
        /// Remove a function of the listener from the Ui event sources
        /// </summary>
        void Remove<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.IHandler handler)
            where THub : IHub<THub, TSendValue, TCallBackValue>;

        /// <summary>
        /// Main Logic work function
        /// </summary>
        void DoLogicWork<THub, TSendValue, TCallBackValue>(TSendValue sendValue,
            Hub<THub, TSendValue, TCallBackValue>.ActionsPack actionsPack)
            where THub : IHub<THub, TSendValue, TCallBackValue>;

        /// <summary>
        /// Register action for late binding and calculations
        /// </summary>
        void RegCallBack<THub, TSendValue, TCallBackValue>(Action<TCallBackValue> callBack)
            where THub : IHub<THub, TSendValue, TCallBackValue>;

        /// <summary>
        /// Unregister action for late binding and calculations
        /// </summary>
        void UnRegCallBack<THub, TSendValue, TCallBackValue>(Action<TCallBackValue> callBack);

//        /// <summary>
//        /// Register rich (extended) actions for late binding and execution
//        /// </summary>
//        void Register<TEnum>(TEnum logicUnit, Func<object> getParametersUi,
//            Action<object> nonValidAction, Action<object> postActionUi)
//            where TEnum : struct, IConvertible;
    }
}