using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.Enums {
    public enum ConfigEnum {
        RootFolderToken,
        IsHideTitleBarButtonInFullScreen,
        /// <summary>
        /// 合页模式 0: L/R  1: R/L
        /// </summary>
        PageMode,
        /// <summary>
        /// 翻页方向  0: 左向右  1: 右向左  
        /// </summary>
        Direction,
        /// <summary>
        /// 0单页、1双页模式
        /// </summary>
        PageCount,
        /// <summary>
        /// 章节列表每页大小
        /// </summary>
        GroupSize,
        RepairedPageMode,
    }
}
