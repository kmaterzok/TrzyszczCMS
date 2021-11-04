using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TrzyszczCMS.Client.Views.Administering.Edit
{
    public partial class PageEditor
    {
        protected override async Task OnInitializedAsync()
        {
            this.ViewModel.PropertyChanged += new PropertyChangedEventHandler(
                async (s, e) => await this.InvokeAsync(() => this.StateHasChanged())
            );
            await base.OnInitializedAsync();
        }
    }
}
