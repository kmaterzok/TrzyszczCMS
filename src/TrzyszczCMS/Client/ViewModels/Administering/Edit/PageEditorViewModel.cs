using Core.Application.Models.Deposits;
using Core.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering.Edit
{
    public class PageEditorViewModel : ViewModelBase
    {
        #region Fields
        private EditedPageDeposit _editedPageDeposit;
        #endregion

        #region Ctor & init
        public PageEditorViewModel(IDataDepository depository)
        {
            this.ApplyDataFromDeposit(depository);
        }

        private void ApplyDataFromDeposit(IDataDepository depository)
        {
            if (!depository.TryGet(out _editedPageDeposit))
            {
                _editedPageDeposit = null;
            }
        }
        #endregion
    }
}
