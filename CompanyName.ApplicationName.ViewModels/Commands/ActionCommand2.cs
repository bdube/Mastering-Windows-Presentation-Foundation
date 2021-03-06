﻿using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CompanyName.ApplicationName.ViewModels.Commands
{
    public class ActionCommand2 : ICommand
    {
        private readonly Action<object> action;
        private readonly Predicate<object> canExecute;
        private List<WeakReference> eventHandlers = new List<WeakReference>();

        /// <summary>
        /// Initialises a new ActionCommand object with the value provided by the input parameter.
        /// </summary>
        /// <param name="action">The Action&gt;object&lt; that will be performed when the command is executed.</param>
        public ActionCommand2(Action<object> action) : this(action, null) { }

        /// <summary>
        /// Initialises a new ActionCommand object with the values provided by the input parameters.
        /// </summary>
        /// <param name="action">The Action&gt;object&gt; that will be performed when the command is executed.</param>
        /// <param name="canExecute">A Predicate&lt;object&gt; that determines whether the Action&gt;object&gt; object specified by the action input parameter can execute or not.</param>
        public ActionCommand2(Action<object> action, Predicate<object> canExecute)
        {
            if (action == null) throw new ArgumentNullException("The action input parameter of the ActionCommand constructor cannot be null.");
            this.action = action;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the related command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                eventHandlers.Add(new WeakReference(value));
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (eventHandlers == null) return;
                for (int i = eventHandlers.Count - 1; i >= 0; i--)
                {
                    WeakReference weakReference = eventHandlers[i];
                    EventHandler handler = weakReference.Target as EventHandler;
                    if (handler == null || handler == value)
                    {
                        eventHandlers.RemoveAt(i);
                    }
                }
                CommandManager.RequerySuggested -= value;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            eventHandlers.ForEach(r => (r.Target as EventHandler)?.Invoke(this, new EventArgs()));
        }

        /// <summary>
        /// Returns a bool value that specifies whether the Action&gt;object&gt; object specified by the action input parameter can execute or not.
        /// </summary>
        /// <param name="parameter">The ICommand.Parameter object that can be used in the method.</param>
        /// <returns>A bool value that specifies whether the Action&gt;object&gt; object specified by the action input parameter can execute or not.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        /// <summary>
        /// Executes the Action&gt;object&gt; object specified by the action input parameter in the constructor if the CanExecute method returns true.
        /// </summary>
        /// <param name="parameter">The ICommand.Parameter object that can be used in the method.</param>
        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}