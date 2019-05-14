// <copyright company="XIM Inc.">
// <author>Sergey Orlov, sergey.orlov@ximxim.com</author>
// <author>Ivan Bondarenko, wivanw@gmail.com</author>
// </copyright>

namespace VBCM.Interfaces
{
    /// <summary>
    /// Main logic validation interface
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Add unit for logic unit validation.
        /// </summary>
        void Add<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.IValidated validated)
            where THub : IHub<THub, TSendValue, TCallBackValue>;

        /// <summary>
        /// Remove unit for logic unit validation.
        /// </summary>
        void Remove<THub, TSendValue, TCallBackValue>(
            Hub<THub, TSendValue, TCallBackValue>.IValidated validated)
            where THub : IHub<THub, TSendValue, TCallBackValue>;

        /// <summary>
        /// Single place for logic unit validation (before executing).
        /// Something line CanExecute in Command pattern
        /// </summary>
        bool IsValidStateOperation<THub, TSendValue, TCallBackValue>(TSendValue sendValue, out string errorMessage)
            where THub : IHub<THub, TSendValue, TCallBackValue>;
    }
}