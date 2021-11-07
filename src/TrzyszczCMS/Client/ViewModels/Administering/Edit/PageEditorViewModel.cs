using Core.Application.Models.Deposits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Services.Interfaces;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering.Edit
{
    public class PageEditorViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDataDepository _depository;
        private EditedPageDeposit _editedPageDeposit;
        #endregion


        #region Properties
        private bool _isManagingPossible;
        /// <summary>
        /// It says whether there is data allowing to manage a page.
        /// </summary>
        public bool IsManagingPossible
        {
            get => _isManagingPossible;
            set => Set(ref _isManagingPossible, value, nameof(IsManagingPossible));
        }
        #endregion


        #region Ctor & init
        public PageEditorViewModel(IDataDepository depository)
        {
            this._depository = depository;
        }
        public async Task LoadDataFromDeposit()
        {
            _editedPageDeposit = await this._depository.TryGetAsync<EditedPageDeposit>();
            IsManagingPossible = _editedPageDeposit != null;
        }
        #endregion

        #region Methods
        #endregion
    }
}
