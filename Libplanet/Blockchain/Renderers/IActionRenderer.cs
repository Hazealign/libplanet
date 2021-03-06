#nullable enable
using System;
using Libplanet.Action;

namespace Libplanet.Blockchain.Renderers
{
    /// <summary>
    /// Listens state changes of every step of actions, besides blocks,
    /// on a <see cref="BlockChain{T}"/>.
    /// If you need more fine-grained events than <see cref="IRenderer{T}"/>,
    /// implement this interface instead.
    /// </summary>
    /// <typeparam name="T">An <see cref="IAction"/> type.  It should match to
    /// <see cref="BlockChain{T}"/>'s type parameter.</typeparam>
    public interface IActionRenderer<T> : IRenderer<T>
        where T : IAction, new()
    {
        /// <summary>
        /// Does things that should be done right after an <paramref name="action"/>
        /// is executed and applied to the blockchain.
        /// </summary>
        /// <remarks>It is guaranteed to be called only once for an <paramref name="action"/>,
        /// and only after applied to the blockchain, unless an exception is thrown during executing
        /// the <paramref name="action"/> (in that case <see
        /// cref="RenderActionError(IAction, IActionContext, Exception)"/> is called instead) or
        /// once the <paramref name="action"/> has been unrendered.</remarks>
        /// <param name="action">An executed action.</param>
        /// <param name="context">The equivalent context object to an object passed to
        /// the <paramref name="action"/>'s <see cref="IAction.Execute(IActionContext)"/> method.
        /// That means <see cref="IActionContext.PreviousStates"/> are the states right
        /// <em>before</em> this action executed.  For the states after this action executed,
        /// use the <paramref name="nextStates"/> argument instead.</param>
        /// <param name="nextStates">The states right <em>after</em> this action executed,
        /// which means it is equivalent to the states <paramref name="action"/>'s
        /// <see cref="IAction.Execute(IActionContext)"/> method returned.</param>
        /// <remarks>The reason why the parameter <paramref name="action"/> takes
        /// <see cref="IAction"/> instead of <typeparamref name="T"/> is because it can take
        /// block actions (<see cref="Policies.IBlockPolicy{T}.BlockAction"/>) besides transaction
        /// actions (<see cref="Tx.Transaction{T}.Actions"/>).</remarks>
        void RenderAction(IAction action, IActionContext context, IAccountStateDelta nextStates);

        /// <summary>
        /// Does things that should be undone right after the given <paramref name="action"/> is
        /// invalidated (mostly due to reorg, i.e., a block which the action has belonged to becomes
        /// considered stale).
        /// <para>This method takes the equivalent arguments to <see
        /// cref="RenderAction(IAction, IActionContext, IAccountStateDelta)"/> method.</para>
        /// </summary>
        /// <param name="action">A stale action.</param>
        /// <param name="context">The equivalent context object to an object passed to
        /// the <paramref name="action"/>'s <see cref="IAction.Execute(IActionContext)"/> method.
        /// That means <see cref="IActionContext.PreviousStates"/> are the states right
        /// <em>before</em> this action executed.  For the states after this action executed,
        /// use the <paramref name="nextStates"/> argument instead.</param>
        /// <param name="nextStates">The states right <em>after</em> this action executed,
        /// which means it is equivalent to the states <paramref name="action"/>'s
        /// <see cref="IAction.Execute(IActionContext)"/> method returned.</param>
        /// <remarks>As a rule of thumb, this should be the inverse of
        /// <see cref="RenderAction(IAction, IActionContext, IAccountStateDelta)"/> method
        /// with redrawing the graphics on the display at the finish.</remarks>
        /// <remarks>The reason why the parameter <paramref name="action"/> takes
        /// <see cref="IAction"/> instead of <typeparamref name="T"/> is because it can take
        /// block actions (<see cref="Policies.IBlockPolicy{T}.BlockAction"/>) besides transaction
        /// actions (<see cref="Tx.Transaction{T}.Actions"/>).</remarks>
        void UnrenderAction(IAction action, IActionContext context, IAccountStateDelta nextStates);

        /// <summary>
        /// Does the similar things to <see cref=
        /// "RenderAction(IAction, IActionContext, IAccountStateDelta)"/>, except that this method
        /// is invoked when <paramref name="action"/> has terminated with an exception.
        /// </summary>
        /// <param name="action">An action which threw an exception during execution.</param>
        /// <param name="context">The equivalent context object to an object passed to
        /// the <paramref name="action"/>'s <see cref="IAction.Execute(IActionContext)"/> method.
        /// That means <see cref="IActionContext.PreviousStates"/> are the states right
        /// <em>before</em> this action executed.</param>
        /// <param name="exception">The exception thrown during executing the <paramref
        /// name="action"/>.</param>
        /// <remarks>The reason why the parameter <paramref name="action"/> takes
        /// <see cref="IAction"/> instead of <typeparamref name="T"/> is because it can take
        /// block actions (<see cref="Policies.IBlockPolicy{T}.BlockAction"/>) besides transaction
        /// actions (<see cref="Tx.Transaction{T}.Actions"/>).</remarks>
        void RenderActionError(IAction action, IActionContext context, Exception exception);

        /// <summary>
        /// Does the similar things to <see
        /// cref="UnrenderAction(IAction, IActionContext, IAccountStateDelta)"/>, except that
        /// this method is invoked when <paramref name="action"/> has terminated with an exception.
        /// <para>This method takes the equivalent arguments to <see
        /// cref="RenderActionError(IAction, IActionContext, Exception)"/> method.</para>
        /// </summary>
        /// <param name="action">An action which threw an exception during execution.</param>
        /// <param name="context">The equivalent context object to an object passed to
        /// the <paramref name="action"/>'s <see cref="IAction.Execute(IActionContext)"/> method.
        /// That means <see cref="IActionContext.PreviousStates"/> are the states right
        /// <em>before</em> this action executed.</param>
        /// <param name="exception">The exception thrown during executing the <paramref
        /// name="action"/>.</param>
        /// <remarks>The reason why the parameter <paramref name="action"/> takes
        /// <see cref="IAction"/> instead of <typeparamref name="T"/> is because it can take
        /// block actions (<see cref="Policies.IBlockPolicy{T}.BlockAction"/>) besides transaction
        /// actions (<see cref="Tx.Transaction{T}.Actions"/>).</remarks>
        void UnrenderActionError(IAction action, IActionContext context, Exception exception);
    }
}
