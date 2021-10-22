using Core.Server.Services.Interfaces.DbAccess.Read;
using Core.Shared.Enums;
using Core.Shared.Models.Rest.Responses.PageContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Server.Services.Implementation.DbAccess.Read
{
    public class LoadPageDbService : ILoadPageDbService
    {
        public async Task<ModularPageContentResponse> GetPageContentAsync(PageType type, string name)
        {
            // TODO: Dummy content
            return await Task.FromResult(new ModularPageContentResponse()
            {
                ModuleContents = new Shared.Models.SiteContent.ModuleContent[]
                {
                    new Shared.Models.SiteContent.ModuleContent()
                    {
                        TextWallModuleContent = new Shared.Models.SiteContent.TextWallModuleContent()
                        {
                            SectionMarkDownContent = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur.",
                            SectionWidth = TextWallSectionWidth._1200,
                            LeftAsideMarkDownContent = null,
                            RightAsideMarkDownContent = null
                        }
                    },
                    new Shared.Models.SiteContent.ModuleContent()
                    {
                        TextWallModuleContent = new Shared.Models.SiteContent.TextWallModuleContent()
                        {
                            SectionMarkDownContent = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur.",
                            SectionWidth = TextWallSectionWidth._1000,
                            LeftAsideMarkDownContent = null,
                            RightAsideMarkDownContent = null
                        }
                    },
                    new Shared.Models.SiteContent.ModuleContent()
                    {
                        TextWallModuleContent = new Shared.Models.SiteContent.TextWallModuleContent()
                        {
                            SectionMarkDownContent = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur.",
                            SectionWidth = TextWallSectionWidth._800,
                            LeftAsideMarkDownContent = null,
                            RightAsideMarkDownContent = null
                        }
                    },
                    new Shared.Models.SiteContent.ModuleContent()
                    {
                        TextWallModuleContent = new Shared.Models.SiteContent.TextWallModuleContent()
                        {
                            SectionMarkDownContent = "**Accusantium consequatur et maiores.** Est quia iste consectetur illum repellendus officia quia quam. Perspiciatis nihil dignissimos est et beatae qui ex ipsa. Dolorem est at molestias aut architecto quis non. Vel unde sequi itaque. Deserunt est numquam quia harum voluptatem excepturi. Sunt expedita sequi veniam optio et voluptate quia voluptatem. Vel corporis deleniti dolorem accusantium. Eos vel modi qui eos. At sed et dolorum ad temporibus vel. Omnis illum quam fugit. Officiis officia quia autem laudantium aut impedit. Asperiores laborum neque quaerat excepturi autem et. Ad id fugit error voluptas sed eum architecto. Error non commodi in delectus. Harum veritatis rerum eum quos et aperiam et vel. Est et nemo similique nam quos necessitatibus tempore commodi. Minima sunt dolorem velit aut omnis eaque repudiandae qui. Atque assumenda voluptas assumenda sint quia deleniti. Corporis aliquam dolorem aut quasi fuga est inventore deleniti. Ratione sit est totam facere libero. In quaerat et consequuntur.",
                            SectionWidth = TextWallSectionWidth._600,
                            LeftAsideMarkDownContent = null,
                            RightAsideMarkDownContent = null
                        }
                    }
                }
            });
        }
    }
}
