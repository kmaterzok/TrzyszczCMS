﻿using System.ComponentModel;

namespace TrzyszczCMS.Client.ViewModels.Shared
{
    /// <summary>
    /// The base class for creating viewmodels.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Events
        /// <summary>
        /// The event for notifying chagnes in properties.
        /// It should be fired if data in a complementary view must be refreshed to be displayed.
        /// It is recommended to create a method in the view class
        /// and assign it to the event during initialising of the view.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Ctor
        /// <summary>
        /// A simple protected constructor. 
        /// </summary>
        protected ViewModelBase()
        {
            // Do nothing
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Assign a parameter to the value and fire <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">Type of set variable</typeparam>
        /// <param name="field">Updated variable reference</param>
        /// <param name="value">New value</param>
        /// <param name="name">Name of the property</param>
        protected void Set<T>(ref T field, T value, string name)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// Notify change of the property.
        /// </summary>
        /// <param name="name">Name of the property</param>
        protected void NotifyPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}
