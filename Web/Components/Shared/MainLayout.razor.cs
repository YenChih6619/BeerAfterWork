using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Web.Components.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MainLayout
    {
        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = "";

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem>? Menus { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            Menus = GetIconSideMenuItems();
        }

        private static List<MenuItem> GetIconSideMenuItems()
        {
            var menus = new List<MenuItem>
            {
                new() { Text = "首頁 Index", Icon = "fas fa-wine-glass", Url = "/" },
                new() { Text = "庫存清點", Icon = "fas fa-box", Url = "/Wms_Stock" },
                new() { Text = "收銀功能", Icon = "fas fa-box", Url = "/Sys_cashRegister" },
                new() { Text = "廠商設定", Icon = "fas fa-box", Url = "/Sys_Manufacturerrazor" },
                new() { Text = "訂位功能", Icon = "fas fa-box", Url = "/Sys_Booking" },
                new() { Text = "座位編輯", Icon = "fas fa-box", Url = "/Sys_Table" },
                new() { Text = "單位設定", Icon = "fas fa-box", Url = "/Sys_Unit" }
            };

            return menus;
        }
    }
}
