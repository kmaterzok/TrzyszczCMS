using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Linq;

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
        /// Assign a parameter to the value using <paramref name="assigner"/> and fire <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="assigner">Action assigning a new value to the specified field or property</param>
        /// <param name="name">Name of the property</param>
        protected void Set(Action assigner, string name)
        {
            assigner.Invoke();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// Notify change of the property.
        /// </summary>
        /// <param name="name">Name of the property</param>
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// Invoke event responsible for notifying changes
        /// for every property defined in the viewmodel.
        /// </summary>
        public void ModelUpdated()
        {
            var properties = this.GetType().GetProperties().Where(p => p.CanWrite && p.CanRead);

            if (properties == null || properties.Count() == 0)
            {
                return;
            }

            foreach (var property in properties)
            {
                this.NotifyPropertyChanged(property.Name);
            }
        }

        // TODO: Add method assigning PorpertyChanged event.
        #endregion
    }
}
